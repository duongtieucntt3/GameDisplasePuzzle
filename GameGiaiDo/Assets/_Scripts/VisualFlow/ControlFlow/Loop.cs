using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace VisualFlow
{
    public class Loop : VisualAction, IControlFlow
    {
        [SerializeField] private VisualAction loopAction;

        protected override async UniTask OnExecuting(CancellationToken cancellationToken)
        {
            try
            {
                while (true)
                {
                    await this.loopAction.Execute(cancellationToken);
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
        [MenuItem("GameObject/Visual Flow/Control Flow/Loop", false, -10000)]
        private static void CreateSequence(MenuCommand menuCommand)
        {
            CreateVisualActionHelper.CreateGameObject("Loop", menuCommand).AddComponent<Loop>();
        }
#endif
    }
}