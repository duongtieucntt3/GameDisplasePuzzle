using Spine;
using Spine.Unity;
using UnityEngine;

namespace VisualFlow
{
    public class SpineSkinMixer : MonoBehaviour
    {
        [SerializeField] private SkeletonAnimation skeletonAnimation;

        private readonly Skin mixedSkin = new Skin("Mix");

        public void SetSlot(string skinName)
        {
            var skin = this.skeletonAnimation.Skeleton.Data.FindSkin(skinName);
            this.mixedSkin.AddSkin(skin);
            this.skeletonAnimation.Skeleton.SetSkin(this.mixedSkin);
            this.skeletonAnimation.Skeleton.SetSlotsToSetupPose();
            this.skeletonAnimation.AnimationState.Apply(this.skeletonAnimation.Skeleton);
        }

        public void RemoveSkin(string skinName)
        {
            var skin = this.skeletonAnimation.Skeleton.Data.FindSkin(skinName);

            foreach (var attachmentGroup in skin.Attachments)
            {
                var attachment = attachmentGroup.Key;
                this.mixedSkin.RemoveAttachment(attachment.SlotIndex, attachment.Name);
            }

            this.skeletonAnimation.Skeleton.SetSkin(this.mixedSkin);
            this.skeletonAnimation.Skeleton.SetSlotsToSetupPose();
            this.skeletonAnimation.AnimationState.Apply(this.skeletonAnimation.Skeleton);
        }
    }
}