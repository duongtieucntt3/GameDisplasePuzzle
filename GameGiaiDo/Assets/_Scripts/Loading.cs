using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField]private Slider _slider;
    [SerializeField]private TMP_Text loadingPercent;
    [SerializeField] private GameObject[] active;
    private int percent;

    private void Start()
    {
        StartCoroutine(Load());
    }

    IEnumerator Load()
    {
        int ran = Random.Range(75, 90);
        while (percent < 150)
        {
            percent++;
            _slider.value = (float)percent / 100;
            if (percent == ran)
            {
                yield return new WaitForSecondsRealtime(0.5f);
            }
            loadingPercent.text = (percent >= 100) ? "100%" : $"{percent}%";
            yield return new WaitForSeconds(0.01f);
        }

        LoadMenu();
        transform.gameObject.SetActive(false);
    }

    private void LoadMenu()
    {
        foreach (GameObject go in active) 
        {
            if (go != null)
            {
                go.gameObject.SetActive(true);
            }
        }
 
    }
}
