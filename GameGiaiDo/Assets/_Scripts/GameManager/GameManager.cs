using System.Threading;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameWin;
    [SerializeField] private GameObject chienThang;

    private static GameManager instance;
    public static GameManager Instance => instance;

    private void Awake()
    {
        if(GameManager.instance != null) Debug.LogError("Only 1 GameManager allow to exist");
        GameManager.instance = this;
    }
    public void SetGameWin()
    {
        this.gameWin.SetActive(true);
    }
    public void SetChienThang()
    {
        this.chienThang.SetActive(true);
    }


}