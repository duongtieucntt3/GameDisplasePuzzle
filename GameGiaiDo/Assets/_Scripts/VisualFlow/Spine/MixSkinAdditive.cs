using System.Threading;
using Cysharp.Threading.Tasks;
using Spine.Unity;
using UnityEngine;
using NaughtyAttributes;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VisualFlow
{
    public class MixSkinAdditive : VisualAction
    {
        [SerializeField, Required] private SpineSkinMixer skinMixer;
        [SerializeField, Required] private SkeletonAnimation skeletonAnimation;

        [SerializeField, SpineSkin(dataField = "skeletonAnimation")]
        private string skinName;

        protected override async UniTask OnExecuting(CancellationToken cancellationToken)
        {
            this.skinMixer.SetSlot(this.skinName);
            await UniTask.CompletedTask;
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/Visual Flow/Spine/Mix Skin Additive", false, -10000)]
        private static void CreateMixSkinAdditive(MenuCommand menuCommand)
        {
            CreateVisualActionHelper.CreateGameObject("MixSpineSkin", menuCommand).AddComponent<MixSkinAdditive>();
        }
#endif
    }
}