using System;
using System.Threading;
using Cysharp.Threading.Tasks;

using Spine;
using Spine.Unity;
using UnityEngine;
using Event = Spine.Event;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VisualFlow
{
    public class WaitSpineAnim : VisualAction
    {
        [SerializeField] private SkeletonAnimation skeletonAnimation;

        [SerializeField, SpineAnimation(dataField = "skeletonAnimation")]
        private new string animation;

        [SerializeField] private int track;
        [SerializeField] private bool loop;

        private bool IsAnimationComplete => this.skeletonAnimation.AnimationState.GetCurrent(this.track) == null ||
                                            this.skeletonAnimation.AnimationState.GetCurrent(this.track).IsComplete;



        protected override async UniTask OnExecuting(CancellationToken cancellationToken)
        {
            this.skeletonAnimation.AnimationState.SetAnimation(this.track, this.animation, this.loop);
            //   skeletonAnimation.AnimationState.GetCurrent().
            if (!this.loop)
            {
                try
                {
                    await UniTask.WaitUntil(() => IsAnimationComplete,
                        PlayerLoopTiming.Update, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                }
            }
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/Visual Flow/Spine/Wait Spine Animation", false, -10000)]
        private static void CreatePlaySpineAnim(MenuCommand menuCommand)
        {
            CreateVisualActionHelper.CreateGameObject(nameof(WaitSpineAnim), menuCommand).AddComponent<WaitSpineAnim>();
        }

#endif
    }
}