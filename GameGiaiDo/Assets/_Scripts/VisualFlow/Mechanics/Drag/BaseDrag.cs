using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Lean.Common;
using Lean.Touch;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

namespace VisualFlow
{
    public abstract class BaseDrag : VisualAction, IMechanic
    {
        [SerializeField, Required]
        private Transform targetTransform;

        [SerializeField, Required]
        private LeanSelectable selectable;

        [SerializeField, Required]
        private Draggable leanDragTranslate;

        [SerializeField] private BaseHeaderGraphic headerGraphic;
        [SerializeField] private UnityEvent onStartDrag;
        [SerializeField] private UnityEvent onEndDrag;
        [SerializeField] private VisualAction startDragAction;
        [SerializeField] private VisualAction endDragAction;
        [SerializeField] private VisualAction dragFailed;
        [SerializeField] private VisualAction dragSucceed;

        [SerializeField] private bool checkFx;


        private bool isDrag;
        private Vector2 checkPos;
        private bool complete;
        private CancellationToken token;
        private Transform dragFxTrans;

        protected Transform TargetTransform => this.targetTransform;
        protected Draggable Draggable => this.leanDragTranslate;

        protected override async UniTask OnInitializing()
        {
            await base.OnInitializing();
            this.leanDragTranslate.enabled = false;

            if (this.targetTransform == null)
            {
                this.targetTransform = this.leanDragTranslate.transform;
            }

            if (this.dragFxTrans == null && checkFx)
            {
                var fxPrefab = Resources.Load<GameObject>("Drag_HighlightFx");
                this.dragFxTrans = Instantiate(fxPrefab).transform;
                this.dragFxTrans.gameObject.SetActive(false);
                this.dragFxTrans.SetParent(transform);
            }
        }

        protected override async UniTask OnExecuting(CancellationToken cancellationToken)
        {
            this.complete = false;
            this.token = cancellationToken;
            this.selectable.OnSelected.AddListener(SelectHandler);
            this.selectable.OnDeselected.AddListener(DeselectHandler);
            LeanTouch.OnFingerUpdate += FingerUpdateHandler;
            LeanTouch.OnFingerUp += FingerUpHandler;

            try
            {
                await UniTask.WaitUntil(() => this.complete, PlayerLoopTiming.Update, cancellationToken);
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception)
            {
                LeanTouch.OnFingerUp -= FingerUpHandler;
            }
            finally
            {
                if (this.dragFxTrans != null && checkFx)
                {
                    this.dragFxTrans.gameObject.SetActive(false);
                }

                this.selectable.OnSelected.RemoveListener(SelectHandler);
                this.selectable.OnDeselected.RemoveListener(DeselectHandler);
                LeanTouch.OnFingerUpdate -= FingerUpdateHandler;
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            if (this.dragFxTrans != null && checkFx)
            {
                this.dragFxTrans.gameObject.SetActive(false);
            }

            this.selectable.OnSelected.RemoveListener(SelectHandler);
            this.selectable.OnDeselected.RemoveListener(DeselectHandler);
            LeanTouch.OnFingerUpdate -= FingerUpdateHandler;
            LeanTouch.OnFingerUp -= FingerUpHandler;
        }

        private async void SelectHandler(LeanSelect select)
        {
            //ServiceLocator.GetService<IVibrationService>().PlayLoopDefault();
            this.leanDragTranslate.enabled = true;
            OnDragStart();
            this.onStartDrag?.Invoke();

            if (this.headerGraphic != null)
            {
                this.headerGraphic.SetActive(true);
            }

            if (checkFx)
            {
                this.dragFxTrans.position = this.targetTransform.position;
                this.dragFxTrans.gameObject.SetActive(true);
            }


            if (this.startDragAction != null && !this.startDragAction.IsExecuting)
            {
                this.startDragAction.Execute(this.token);
            }
        }

        private void DeselectHandler(LeanSelect select)
        {
            this.leanDragTranslate.enabled = false;

            if (this.headerGraphic != null)
            {
                this.headerGraphic.SetActive(false);
            }
        }

        private void FingerUpdateHandler(LeanFinger finger)
        {
            if (!this.selectable.IsSelected) return;
            OnDragUpdate(finger);
            if (checkFx)
            {
                this.dragFxTrans.position = this.targetTransform.position;
            }
        }

        private async void FingerUpHandler(LeanFinger finger)
        {
            if (!this.selectable.IsSelected) return;
            this.selectable.Deselect();
            OnDragEnd(finger);
            this.onEndDrag?.Invoke();
            if (checkFx)
            {
                this.dragFxTrans.gameObject.SetActive(false);
            }

            if (this.endDragAction != null && !this.endDragAction.IsExecuting)
            {
                this.endDragAction.Execute(this.token);
            }
            
        }

        protected void InvokeDragComplete()
        {
            this.complete = true;
        }
        protected async void InvokeDragFailed()
        {
            this.complete = false;
            if (this.dragFailed != null && !this.dragFailed.IsExecuting)
            {
                this.dragFailed.Execute(this.token);
            }
        }

        protected abstract void OnDragStart();
        protected abstract void OnDragUpdate(LeanFinger finger);
        protected abstract void OnDragEnd(LeanFinger finger);
    }
}