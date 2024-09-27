using UnityEngine;

public class SetMusic : MonoBehaviour
{
    private AudioManager audioManager;
    [SerializeField] private GameObject imageMusicOn;
    [SerializeField] private GameObject soundOn;
    [SerializeField] private GameObject soundOff;
    [SerializeField] private bool play;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void Music()
    {
        if (play)
        {
            audioManager.StopMusic(); 
            this.imageMusicOn.SetActive(false);
            this.soundOn.SetActive(false);
            this.soundOff.SetActive(true);
            play = false;
        }
        else
        {
            audioManager.PlayMusic();
            this.imageMusicOn.SetActive(true);
            this.soundOn.SetActive(true);
            this.soundOff.SetActive(false);
            play = true;
        }
    }
}
