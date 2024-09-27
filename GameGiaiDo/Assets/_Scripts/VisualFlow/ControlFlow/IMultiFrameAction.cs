namespace VisualFlow
{
    public interface IMultiFrameAction
    {
        bool CompleteTrigger { get; }

        void SetCompleteTrigger(bool complete);
    }
}