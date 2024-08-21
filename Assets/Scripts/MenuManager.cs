using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject menuPanel; // Панель с меню
    public Button openMenuButton; // Кнопка для открытия меню
    public Slider speedSlider; // Ползунок для скорости мяча
    public Slider volumeSlider; // Ползунок для громкости
    public Button mainMenuButton; // Кнопка для выхода в главное меню

    private void Start()
    {
        // Отключаем панель меню при старте
        menuPanel.SetActive(false);

        // Подписываем кнопку открытия меню на метод открытия
        openMenuButton.onClick.AddListener(OpenMenu);

        // Устанавливаем начальные значения ползунков
        speedSlider.value = PlayerPrefs.GetFloat("BallSpeed", 5f);
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);

        // Подписываем методы на события ползунков и кнопок
        speedSlider.onValueChanged.AddListener(SetBallSpeed);
        volumeSlider.onValueChanged.AddListener(SetVolume);
        mainMenuButton.onClick.AddListener(ReturnToMainMenu);
    }

    public void OpenMenu()
    {
        // Включаем панель меню
        menuPanel.SetActive(true);
    }

    public void CloseMenu()
    {
        // Отключаем панель меню
        menuPanel.SetActive(false);
    }

    public void SetBallSpeed(float speed)
    {
        PlayerPrefs.SetFloat("BallSpeed", speed);
        PlayerPrefs.Save();
        Debug.Log("Скорость мяча установлена на: " + speed);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();
        Debug.Log("Громкость установлена на: " + volume);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
