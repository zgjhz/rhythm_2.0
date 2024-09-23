using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    // Ссылки на UI-элементы
    public GameObject menuPanel;         // Панель меню
    public Button openMenuButton;        // Кнопка открытия меню
    public Slider speedSlider;           // Ползунок для скорости мяча
    public Slider volumeSlider;          // Ползунок для громкости
    public Button mainMenuButton;        // Кнопка выхода в главное меню
    public Button closeButton;           // Кнопка закрытия меню
    public string gameTag = "";
    public AudioSource metronomSound;

    public float interval = 1f;
    private float timer;
    public bool canClick = true;
    private bool isSpacePressed = false;


    private void Start()
    {
        // Отключаем панель меню при старте
        menuPanel.SetActive(false);
        closeButton.gameObject.SetActive(false);

        // Подписываем кнопки на методы
        openMenuButton.onClick.AddListener(OpenMenu);
        closeButton.onClick.AddListener(CloseMenu);
        mainMenuButton.onClick.AddListener(ReturnToMainMenu);

        // Устанавливаем начальные значения ползунков
        speedSlider.value = PlayerPrefs.GetFloat(gameTag + "_interval", 5f);
        interval = PlayerPrefs.GetFloat(gameTag + "_interval", 5f);
        volumeSlider.value = PlayerPrefs.GetFloat(gameTag + "_volume", 1f);

        // Подписываем ползунки на обработчики изменений
        speedSlider.onValueChanged.AddListener(SetBallSpeed);
        volumeSlider.onValueChanged.AddListener(SetVolume);
        timer = interval;
    }

    private void Update()
    {
        if (!isSpacePressed && Input.GetKeyDown("space")) {
            timer = 0;
            isSpacePressed = true;
        }
        if (isSpacePressed)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                PlaySound();
                timer = interval;
            }
        }
    }

    void PlaySound()
    {
        if (metronomSound != null)
        {
            metronomSound.Play();
        }
    }

    // Метод открытия меню
    public void OpenMenu()
    {
        canClick = false;
        Time.timeScale = 0f;
        openMenuButton.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(true);
        menuPanel.SetActive(true); // Включаем панель меню
    }

    // Метод закрытия меню
    public void CloseMenu()
    {
        canClick = true;
        Time.timeScale = 1f;
        openMenuButton.gameObject.SetActive(true);
        closeButton.gameObject.SetActive(false);
        menuPanel.SetActive(false); // Отключаем панель меню
    }

    // Метод установки скорости мяча
    public void SetBallSpeed(float speed)
    {
        PlayerPrefs.SetFloat(gameTag + "_interval", speed);
        PlayerPrefs.Save();
        timer = speed;
        interval = speed;
        Debug.Log("Скорость мяча установлена на: " + speed);
    }

    // Метод установки громкости
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat(gameTag + "_volume", volume);
        PlayerPrefs.Save();
        Debug.Log("Громкость установлена на: " + volume);
    }

    // Метод выхода в главное меню
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
