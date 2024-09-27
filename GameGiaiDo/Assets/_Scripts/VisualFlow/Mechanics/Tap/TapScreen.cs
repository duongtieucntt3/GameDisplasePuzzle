using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Lean.Touch;
using UnityEditor;

namespace VisualFlow
{
    public class TapScreen : VisualAction, IMechanic
    {
        private bool complete;

        protected override async UniTask OnExecuting(CancellationToken cancellationToken)
        {
            try
            {
                this.complete = false;
                LeanTouch.OnFingerTap += FingerTapHandler;
                await UniTask.WaitUntil(() => this.complete, PlayerLoopTiming.Update, cancellationToken);
            }
            catch (OperationCanceledException )
            {
            }
            finally
            {
                LeanTouch.OnFingerTap -= FingerTapHandler;
            }
        }
        private void FingerTapHandler(LeanFinger finger)
        {
            this.complete = true;
        }
        private void OnDestroy()
        {
            LeanTouch.OnFingerTap -= FingerTapHandler;
            

        }
        private void OnDisable()
        {
            LeanTouch.OnFingerTap -= FingerTapHandler;
        }


#if UNITY_EDITOR
            [MenuItem("GameObject/Visual Flow/Mechanics/Tap Screen", false, -10000)]
        private static void CreateTapArea(MenuCommand menuCommand)
        {
            CreateVisualActionHelper.CreateGameObject("TapScreen", menuCommand).AddComponent<TapScreen>();
        }
#endif
    }
}