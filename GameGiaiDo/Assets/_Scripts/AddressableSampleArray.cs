using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AddressableSampleArray : MonoBehaviour
{
    //public delegate void ClickAction();
    //public static event ClickAction OnClicked;
    [SerializeField] private AssetReference[] _levelPrefabs;
    private int currentLevel = 1;
    private GameObject _currentLevelInstance;
    [SerializeField] private TextMeshProUGUI textLevel;

    private void Awake()
    {
        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
    }
    public void ReloadCurrentLevel()
    {
        UnLevels();
        this.LoadCurrentLevel();
    }
    public async void LoadCurrentLevel()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        LoadLevel(currentLevel);
    }
    public void ButtonPlayLoadLevel()
    {
        currentLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        LoadCurrentLevel();
    }
    public async void UnLevels()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.07f));
        UnloadCurrentLevel();


    }
    public async void OnNextLevelButtonClicked()
    {
        if (currentLevel < _levelPrefabs.Length)
        {
            currentLevel++;
            if(currentLevel % 3 == 0)
            {
                AdsManager.Instance.interstitialAds.ShowInterstitialAd();
            }
            UnLevels();
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
            UnLockNewLevel();
            PlayerPrefs.SetInt("CurrentLevel", currentLevel);
            PlayerPrefs.Save();
            LoadLevel(currentLevel);
        }
    }
    public void RewindLevel()
    {
        if (currentLevel > 5)
        {
            this.LoadLevel(currentLevel - 4);

            PlayerPrefs.SetInt("ReachedIndex", currentLevel - 4);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel") - 4);
            PlayerPrefs.SetInt("CurrentLevel", currentLevel - 4);
            PlayerPrefs.Save();

        }
        else
        {
            this.LoadLevel(1);
            PlayerPrefs.SetInt("ReachedIndex", 1);
            PlayerPrefs.SetInt("UnlockedLevel",1);
            PlayerPrefs.SetInt("CurrentLevel", 1);
            PlayerPrefs.Save();

        }

    }
    public async void LoadLevel(int level)
    {
        await LoadLevelPrefab(level);
        currentLevel = level;
        textLevel.text = currentLevel.ToString();
    }
    private async UniTask LoadLevelPrefab(int level)
    {
        if (level <= 0 || level > _levelPrefabs.Length) return;
        AssetReference assetReference = _levelPrefabs[level - 1];
        if (!assetReference.RuntimeKeyIsValid()) return;
        try
        {
            GameObject result = await assetReference.GetGameObject();
            if (result != null)
            {
                _currentLevelInstance = Instantiate(result);
            }

        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error loading level prefab: {ex.Message}");
        }
    }

    private void UnloadCurrentLevel()
    {
        if (_currentLevelInstance == null) return;
        Destroy(_currentLevelInstance);
        Addressables.ReleaseInstance(_currentLevelInstance);
    }
    private void UnLockNewLevel()
    {
        if (currentLevel >= PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex", currentLevel + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) +1);
            PlayerPrefs.Save();
        }
    }

}

public static class AddressableUniTaskExtensions
{
    public static async UniTask<GameObject> GetGameObject(this AssetReference assetReference)
    {
        var asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>(assetReference);
        GameObject result = await asyncOperationHandle.Task.AsUniTask();
        return result;
    }
}
