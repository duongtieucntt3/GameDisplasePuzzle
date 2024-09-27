using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowBanner : MonoBehaviour
{
    void Start()
    {
        AdsManager.Instance.bannerAds.ShowBannerAd();
    }

}
