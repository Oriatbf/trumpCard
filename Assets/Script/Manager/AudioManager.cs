using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Inst;
    public AudioSource audioSource_BGM;
    public AudioSource audioSource_SFX;
    public AudioClip[] backGroundAudio;
    public AudioClip[] effectAudioClip;

    private void Awake()
    {
        Inst = this;
        //audioSource = GetComponent<AudioSource>();
    }

    public void AudioPlay(int index)
    {
        audioSource_BGM.clip = backGroundAudio[index];
        audioSource_BGM.Play();
    }

    public void AudioEffectPlay(int index)
    {
        audioSource_SFX.PlayOneShot(effectAudioClip[index]);
    }

}
