using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHome : MonoBehaviour
{
    private Button btn;
    [SerializeField] private UnlockedLevel unlockedLevel;
    [SerializeField] private GameObject go;

    private void Awake()
    { 
        btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(ClickButton);

    }

    private async void ClickButton()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.1));
        go.SetActive(false);
        unlockedLevel.Unlock();
    }
}
