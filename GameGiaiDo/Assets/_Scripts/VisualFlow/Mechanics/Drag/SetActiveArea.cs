using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace VisualFlow
{
    public class SetActiveArea : VisualAction
    {
        [SerializeField] private BaseArea dropTarget;
        [SerializeField] private bool active = true;

        protected override async UniTask OnExecuting(CancellationToken cancellationToken)
        {
            this.dropTarget.SetActive(this.active);
            await UniTask.CompletedTask;
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/Visual Flow/Areas/Set Active Area", false, -10000)]
        private static void SetActiveDropTarget(MenuCommand menuCommand)
        {
            CreateVisualActionHelper.CreateGameObject("SetActiveDropTarget", menuCommand).AddComponent<SetActiveArea>();
        }
#endif
    }
}