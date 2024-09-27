using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VisualFlow
{
    public class Parallel : VisualAction, IControlFlow
    {
        private List<VisualAction> actions;

        protected override async UniTask OnInitializing()
        {
            await base.OnInitializing();
            this.actions = GetAllChildActions();
        }

        protected override async UniTask OnExecuting(CancellationToken cancellationToken)
        {
            var tasks = new UniTask[this.actions.Count];

            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = this.actions[i].Execute(cancellationToken);
            }

            try
            {
                await UniTask.WhenAll(tasks).AttachExternalCancellation(cancellationToken);
            }
            catch (OperationCanceledException)
            {
            }
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/Visual Flow/Control Flow/Parallel", false, -10000)]
        private static void CreateParallel(MenuCommand menuCommand)
        {
            CreateVisualActionHelper.CreateGameObject("Parallel", menuCommand).AddComponent<Parallel>();
        }
#endif
    }
}