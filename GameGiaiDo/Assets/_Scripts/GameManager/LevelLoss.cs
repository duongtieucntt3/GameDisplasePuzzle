using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VisualFlow;

public class LevelLoss : VisualAction
{
    private AddressableSampleArray addressableSample;
    [SerializeField] private AudioManager audioManager;
    protected override async UniTask OnExecuting(CancellationToken cancellationToken)
    {
        if (HealthManager.health > 1)
        {
            HealthManager.health--;
            PlayerPrefs.SetInt("CurrentHealth", PlayerPrefs.GetInt("CurrentHealth", 5) - 1);
            PlayerPrefs.Save();
            audioManager.PlaySFX(audioManager.death);
            addressableSample.ReloadCurrentLevel();
        }
        else
        {
            audioManager.PlaySFX(audioManager.death);
            //GameManager.Instance.SetGameLose();
            addressableSample.UnLevels();
        }
        await UniTask.CompletedTask;
    }
    private void Awake()
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