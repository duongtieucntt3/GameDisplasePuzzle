using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace VisualFlow
{
    public class WaitDelete : VisualAction, IControlFlow
    {
        [SerializeField] private VisualAction waitAction;

        private List<VisualAction> actions ;
        private List<string> actionsName ;

        protected override async UniTask OnInitializing()
        {
            await base.OnInitializing();
            this.actions = GetAllChildActions();
            
        }

        protected override async UniTask OnExecuting(CancellationToken cancellationToken)
        {
            for (int i = 0; i < this.actions.Count; i++)
            {
                if (this.waitAction.Equals(this.actions[i])) continue;
                await this.actions[i].Execute(cancellationToken);
                this.actionsName.Add(this.actions[i].gameObject.name);
            }

            try
            {
                await this.waitAction.Execute(cancellationToken);
                // for (int i = 0; i < this.actions.Count; i++)
                // {
                //     if(this.actions[i].IsExecuting)Execute(cancellationToken);
                // }
            }
            catch (OperationCanceledException e)
            {
                Debug.LogError("Wait Exception: " + e);
            }
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/Visual Flow/Control Flow/WaitDelete", false, -10000)]
        private static void CreateWait(MenuCommand menuCommand)
        {
            CreateVisualActionHelper.CreateGameObject(nameof(WaitDelete), menuCommand).AddComponent<WaitDelete>();
        }
#endif
    }
}