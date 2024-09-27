using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VisualFlow
{
    public class SetEnableComponent : VisualAction
    {
        [SerializeField] private MonoBehaviour component;
        [SerializeField] private bool enable;

        protected override async UniTask OnExecuting(CancellationToken cancellationToken)
        {
            this.component.enabled = this.enable;
            await UniTask.CompletedTask;
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/Visual Flow/Game Object/Set Enable Component", false, -10000)]
        private static void Create(MenuCommand menuCommand)
        {
            CreateVisualActionHelper.CreateGameObject(nameof(SetEnableComponent), menuCommand)
                .AddComponent<SetEnableComponent>();
        }
#endif
    }
}