using System.Threading;
using Cysharp.Threading.Tasks;
using Spine.Unity;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VisualFlow
{
    public class RemoveSkin : VisualAction
    {
        [SerializeField] private SpineSkinMixer skinMixer;
        [SerializeField] private SkeletonAnimation skeletonAnimation;

        [SerializeField, SpineSkin(dataField = "skeletonAnimation")]
        private string skin;

        protected override async UniTask OnExecuting(CancellationToken cancellationToken)
        {
            this.skinMixer.RemoveSkin(this.skin);
            await UniTask.CompletedTask;
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/Visual Flow/Spine/Remove Skin", false, -10000)]
        private static void CreateRemoveSkin(MenuCommand menuCommand)
        {
            CreateVisualActionHelper.CreateGameObject("RemoveSkin", menuCommand).AddComponent<RemoveSkin>();
        }
#endif
    }
}