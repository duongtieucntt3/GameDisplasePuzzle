using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VisualFlow
{
    public abstract class VisualAction : MonoBehaviour, IDisposable
    {
        [SerializeField] private UnityEvent onEnter;
        [SerializeField] private UnityEvent onExit;

        public bool Completed { private set; get; }
        public bool IsExecuting { private set; get; }

#if UNITY_EDITOR
        public bool HasError { private set; get; }
        public string ErrorMessage { private set; get; }
#endif

        private async void Awake()
        {
            await OnInitializing();
        }
        private void OnDisable()
        {
            Dispose();
        }
        private void OnDestroy()
        {
            Dispose();
        }

        protected virtual async UniTask OnInitializing()
        {
            await UniTask.CompletedTask;
        }

        public async UniTask Execute(CancellationToken cancellationToken)
        {
#if UNITY_EDITOR
            HasError = false;
            EditorGUIUtility.PingObject(gameObject);
#endif

#if UNITY_EDITOR
            try
            {
                await OnEnter(cancellationToken);
                await OnExecuting(cancellationToken);
                await OnExit(cancellationToken);
                //await OnReset(cancellationToken);
            }
            catch (OperationCanceledException )
            {
            }
            catch (Exception e)
            {
                HasError = true;
                ErrorMessage = e.Message;
                Debug.LogException(e);
            }
#else
            await OnEnter(cancellationToken);
            await OnExecuting(cancellationToken);
            await OnExit(cancellationToken);
            //await OnReset(cancellationToken);
#endif
        }

        protected virtual async UniTask OnEnter(CancellationToken cancellationToken)
        {
            Completed = false;
            IsExecuting = true;
            this.onEnter?.Invoke();
            await UniTask.CompletedTask;
        }

        protected virtual async UniTask OnExit(CancellationToken cancellationToken)
        {
            Completed = true;
            IsExecuting = false;
            this.onExit?.Invoke();
            await UniTask.CompletedTask;
        }

        protected virtual async UniTask OnReset(CancellationToken cancellationToken)
        {
            Completed = false;
            IsExecuting = true;
            this.onEnter?.Invoke();
            await UniTask.CompletedTask;
        }
        protected abstract UniTask OnExecuting(CancellationToken cancellationToken);

        public virtual void Dispose()
        {
            this.onEnter.RemoveAllListeners();
            this.onExit.RemoveAllListeners();
        }

        protected List<VisualAction> GetAllChildActions()
        {
            var allActions = new List<VisualAction>();
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);

                if (!child.gameObject.activeSelf) continue;
                var action = child.GetComponent<VisualAction>();

                if (action != null)
                {
                    allActions.Add(action);
                }
            }

            return allActions;
        }
    }
}