using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Inst;
    AudioSource audioSource;
    public AudioClip[] audioClip;

    private void Awake()
    {
        Inst = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void AudioPlay(int index)
    {
        AudioClip clip = audioClip[index];
        audioSource.Play();
    }

    public void AudioEffectPlay(int index)
    {
        AudioClip clip = audioClip[index];
        audioSource.PlayOneShot(audioClip[index]);
    }

}
