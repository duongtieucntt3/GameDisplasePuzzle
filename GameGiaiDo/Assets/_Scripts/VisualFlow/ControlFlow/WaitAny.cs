using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VisualFlow
{
    public class WaitAny : VisualAction, IControlFlow
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
                await UniTask.WhenAny(tasks);
            }
            catch (OperationCanceledException e)
            {
                Debug.LogError("WaitAny Exception: " + e);
            }
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/Visual Flow/Control Flow/Wait Any", false, -10000)]
        private static void CreateWaitAny(MenuCommand menuCommand)
        {
            CreateVisualActionHelper.CreateGameObject("WaitAny", menuCommand).AddComponent<WaitAny>();
        }
#endif
    }
}