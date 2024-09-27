using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("-----Audio Source-----")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("-----Audio Clip-----")]
    public AudioClip background;
    public AudioClip death;
    public AudioClip win;
    public AudioClip coin;
    public AudioClip eat;
    public AudioClip button;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }
    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
    public void StopMusic()
    {
        musicSource.clip = background;
        musicSource.Stop();
    }
    public void PlayMusic()
    {
        musicSource.clip = background;
        musicSource.Play();
    }
}
