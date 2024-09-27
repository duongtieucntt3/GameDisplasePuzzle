using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VisualFlow
{
    public class WaitMs : VisualAction, IControlFlow
    {
        [SerializeField]
        private int ms;

        protected override async UniTask OnExecuting(CancellationToken cancellationToken)
        {
            try
            {
                await UniTask.Delay(this.ms, false, PlayerLoopTiming.Update, cancellationToken);
            }
            catch (OperationCanceledException)
            {
            }
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/Visual Flow/Control Flow/Wait Milliseconds", false, -10000)]
        private static void CreateWaitMs(MenuCommand menuCommand)
        {
            CreateVisualActionHelper.CreateGameObject("WaitMs", menuCommand).AddComponent<WaitMs>();
        }
#endif
    }
}