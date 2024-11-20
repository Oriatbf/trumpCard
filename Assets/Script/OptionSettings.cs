using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System;
using EasyTransition;

public class OptionSettings : MonoBehaviour
{
    public Animator anim;
    public AudioMixer audioMixer;
    public Slider bgmSlider;
    public Slider sfxSlider;

    private bool fading;

    void Start()
    {
        bgmSlider.value = PlayerPrefs.GetFloat("BGMParam", 0.75f);  // default 0.75
        sfxSlider.value = PlayerPrefs.GetFloat("SFXParam", 0.75f);  // default 0.75

        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        SetBGMVolume(bgmSlider.value);
        SetSFXVolume(sfxSlider.value);
    }

    public void SetBGMVolume(float volume)
    {
        if (volume == 0)
            audioMixer.SetFloat("BGMParam", -80f); // 최소 음량으로 설정 (음소거)
        else
            audioMixer.SetFloat("BGMParam", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("BGMParam", volume);
    }

    public void SetSFXVolume(float volume)
    {
        if (volume == 0)
            audioMixer.SetFloat("SFXParam", -80f); // 최소 음량으로 설정 (음소거)
        else
            audioMixer.SetFloat("SFXParam", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXParam", volume);
    }

    public void OptionFade(bool fade)
    {
        // InGame Stop
        if (GameObject.Find("Player") != null &&  !fading)
        {
            return;
        }

        fading = fade;
        anim.SetBool("OptionFade", fade);


    }

    public void GiveUp()
    {
        anim.SetBool("OptionFade", false);
        GameManager.Inst.SceneTransition("LobbyScene");
    }

    private void SceneTransition(string v)
    {
        throw new NotImplementedException();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // ���ø����̼� ����
#endif
    }
}
