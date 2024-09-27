using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VisualFlow
{
    public class CompleteAction : VisualAction, IControlFlow
    {
        [SerializeField] private VisualAction action;

        protected override async UniTask OnExecuting(CancellationToken cancellationToken)
        {
            if (this.action is IMultiFrameAction multiFrameAction)
            {
                multiFrameAction.SetCompleteTrigger(true);
            }

            await UniTask.CompletedTask;
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/Visual Flow/Control Flow/Complete Action", false, -10000)]
        private static void Create(MenuCommand menuCommand)
        {
            CreateVisualActionHelper.CreateGameObject(nameof(CompleteAction), menuCommand)
                .AddComponent<CompleteAction>();
        }
#endif
    }
}