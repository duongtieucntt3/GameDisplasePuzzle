using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public InitializeAds initializeAds;
    public BannerAds bannerAds;
    public InterstitialAds interstitialAds;
    public RewardedAds rewardedAds;

    public static AdsManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        this.LoadAd();

    }
    private async void LoadAd()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        bannerAds.LoadBannerAd();
        interstitialAds.LoadInterstitalAd();
        rewardedAds.LoadRewardedAd();
    }
}
