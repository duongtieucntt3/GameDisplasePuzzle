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
    public class Sequence : VisualAction, IControlFlow
    {
        private List<VisualAction> actions;

        protected override async UniTask OnInitializing()
        {
            await base.OnInitializing();
            this.actions = GetAllChildActions();
            
        }

        protected override async UniTask OnExecuting(CancellationToken cancellationToken)
        {
            try
            { 
                foreach (var action in this.actions)
                {
                    await action.Execute(cancellationToken);

                    if (cancellationToken.IsCancellationRequested)
                    {
                        throw new OperationCanceledException();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/Visual Flow/Control Flow/Sequence", false, -10000)]
        private static void CreateSequence(MenuCommand menuCommand)
        {
            CreateVisualActionHelper.CreateGameObject("Sequence", menuCommand).AddComponent<Sequence>();
        }
#endif
    }
}