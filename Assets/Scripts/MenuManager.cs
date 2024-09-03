using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems; // Добавлено для работы с EventSystem

public class MenuManager : MonoBehaviour
{
    // Ссылки на UI-элементы
    public GameObject menuPanel;         // Панель меню
    public Button openMenuButton;        // Кнопка открытия/закрытия меню
    public Slider speedSlider;           // Ползунок для скорости мяча
    public Slider volumeSlider;          // Ползунок для громкости
    public Button mainMenuButton;        // Кнопка выхода в главное меню
    public Button closeButton;           // Кнопка закрытия меню

    private void Start()
    {
        // Отключаем панель меню при старте
        menuPanel.SetActive(false);

        // Подписываем кнопки на методы
        openMenuButton.onClick.AddListener(ToggleMenu);  // Изменено на ToggleMenu
        closeButton.onClick.AddListener(CloseMenu);
        mainMenuButton.onClick.AddListener(ReturnToMainMenu);

        // Устанавливаем начальные значения ползунков
        speedSlider.value = PlayerPrefs.GetFloat("BallSpeed", 5f);
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);

        // Подписываем ползунки на обработчики изменений
        speedSlider.onValueChanged.AddListener(SetBallSpeed);
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    // Метод открытия/закрытия меню
    public void ToggleMenu()
    {
        bool isMenuActive = menuPanel.activeSelf; // Текущее состояние панели меню
        menuPanel.SetActive(!isMenuActive); // Переключаем состояние панели меню

        if (menuPanel.activeSelf)
        {
            // Если меню открыто, сбрасываем выделение с кнопки и останавливаем игру
            EventSystem.current.SetSelectedGameObject(null);
            Time.timeScale = 0f; // Останавливаем игру
        }
        else
        {
            // Если меню закрыто, продолжаем игру
            Time.timeScale = 1f; // Продолжаем игру
        }
    }

    // Метод закрытия меню
    public void CloseMenu()
    {
        menuPanel.SetActive(false); // Отключаем панель меню
        EventSystem.current.SetSelectedGameObject(null); // Сбрасываем выделение с кнопки
        Time.timeScale = 1f; // Продолжаем игру
    }

    // Метод установки скорости мяча
    public void SetBallSpeed(float speed)
    {
        PlayerPrefs.SetFloat("BallSpeed", speed);
        PlayerPrefs.Save();
        Debug.Log("Скорость мяча установлена на: " + speed);
    }

    // Метод установки громкости
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();
        Debug.Log("Громкость установлена на: " + volume);
    }

    // Метод выхода в главное меню
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // Восстанавливаем скорость игры перед выходом
        SceneManager.LoadScene("MainMenu");
    }
}
