using UnityEngine;
using UnityEngine.UI;

public class QuitGame : MonoBehaviour
{
    private Button btn;

    private void Awake()
    {
        btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(Quit);
    }
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Game has been exited");
    }
}
