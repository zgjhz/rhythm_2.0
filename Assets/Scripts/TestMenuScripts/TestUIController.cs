using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.IO.Ports;
using System.IO;
using System.Collections.Generic;
using System;
public class TestUIController : MonoBehaviour
{
    public GameObject soundSettingsPanel;
    public List<AudioClip> metronomAudioClips;
    public AudioSource metronomAudio;

    public List<Toggle> audioToggles;

    public Image darkenBackground;

    private void ShowDarkenBackground()
    {
        darkenBackground.gameObject.SetActive(true); // Включаем Image
        var color = darkenBackground.color;
        color.a = 0.5f; // Устанавливаем полупрозрачный фон (от 0 до 1)
        darkenBackground.color = color;
    }

    private void HideDarkenBackground()
    {
        darkenBackground.gameObject.SetActive(false); // Отключаем Image
    }

    // Start is called before the first frame update
    void Start()
    {
        soundSettingsPanel.SetActive(false);
        if (!PlayerPrefs.HasKey("chosen_sound"))
        {
            PlayerPrefs.SetInt("chosen_sound", 1);
        }
        int toggleIndex = PlayerPrefs.GetInt("chosen_sound") - 1;
        audioToggles[toggleIndex].isOn = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnSettingsButtonClick()
    {
        soundSettingsPanel.SetActive(true);
        int toggleIndex = PlayerPrefs.GetInt("chosen_sound") - 1;
        audioToggles[toggleIndex].isOn = true;
        ShowDarkenBackground();
    }

    public void onToggleValueChanged(int soundNumber)
    {
        PlayerPrefs.SetInt("chosen_sound", soundNumber);
        PlayerPrefs.Save();
    }

    public void ListenSound(int index)
    {
        metronomAudio.clip = metronomAudioClips[index];
        metronomAudio.Play();
    }

    public void HidePanel() {
        HideDarkenBackground();
        soundSettingsPanel.SetActive(false);
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void ReturnToGrandMenu() {
        SceneManager.LoadScene("MainMainMenu");
    }

    public void PlayTest(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}
