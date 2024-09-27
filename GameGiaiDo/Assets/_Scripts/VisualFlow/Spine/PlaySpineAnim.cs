using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Spine.Unity;
using UnityEditor;
using UnityEngine;

namespace VisualFlow
{
    public class PlaySpineAnim : VisualAction
    {
        [SerializeField] private SkeletonAnimation skeletonAnimation;

        [SerializeField, SpineAnimation(dataField = "skeletonAnimation")]
        private new string animation;

        [SerializeField] private int track;
        [SerializeField] private bool loop;

        protected override async UniTask OnExecuting(CancellationToken cancellationToken)
        {
            this.skeletonAnimation.AnimationState.SetAnimation(this.track, this.animation, this.loop);
            await UniTask.CompletedTask;
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/Visual Flow/Spine/Play Spine Animation", false, -10000)]
        private static void CreatePlaySpineAnim(MenuCommand menuCommand)
        {
            CreateVisualActionHelper.CreateGameObject(nameof(PlaySpineAnim), menuCommand).AddComponent<PlaySpineAnim>();
        }

#endif
    }
}