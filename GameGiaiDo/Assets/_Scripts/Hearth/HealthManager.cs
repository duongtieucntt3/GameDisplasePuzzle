using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{   
    public static int health = 5;
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    private void Awake()
    {
        health = PlayerPrefs.GetInt("CurrentHealth", 5);
    }
    private void Update()
    {
        foreach (Image img in hearts)
        {
            img.sprite = emptyHeart;
        }
        for (int i = 0; i < health; i++)
        {
            hearts[i].sprite = fullHeart;
        }
    }
    public void SetHealth(int count)
    {
        health = count;
        PlayerPrefs.SetInt("CurrentHealth", count);
        PlayerPrefs.Save();
    }

}
