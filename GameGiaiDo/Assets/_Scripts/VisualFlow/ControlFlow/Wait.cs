using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace VisualFlow
{
    public class Wait : VisualAction, IControlFlow
    {
        [SerializeField] private VisualAction waitAction;

        private List<VisualAction> actions;
        private CancellationTokenSource cancellationTokenSource;

        protected override async UniTask OnInitializing()
        {
            await base.OnInitializing();
            this.actions = GetAllChildActions();
            this.cancellationTokenSource = new CancellationTokenSource();
        }

        protected override async UniTask OnExecuting(CancellationToken cancellationToken)
        {
            for (int i = 0; i < this.actions.Count; i++)
            {
                if (this.waitAction.Equals(this.actions[i])) continue;
                await this.actions[i].Execute(this.cancellationTokenSource.Token);
            }

            try
            {
                await this.waitAction.Execute(this.cancellationTokenSource.Token);
            }
            catch (OperationCanceledException e)
            {
                Debug.LogError("Wait Exception: " + e);
            }
            finally
            {
                this.cancellationTokenSource.Cancel();
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            this.cancellationTokenSource.Cancel();
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/Visual Flow/Control Flow/Wait", false, -10000)]
        private static void CreateWait(MenuCommand menuCommand)
        {
            CreateVisualActionHelper.CreateGameObject(nameof(Wait), menuCommand).AddComponent<Wait>();
        }
#endif
    }
}