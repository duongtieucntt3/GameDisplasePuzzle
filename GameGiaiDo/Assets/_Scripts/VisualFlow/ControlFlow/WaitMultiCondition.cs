using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VisualFlow;

public class WaitMultiCondition : VisualAction, IMultiFrameAction, IControlFlow
{
    [SerializeField] private VisualCondition[] condition;
    [SerializeField] private VisualAction action;
    
    protected override async UniTask OnExecuting(CancellationToken cancellationToken)
    {
        await UniTask.WaitUntil(() =>
        {
            return condition.All(a => a.Check());
        }, cancellationToken: cancellationToken);

        if (this.action != null)
        {
            await this.action.Execute(cancellationToken);
        }
    }

    public bool CompleteTrigger { private set; get; }

    public void SetCompleteTrigger(bool complete)
    {
        CompleteTrigger = complete;
    }
}
