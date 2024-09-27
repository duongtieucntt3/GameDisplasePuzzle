using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VisualFlow
{
    public class SetActiveGameObject : VisualAction
    {
        [SerializeField] private GameObject go;
        [SerializeField] private bool active;

        protected override async UniTask OnExecuting(CancellationToken cancellationToken)
        {
            this.go.SetActive(this.active);
            await UniTask.CompletedTask;
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/Visual Flow/Game Object/Set Active", false, -10000)]
        private static void CreateSetActiveGameObject(MenuCommand menuCommand)
        {
            CreateVisualActionHelper.CreateGameObject("SetActiveGameObject", menuCommand)
                .AddComponent<SetActiveGameObject>();
        }

#endif
    }
}