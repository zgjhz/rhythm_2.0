//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.SceneManagement;
//using System.Collections;

//public class MenuManager : MonoBehaviour
//{
//    // Ссылки на UI-элементы
//    public GameObject menuPanel;         // Панель меню
//    public Button openMenuButton;        // Кнопка открытия меню
//    public Slider speedSlider;           // Ползунок для скорости мяча
//    public Slider volumeSlider;          // Ползунок для громкости
//    public Button mainMenuButton;        // Кнопка выхода в главное меню
//    public Button closeButton;           // Кнопка закрытия меню
//    public string gameTag = "";
//    public AudioSource metronomSound;

//    public float interval = 1f;
//    private float timer;
//    public bool canClick = true;
//    private bool isSpacePressed = false;
//    public bool isPaused = false; // Добавляем состояние для проверки паузы
//    public float Sucsses_space = 0;
//    public float All_space = 0;
//    private void Start()
//    {
//        // Отключаем панель меню при старте
//        menuPanel.SetActive(false);
//        closeButton.gameObject.SetActive(false);

//        // Подписываем кнопки на методы
//        openMenuButton.onClick.AddListener(OpenMenu);
//        closeButton.onClick.AddListener(CloseMenu);
//        mainMenuButton.onClick.AddListener(ReturnToMainMenu);

//        // Устанавливаем начальные значения ползунков
//        speedSlider.value = PlayerPrefs.GetFloat(gameTag + "_interval", 5f);
//        interval = PlayerPrefs.GetFloat(gameTag + "_interval", 5f);
//        volumeSlider.value = PlayerPrefs.GetFloat(gameTag + "_volume", 1f);

//        // Подписываем ползунки на обработчики изменений
//        speedSlider.onValueChanged.AddListener(SetBallSpeed);
//        volumeSlider.onValueChanged.AddListener(SetVolume);
//        timer = interval;
//    }

//    private void Update()
//    {
//        // Проверяем, не нажата ли клавиша "пробел" и не пауза ли игра
//        if (!isSpacePressed && !isPaused && Input.GetKeyDown(KeyCode.Space))
//        {
//            timer = 0;
//            isSpacePressed = true;
//        }

//        // Если метроном активен
//        if (isSpacePressed)
//        {
//            timer -= Time.deltaTime;

//            if (timer <= 0f)
//            {
//                PlaySound();
//                timer = interval;
//            }
//        }
//    }

//    public void PlaySound()
//    {
//        if (metronomSound != null)
//        {
//            metronomSound.Play();
//        }
//    }

//    // Метод открытия меню
//    public void OpenMenu()
//    {
//        canClick = false;
//        isPaused = true; // Устанавливаем флаг паузы
//        Time.timeScale = 0f;
//        openMenuButton.gameObject.SetActive(false);
//        closeButton.gameObject.SetActive(true);
//        menuPanel.SetActive(true); // Включаем панель меню
//    }

//    // Метод закрытия меню
//    public void CloseMenu()
//    {
//        canClick = true;
//        isPaused = false; // Сбрасываем флаг паузы
//        Time.timeScale = 1f;
//        openMenuButton.gameObject.SetActive(true);
//        closeButton.gameObject.SetActive(false);
//        menuPanel.SetActive(false); // Отключаем панель меню

//        // Ожидаем нажатия пробела для продолжения метронома
//        isSpacePressed = false; // Сбрасываем состояние нажатия пробела
//    }

//    // Метод установки скорости мяча
//    public void SetBallSpeed(float speed)
//    {
//        PlayerPrefs.SetFloat(gameTag + "_interval", speed);
//        PlayerPrefs.Save();
//        timer = speed;
//        interval = speed;
//        Debug.Log("Скорость мяча установлена на: " + speed);
//    }

//    // Метод установки громкости
//    public void SetVolume(float volume)
//    {
//        AudioListener.volume = volume;
//        PlayerPrefs.SetFloat(gameTag + "_volume", volume);
//        PlayerPrefs.Save();
//        Debug.Log("Громкость установлена на: " + volume);
//    }

//    // Метод выхода в главное меню
//    public void ReturnToMainMenu()
//    {
//        SceneManager.LoadScene("MainMenu");
//    }

//    public void StopMetronomeSound()
//    {
//        if (metronomSound != null && metronomSound.isPlaying)
//        {
//            metronomSound.Stop();
//        }
//    }

//}
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    // Ссылки на UI-элементы
    public GameObject menuPanel;         // Панель меню
    public Button openMenuButton;        // Кнопка открытия меню
    public Slider speedSlider;           // Ползунок для скорости мяча
    public Slider volumeSlider;          // Ползунок для громкости
    public Button mainMenuButton;        // Кнопка выхода в главное меню
    public Button closeButton;           // Кнопка закрытия меню
    public TMP_Text scoreText;           // Поле для отображения счёта
    public string gameTag = "";
    public AudioSource metronomSound;

    public float interval = 1f;
    private float timer;
    public bool canClick = true;
    public bool isPaused = false; // Добавляем состояние для проверки паузы

    // Для управления счётом
    public int score = 0; // Счёт
    public int currentStreak = 0; // Текущая серия попаданий
    public int maxStreak = 0;     // Максимальная серия попаданий


    // Для подсчёта нажатий на пробел
    private int spacePressCount = 0; // Счётчик нажатий на пробел
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

        // Инициализируем отображение счёта
        UpdateScoreText();
    }

    private void Update()
    {
        if (isPaused) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CountSpacePress();

            // Запускаем метроном, если меню открыто
            if (canClick)
            {
                print("set isSpacePressed to true");
                //timer = 0; // Сбрасываем таймер
                //PlaySound(); // Воспроизводим звук
                isSpacePressed = true; // Устанавливаем флаг нажатия пробела
            }
        }

        // Если метроном активен
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

    public void PlaySound()
    {
        if (gameTag == "ArrowGame") {
            GameObject arrowControllerObj = GameObject.Find("Arrow Controller");
            arrowControllerObj.GetComponent<ArrowController>().MetronomTicked();
        }
        print("playSound");
        if (metronomSound != null)
        {
            metronomSound.Play();
        }
    }

    // Метод открытия меню
    public void OpenMenu()
    {
        canClick = false;
        isPaused = true; // Устанавливаем флаг паузы
        Time.timeScale = 0f;
        openMenuButton.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(true);
        menuPanel.SetActive(true); // Включаем панель меню
    }

    // Метод закрытия меню
    public void CloseMenu()
    {
        canClick = true;
        isPaused = false; // Сбрасываем флаг паузы
        Time.timeScale = 1f;
        openMenuButton.gameObject.SetActive(true);
        closeButton.gameObject.SetActive(false);
        menuPanel.SetActive(false); // Отключаем панель меню

        // Сбрасываем состояние нажатия пробела
        isSpacePressed = false; // Сбрасываем состояние нажатия пробела
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

    public void StopMetronomeSound()
    {
        if (metronomSound != null && metronomSound.isPlaying)
        {
            metronomSound.Stop();
        }
    }

    // Методы для работы со счётом
    public void UpdateScore()
    {
        score++; // Увеличиваем счёт на 1
        UpdateScoreText(); // Обновляем отображение счёта
        IncrementStreak();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Очки: " + score.ToString(); // Обновляем текст с текущим счётом
        }
    }

    public int GetScore()
    {
        return score; // Возвращаем текущее значение счёта
    }

    // Метод для подсчёта нажатий на пробел
    private void CountSpacePress()
    {
        spacePressCount++; // Увеличиваем счётчик нажатий
        Debug.Log("Пробел нажат " + spacePressCount + " раз.");
    }

    // Метод для получения количества нажатий на пробел
    public int GetSpacePressCount()
    {
        return spacePressCount; // Возвращаем количество нажатий на пробел
    }

    public float GetPersentHits()
    {
        return (score / spacePressCount)*100;
    }
    public void IncrementStreak()
    {
        currentStreak++; // Увеличиваем текущую серию попаданий

        // Обновляем максимальную серию, если текущая серия больше
        if (currentStreak > maxStreak)
        {
            maxStreak = currentStreak;
        }
    }

    // Метод для сброса серии попаданий
    public void ResetStreak()
    {
        currentStreak = 0; // Сбрасываем текущую серию попаданий
    }

    // Метод для получения максимальной серии попаданий
    public int GetMaxStreak()
    {
        return maxStreak; // Возвращаем максимальную серию попаданий
    }
}
