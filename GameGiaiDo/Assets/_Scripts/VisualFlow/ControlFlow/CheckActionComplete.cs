using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VisualFlow
{
    public class CheckActionComplete : VisualAction, IControlFlow
    {
        [SerializeField] private VisualAction completedAction;
        [SerializeField] private VisualAction executedAction;
        [SerializeField] private bool not;

        protected override async UniTask OnExecuting(CancellationToken cancellationToken)
        {
            if (this.not)
            {
                if (!this.completedAction.Completed)
                {
                    await this.executedAction.Execute(cancellationToken);
                }
                else
                {
                    await UniTask.CompletedTask;
                }
            }
            else
            {
                if (this.completedAction.Completed)
                {
                    await this.executedAction.Execute(cancellationToken);
                }
                else
                {
                    await UniTask.CompletedTask;
                }
            }
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/Visual Flow/Control Flow/Check Action Complete", false, -10000)]
        private static void CreateCheckActionComplete(MenuCommand menuCommand)
        {
            CreateVisualActionHelper.CreateGameObject("CheckActionComplete", menuCommand)
                .AddComponent<CheckActionComplete>();
        }
#endif
    }
}