using System.Threading;
using Cysharp.Threading.Tasks;
using Spine.Unity;
using UnityEngine;
using VisualFlow;

public class PlaySpineGraphic : VisualAction
{
    [SerializeField] private SkeletonGraphic skeletonAnimation;

    [SerializeField, SpineAnimation(dataField = "skeletonAnimation")]
    private new string animation;

    [SerializeField] private int track;
    [SerializeField] private bool loop;

    protected override async UniTask OnExecuting(CancellationToken cancellationToken)
    {
        this.skeletonAnimation.AnimationState.SetAnimation(this.track, this.animation, this.loop);
        await UniTask.CompletedTask;
    }
}
