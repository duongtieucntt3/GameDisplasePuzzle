using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;
using Lean.Touch;
using UnityEngine;
using NaughtyAttributes;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VisualFlow
{
    public class TapArea : VisualAction, IPathHint, IMechanic
    {
        [SerializeField, Required]
        private BaseArea target;
        [SerializeField] private bool supportMultiTouch;
                
        private bool complete;

        public Vector3[] Path { private set; get; }

        protected override async UniTask OnInitializing()
        {
            await base.OnInitializing();
            Path = new[] { this.target.Center };
        }

        protected override async UniTask OnExecuting(CancellationToken cancellationToken)
        {
            try
            {
                this.complete = false;
                LeanTouch.OnFingerTap += FingerTapHandler;
                await UniTask.WaitUntil(() => this.complete, PlayerLoopTiming.Update, cancellationToken);
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                LeanTouch.OnFingerTap -= FingerTapHandler;
            }
        }

        private void FingerTapHandler(LeanFinger finger)
        {
            if (finger.IsOverGui) return;
            if (this.supportMultiTouch)
            {
                ProcessTap(finger);
            }
            else
            {
                var isFirstFinger = finger.Index <= 0;

                if (isFirstFinger)
                {
                    ProcessTap(finger);
                }
            }
        }
        private void OnDisable()
        {
            LeanTouch.OnFingerTap -= FingerTapHandler;
        }
        private void ProcessTap(LeanFinger finger)
        {
            var isFingerTapInsideArea = this.target.ContainsScreenPosition(finger.ScreenPosition, Camera.main);
            if (!isFingerTapInsideArea) return;
            this.complete = true;
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/Visual Flow/Mechanics/Tap Area", false, -10000)]
        private static void CreateTapArea(MenuCommand menuCommand)
        {
            var tapArea = CreateVisualActionHelper.CreateGameObject("TapArea", menuCommand).AddComponent<TapArea>();
            BaseArea boxArea = BoxArea.Create(menuCommand);
            boxArea.transform.SetParent(tapArea.transform);
            boxArea.transform.localPosition = Vector3.zero;
            tapArea.GetType()
                .GetField(nameof(target), BindingFlags.Default | BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(tapArea, boxArea);
        }
        
#endif
    }
}