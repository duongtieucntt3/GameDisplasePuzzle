using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using VisualFlow;

namespace Games
{
    [RequireComponent(typeof(Timeline))]
    public class LevelPlayer : MonoBehaviour
    {
        [SerializeField] private Timeline timeline;

        public void Start()
        {
            this.StartPlay();
        }
        public async UniTask Play()
        {
            await this.timeline.Play();
        }

        [Button]
        private async void StartPlay()
        {
            await Play();
        }

        public void Cancel()
        {
            this.timeline.Cancel();
        }
    }
}