using System;
using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Lean.Common;
using Lean.Touch;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VisualFlow
{
    [RequireComponent(typeof(Draggable))]
    public class DragToWorld : VisualAction
    {
        [SerializeField, Required]
        private Transform targetTransform;

        [SerializeField, Required]
        private LeanSelectable selectable;

        [SerializeField]
        private BaseHeaderGraphic headerGraphic;

        [SerializeField, Required]
        private BaseArea targetArea;

        [SerializeField] private VisualAction startDragAction, endDragAction;

        [SerializeField] private UnityEvent startDrag;
        [SerializeField] private UnityEvent completedEvent;
        [SerializeField] private UnityEvent endDrag;

        [SerializeField] private UnityEvent completeEvent;

        [SerializeField] private VisualAction completeDrag;

        [SerializeField] private float scaleGraphic = 1;
        public AudioManager audioManager;

        private bool complete;

        private Transform trans;
        private Vector3 startingPos;
        private Vector3 startingScale;
        private Draggable leanDragTranslate;
        private bool inside;
        private bool isDrag;
        private Vector2 checkPos;
        private CancellationToken cancellationToken;

        protected override async UniTask OnInitializing()
        {
            await base.OnInitializing();
            this.leanDragTranslate = GetComponent<Draggable>();
            this.leanDragTranslate.enabled = false;
            this.trans = transform;
            this.startingPos = this.trans.position;
            this.startingScale = this.targetTransform.transform.localScale;
            this.targetTransform.transform.localScale = Vector3.zero;
            await this.targetTransform.transform.DOScale(Vector3.one * scaleGraphic, 0.4f).SetEase(Ease.OutBack);
            audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        }

        protected override async UniTask OnExecuting(CancellationToken cancellationToken)
        {
            this.cancellationToken = cancellationToken;
            this.selectable.OnSelected.AddListener(SelectHandler);
            this.selectable.OnDeselected.AddListener(DeselectHandler);
            LeanTouch.OnFingerUpdate += FingerUpdateHandler;
            LeanTouch.OnFingerUp += FingerUpHandler;

            try
            {
                await UniTask.WaitUntil(() => complete, PlayerLoopTiming.Update, cancellationToken);
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                this.selectable.OnSelected.RemoveListener(SelectHandler);
                this.selectable.OnDeselected.RemoveListener(DeselectHandler);
                LeanTouch.OnFingerUpdate -= FingerUpdateHandler;
                LeanTouch.OnFingerUp -= FingerUpHandler;
            }
        }

        public void SetCompleted(bool isCompleted)
        {
            complete = isCompleted;
        }

        private async void SelectHandler(LeanSelect select)
        {
            this.targetTransform.gameObject.SetActive(false);
            this.headerGraphic.SetActive(true);
            this.leanDragTranslate.enabled = true;

            if (this.startDragAction != null)
            {
                await this.startDragAction.Execute(this.cancellationToken);
            }

            this.startDrag?.Invoke();
        }

        private async void DeselectHandler(LeanSelect select)
        {
            this.leanDragTranslate.enabled = false;
            this.headerGraphic.SetActive(false);

            if (this.inside)
            {
                complete = true;
                if (this.completeDrag != null)
                {
                    await this.completeDrag.Execute(this.cancellationToken);
                }

                this.completedEvent?.Invoke();
                this.targetArea.Occupy(this.checkPos);
                gameObject.SetActive(false);
                audioManager.PlaySFX(audioManager.button);
            }
            else
            {
                this.targetTransform.gameObject.SetActive(true);
                this.trans.position = this.startingPos;
                this.targetTransform.transform.localScale = Vector3.zero;
                await this.targetTransform.transform.DOScale(this.startingScale, 0.4f).SetEase(Ease.OutBack);
                if (this.endDragAction != null)
                {
                    await this.endDragAction.Execute(this.cancellationToken);
                }

                this.endDrag?.Invoke();
            }
        }

        private void FingerUpdateHandler(LeanFinger finger)
        {
            if (!this.selectable.IsSelected || !this.targetArea.Active) return;
            this.checkPos = this.headerGraphic.Center;
            this.inside = this.targetArea.ContainsWorldSpace(this.checkPos);
        }

        private void FingerUpHandler(LeanFinger finger)
        {
            if (!this.selectable.IsSelected) return;
            this.selectable.Deselect();
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/Visual Flow/Mechanics/Drag To World", false, -10000)]
        private static void CreateDragToWorld(MenuCommand menuCommand)
        {
            var go = new GameObject("DragObject");
            go.transform.position = Vector3.zero;
            var dragObject = go.AddComponent<DragToWorld>();
            var graphicGo = new GameObject("Graphic");
            graphicGo.transform.SetParent(go.transform);
            graphicGo.transform.localPosition = Vector3.zero;
            var spriteRenderer = graphicGo.AddComponent<SpriteRenderer>();
            var selectable = graphicGo.AddComponent<LeanSelectable>();
            var leanDragTranslate = go.GetComponent<LeanDragTranslate>();
            leanDragTranslate.Use.RequiredSelectable = selectable;
            dragObject.GetType().GetField("targetTransform", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.SetValue(dragObject, spriteRenderer);
            dragObject.GetType().GetField("selectable", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.SetValue(dragObject, selectable);
            var boxCol = graphicGo.AddComponent<BoxCollider>();
            boxCol.isTrigger = true;
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, go.GetInstanceID().ToString());
            Selection.activeObject = go;

            SpriteHeader.CreateSpriteHeader(new MenuCommand(go));
            var headerGraphic = go.GetComponentInChildren<BaseHeaderGraphic>();
            dragObject.GetType().GetField("headerGraphic", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.SetValue(dragObject, headerGraphic);
            BoxArea.CreateBoxArea(new MenuCommand(go.transform.parent.gameObject));
        }
#endif
    }
}