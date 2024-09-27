using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VisualFlow;

public class MoveCollider2D : VisualAction
{
    [SerializeField] private Transform[] gameObjects;

    protected override async UniTask OnExecuting(CancellationToken cancellationToken)
    {
        foreach (var item in this.gameObjects)
        {
            item.position = new Vector2(20, 20);
        }

        await UniTask.CompletedTask;
    }
}
