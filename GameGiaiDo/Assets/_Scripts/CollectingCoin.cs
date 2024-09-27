using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class CollectingCoin : MonoBehaviour
{
    [SerializeField] private AddressableSampleArray addressableSampleArray;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private Transform coinParent;
    [SerializeField] private Transform spawnLocation;
    [SerializeField] private Transform endTarget;
    [SerializeField] private TextMeshProUGUI _coinText;
    [SerializeField] private int coinAmount;
    [SerializeField] private float duration;
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;
    List<GameObject> coins = new List<GameObject>();
    private Tween coinReactionTween;
    private int coin;
    private AudioManager audioManager;

    private bool action;
    public bool Action => action;

    private static CollectingCoin instance;
    public static CollectingCoin Instance => instance;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        int coinCount = PlayerPrefs.GetInt("CurrentCoin", 0);
        SetCoin(coinCount);
        if (instance != null) return;
        instance = this;
    }
    [Button]
    public async void CollectCoins()
    {
        // SetCoin(0);
        for (int i = 0; i < coins.Count; i++)
        {
            Destroy(coins[i]);
        }
        coins.Clear();
        List<UniTask> spawnCoinTaskList = new List<UniTask>();
        for (int i = 0; i < coinAmount; i++)
        {
            GameObject coinInstance = Instantiate(coinPrefab, coinParent);
            float xPosition = spawnLocation.position.x + Random.Range(minX, maxX);
            float yPosition = spawnLocation.position.y + Random.Range(minY, maxY);
            coinInstance.transform.position = new Vector3(xPosition, yPosition);
            spawnCoinTaskList.Add(coinInstance.transform.DOPunchPosition(new Vector3(0, 30, 0), Random.Range(0, 1f)).SetEase(Ease.InOutElastic).ToUniTask());
            coins.Add(coinInstance);
            await UniTask.Delay(TimeSpan.FromSeconds(0.01f));
        }
        await UniTask.WhenAll(spawnCoinTaskList);
        await MoveCoinsTask();

    }
    private void SetCoin(int value)
    {
        coin = value;
        _coinText.text = coin.ToString();
        PlayerPrefs.SetInt("CurrentCoin", coin);
        PlayerPrefs.Save();
    }
    private void DeductCoins(int value)
    {
        SetCoin(coin - value);
    }
    public void Hint(int value)
    {
        action = false;
        if (coin < value) return;
        this.DeductCoins(value);
        action = true;

    }
    public void ReloadLevel(int value)
    {
        if (coin < value) return;
        this.DeductCoins(value);
        addressableSampleArray.ReloadCurrentLevel();
    }
    public void NextLevel(int value)
    {
        if (coin < value) return;
        this.DeductCoins(value);
        addressableSampleArray.OnNextLevelButtonClicked();
    }
    private async UniTask MoveCoinsTask()
    {
        List<UniTask> moveCoinTask = new List<UniTask>();
        for (int i = coins.Count - 1; i >= 0; i--)
        {
            moveCoinTask.Add(MovecoinTask(coins[i]));
            await UniTask.Delay(TimeSpan.FromSeconds(0.05f));
        }
    }

    private async UniTask MovecoinTask(GameObject coinInstance)
    {
        await coinInstance.transform.DOMove(endTarget.position, duration).SetEase(Ease.InBack).ToUniTask();
        GameObject temp = coinInstance;
        coins.Remove(coinInstance);
        Destroy(temp);
        await ReactToColectionCoin();
        SetCoin(coin + 1);
        audioManager.PlaySFX(audioManager.coin);
    }

    private async UniTask ReactToColectionCoin()
    {
        if (coinReactionTween == null)
        {
            coinReactionTween = endTarget.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.1f).SetEase(Ease.InOutElastic);
            await coinReactionTween.ToUniTask();
            coinReactionTween = null;
        }
    }

}
