using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hint : MonoBehaviour
{
    [SerializeField] private GameObject hint;
    private Button btn;

    private void Awake()
    {
        btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(LoadHint); 

    }
    private void LoadHint()
    {
        CollectingCoin.Instance.Hint(25);
        if (CollectingCoin.Instance.Action) 
        {
            hint.SetActive(true); 
        }

    }
}
