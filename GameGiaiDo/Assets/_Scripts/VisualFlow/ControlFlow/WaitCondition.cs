using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VisualFlow
{
    public class WaitCondition : VisualAction, IMultiFrameAction, IControlFlow
    {
        [SerializeField] private VisualCondition condition;
        [SerializeField] private VisualAction action;

        protected override async UniTask OnExecuting(CancellationToken cancellationToken)
        {
            await UniTask.WaitUntil(() => this.condition.Check() || CompleteTrigger,
                cancellationToken: cancellationToken);
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

#if UNITY_EDITOR
        [MenuItem("GameObject/Visual Flow/Control Flow/Wait Condition", false, -10000)]
        private static void Create(MenuCommand menuCommand)
        {
            CreateVisualActionHelper.CreateGameObject(nameof(WaitCondition), menuCommand).AddComponent<WaitCondition>();
        }
#endif
    }
}