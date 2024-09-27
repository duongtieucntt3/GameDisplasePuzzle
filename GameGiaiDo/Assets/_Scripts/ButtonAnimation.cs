using UnityEngine;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour
{
    private AudioManager audioManager;
    private Button btn;
    private Vector3 upScale = new Vector3(1.2f, 1.2f, 1f);

    private void Awake()
    {
        btn = gameObject.GetComponent<Button>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        btn.onClick.AddListener(Anim);

    }
    private void Anim()
    {
        LeanTween.scale(gameObject, upScale, 0.1f);
        LeanTween.scale(gameObject, Vector3.one, 0.1f).setDelay(0.1f);
        audioManager.PlaySFX(audioManager.button);
    }
}
