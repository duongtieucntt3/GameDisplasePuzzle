using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Lean.Common;
using Lean.Touch;
using Spine;
using Spine.Unity;
using UnityEngine;
using VisualFlow;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Mechanics.DragAnims
{
    public class DragSpineAnim : VisualAction, IMechanic
    {
        [SerializeField] private BaseArea startArea;
        [SerializeField] private BaseArea endArea;
        [SerializeField] private Orientation orientation;

        [SerializeField] private SkeletonAnimation skeletonAnimation;

        [SerializeField, SpineAnimation(dataField = "skeletonAnimation")]
        private string animation;

        [SerializeField] private bool isLoop;
        [SerializeField] private int track;

        private Camera camera;
        private bool isComplete;
        private bool startDrag;
        private bool reachTarget;

        protected override async UniTask OnInitializing()
        {
            await base.OnInitializing();
            this.camera = Camera.main;
        }

        protected override async UniTask OnExecuting(CancellationToken cancellationToken)
        {
            LeanTouch.OnFingerDown += FingerDownHandler;
            LeanTouch.OnFingerUpdate += FingerUpdateHandler;

            try
            {
                await UniTask.WaitUntil(() => this.isComplete, PlayerLoopTiming.Update, cancellationToken);
            }
            catch (OperationCanceledException e)
            {
            }
            finally
            {
                LeanTouch.OnFingerDown -= FingerDownHandler;
                LeanTouch.OnFingerUpdate -= FingerUpdateHandler;
            }
        }

        private void FingerDownHandler(LeanFinger finger)
        {
            Vector2 pos = finger.GetWorldPosition(20f, this.camera);

            if (this.startArea.ContainsWorldSpace(pos))
            {
                this.startDrag = true;
            }
        }

        private void FingerUpdateHandler(LeanFinger finger)
        {
            if (!this.startDrag || this.isComplete) return;
            Vector2 pos = finger.GetWorldPosition(20f, this.camera);
            var startAreaPos = this.startArea.transform.position;
            var endAreaPos = this.endArea.transform.position;
            var percentage = 0f;

            switch (this.orientation)
            {
                case Orientation.Horizontal:
                    percentage = CalculateHorizontalPercentage(startAreaPos, endAreaPos, pos);
                    break;
                case Orientation.Vertical:
                    percentage = CalculateVerticalPercentage(startAreaPos, endAreaPos, pos);
                    break;
            }

            TrackEntry animationTrack =
                this.skeletonAnimation.AnimationState.SetAnimation(this.track, this.animation, this.isLoop);

            float animationTime = animationTrack.Animation.Duration * percentage;
            animationTrack.TimeScale = 0;
            animationTrack.MixDuration = 0f;
            animationTrack.AnimationStart = animationTime;
            animationTrack.AnimationLast = animationTime;

            if (this.endArea.ContainsWorldSpace(pos))
            {
                this.isComplete = true;
            }
        }

        private static float CalculateHorizontalPercentage(Vector2 startAreaPos, Vector2 endAreaPos, Vector2 pos)
        {
            if (startAreaPos.x > endAreaPos.x)
            {
                if (pos.x <= endAreaPos.x)
                {
                    return 1f;
                }

                if (pos.x >= startAreaPos.x)
                {
                    return 0f;
                }
            }
            else
            {
                if (pos.x >= endAreaPos.x)
                {
                    return 1f;
                }

                if (pos.x <= startAreaPos.x)
                {
                    return 0f;
                }
            }

            return Mathf.Abs(pos.x - startAreaPos.x) / Mathf.Abs(endAreaPos.x - startAreaPos.x);
        }

        private static float CalculateVerticalPercentage(Vector2 startAreaPos, Vector2 endAreaPos, Vector2 pos)
        {
            if (startAreaPos.y > endAreaPos.y)
            {
                if (pos.y <= endAreaPos.y)
                {
                    return 1f;
                }

                if (pos.y >= startAreaPos.y)
                {
                    return 0f;
                }
            }
            else
            {
                if (pos.y >= endAreaPos.y)
                {
                    return 1f;
                }

                if (pos.y <= startAreaPos.y)
                {
                    return 0f;
                }
            }

            return Mathf.Abs(pos.y - startAreaPos.y) / Mathf.Abs(endAreaPos.y - startAreaPos.y);
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/Visual Flow/Mechanics/Drag Spine Anim", false, -10000)]
        private static void CreateDragSpineAnim(MenuCommand menuCommand)
        {
            CreateVisualActionHelper.CreateGameObject("DragSpineAnim", menuCommand).AddComponent<DragSpineAnim>();

            var go = new GameObject("DragSpineAnim");
            go.transform.position = Vector3.zero;
            var dragObject = go.AddComponent<DragSpineAnim>();
            var boxColliderGo = new GameObject("BoxCollider");
            boxColliderGo.transform.SetParent(go.transform);
            boxColliderGo.transform.localPosition = Vector3.zero;
            var boxCol = boxColliderGo.AddComponent<BoxCollider>();
            boxCol.isTrigger = true;
            var selectable = boxColliderGo.AddComponent<LeanSelectable>();
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, go.GetInstanceID().ToString());
            Selection.activeObject = go;
        }
#endif
    }
}