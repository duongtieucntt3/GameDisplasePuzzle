using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
namespace VisualFlow
{
    public class Timeline : MonoBehaviour
    {
        [SerializeField] private VisualAction startingAction;

        private readonly CancellationTokenSource tokenSource = new CancellationTokenSource();

        public async UniTask Play()
        {
            await this.startingAction.Execute(this.tokenSource.Token);
        }

        public void Cancel()
        {
            this.tokenSource.Cancel();
        }
    }

}