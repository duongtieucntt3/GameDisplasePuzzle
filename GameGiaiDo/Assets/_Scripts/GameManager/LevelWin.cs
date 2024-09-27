using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VisualFlow;

public class LevelWin : VisualAction
{
    private AddressableSampleArray addressableSample;
    private AudioManager audioManager;
    protected override async UniTask OnExecuting(CancellationToken cancellationToken)
    {
        audioManager.PlaySFX(audioManager.win);
        GameManager.Instance.SetGameWin();
        addressableSample.UnLevels();
        await UniTask.CompletedTask;
    }
    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        LoadAddressableSampleArray();
    }

    protected virtual void LoadAddressableSampleArray()
    {
        if (this.addressableSample != null) return;
        GameObject go = GameObject.Find("Main Camera");
        this.addressableSample = go.GetComponent<AddressableSampleArray>();
    }
}