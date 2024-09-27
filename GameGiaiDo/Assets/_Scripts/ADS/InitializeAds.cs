using UnityEngine;
using UnityEngine.Advertisements;

public class InitializeAds : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] private string androiGameId;
    [SerializeField] private string iosGameId;
    [SerializeField] private bool isTesting;
    private string gameId;

    private void Awake()
    {
    #if UNITY_IOS
            gameId = iosGameId;
    #elif UNITY_ANDROID
            gameId = androiGameId;
    #elif UNITY_EDITOR
            gameId = androiGameId; // If you havn't switched the platfrom...
    #endif

        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(gameId, isTesting, this);
        }
    }
    public void OnInitializationComplete()
    {
        Debug.Log("Ads Initialized...");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
    }
}
