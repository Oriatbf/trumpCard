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
        audioMixer.SetFloat("BGMParam", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("BGMParam", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXParam", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXParam", volume);
    }

    public void OptionFade(bool fade)
    {
        // InGame Stop
        if (GameObject.Find("Player") != null && !GameManager.Inst.isGameStart && !fading)
        {
            return;
        }

        fading = fade;
        anim.SetBool("OptionFade", fade);

        // InGame Stop
        if (GameObject.Find("Player") != null)
            GameManager.Inst.isGameStart = !fade;
    }

    public void GiveUp()
    {
        DemoLoadScene.Inst.LoadScene("LobbyScene");
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
        Application.Quit(); // 어플리케이션 종료
#endif
    }
}
