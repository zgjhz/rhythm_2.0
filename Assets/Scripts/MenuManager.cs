using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{
    // Ссылки на UI-элементы
    public GameObject menuPanel;         // Панель меню
    public Button openMenuButton;        // Кнопка открытия меню
    public Slider speedSlider;           // Ползунок для скорости мяча
    public Slider volumeSlider;          // Ползунок для громкости
    public Button mainMenuButton;        // Кнопка выхода в главное меню
    public Button closeButton;           // Кнопка закрытия менюф
    public TMP_Text scoreText;           // Поле для отображения счёта
    public string gameTag = "";
    public AudioSource metronomSound;
    public float interval = 1f;
    private float timer;
    public bool canClick = true;
    public bool isPaused = false; // Добавляем состояние для проверки паузы
    public bool isLeft = true;

    // Для управления счётом
    public float score = 0; // Счёт
    public int currentStreak = 0; // Текущая серия попаданий
    public int maxStreak = 0;     // Максимальная серия попаданий
    public List<AudioClip> metronomAudioClips;

    // Для подсчёта нажатий на пробел
    private float spacePressCount = 0; // Счётчик нажатий на пробел
    private bool isSpacePressed = false;
    private float firstSpacePressTime = -1f; // Время первого нажатия пробела
    private bool waitingForFirstPress = true; // Флаг ожидания первого нажатия
    public SerialPortReader serialPortReader;
    private void Start()
    {
        int audioIndex = PlayerPrefs.GetInt("chosen_sound") - 1;
        metronomSound.clip = metronomAudioClips[audioIndex];
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

        // Если игрок нажал пробел
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnSpacePressed(); // Вызов нового метода
        }

        // Логика метронома
        if (isSpacePressed)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                if (gameTag == "FrogGame")
                {
                    PlayDirectionalSound(isLeft);
                    isLeft = !isLeft;
                    timer = interval;
                }
                else
                {
                    PlaySound();
                    timer = interval;
                }
            }
        }
    }

    public void OnSpacePressed()
    {
        if (waitingForFirstPress)
        {
            firstSpacePressTime = Time.time; // Фиксируем время первого нажатия
            waitingForFirstPress = false;   // Сбрасываем ожидание
            timer = interval;               // Сбрасываем таймер на интервал
            isSpacePressed = true;          // Активируем метроном
            return;
        }

        CountSpacePress();

        if (canClick)
        {
            isSpacePressed = true; // Активируем метроном
        }
    }


    public void PlaySound()
    {
        //if (metronomSound != null)
        //{
        // Синхронизация метронома после паузы
        if (firstSpacePressTime < 0)
        {
            float elapsed = Time.time - firstSpacePressTime;
            float offset = elapsed % interval; // Рассчитываем сдвиг относительно интервала
            metronomSound.PlayDelayed(interval - offset); // Синхронизируем звук
        }
        else
        {
            metronomSound.Play();
        }
        //}
    }


    public void PlayDirectionalSound(bool isLeft)
    {
        if (metronomSound != null)
        {
            metronomSound.panStereo = isLeft ? 1.0f : -1.0f; // -1 для левого, 1 для правого уха
            metronomSound.Play();
        }
    }

    // Метод открытия меню
    public void OpenMenu()
    {
        canClick = false;
        isLeft = true;
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

        // Устанавливаем ожидание первого нажатия
        waitingForFirstPress = true;
        firstSpacePressTime = -1f; // Сбрасываем время первого нажатия
        isSpacePressed = false; // Сбрасываем состояние нажатия пробела
    }
    public void SetBallSpeed(float speed)
    {
        PlayerPrefs.SetFloat(gameTag + "_interval", speed);
        PlayerPrefs.Save();
        timer = speed;
        interval = speed;
        // Debug.Log("Скорость мяча установлена на: " + speed);
    }    // Метод установки громкости
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
        //serialPortReader.OnApplicationQuitSuka();
        string username = PlayerPrefs.GetString("current_user");
        Debug.Log("USERNAME SAVE " + username);
        float oldScore = PlayerPrefs.GetFloat(username + gameTag + "_score");
        PlayerPrefs.SetFloat(username + gameTag + "_score", score + oldScore);
        PlayerPrefs.Save();
        float oldStreak = PlayerPrefs.GetFloat(username + gameTag + "_score");
        if (oldStreak > maxStreak)
        {
            PlayerPrefs.SetFloat(username + gameTag + "_score", oldStreak);
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.SetFloat(username + gameTag + "_score", maxStreak);
            PlayerPrefs.Save();
        }
        float oldAcc = PlayerPrefs.GetFloat(username + gameTag + "_PersentHits");
        if (oldAcc > (score / spacePressCount) * 100)
        {
            PlayerPrefs.SetInt(username + gameTag + "_PersentHits", Mathf.RoundToInt(oldAcc));
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.SetInt(username + gameTag + "_PersentHits", Mathf.RoundToInt((score / spacePressCount) * 100));
            PlayerPrefs.Save();
        }
        CloseMenu();
        SaveStatsToCSV(username, false);
        SceneManager.LoadScene("MainMenu");
    }

    public void SaveStatsToCSV(string username, bool start)
    {
        // Путь к файлу stats.csv
        string filePath = Path.Combine(Application.dataPath, "stats.csv");

        // Создаем файл, если он не существует
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "Имя;Максимум подряд Метроном;Максимум подряд Твой ритм;Максимум подряд Ритмогушка;Максимум подряд Ритмамида;Максимум подряд Почтальон;Максимум подряд Светофор;Процент попаданий Меторном;Процент попаданий Твой ритм;Процент попаданий Ритмогушка;Процент попаданий Ритмамида;Процент попаданий Почтальон;Процент попаданий Светофор;Общий счет;Дата сессии\n");
            Debug.Log("Файл stats.csv создан с заголовками.");
        }

        // Читаем строки из файла
        List<string> lines = new List<string>(File.ReadAllLines(filePath));

        // Проверяем, если файл пуст или содержит только пустую строку
        if (lines.Count == 0 || (lines.Count == 1 && string.IsNullOrWhiteSpace(lines[0])))
        {
            lines.Add("Имя;Максимум подряд Метроном;Максимум подряд Твой ритм;Максимум подряд Ритмогушка;Максимум подряд Ритмамида;Максимум подряд Почтальон;Максимум подряд Светофор;Процент попаданий Меторном;Процент попаданий Твой ритм;Процент попаданий Ритмогушка;Процент попаданий Ритмамида;Процент попаданий Почтальон;Процент попаданий Светофор;Общий счет;Дата сессии\n");
        }

        // Собираем данные для текущего пользователя
        int metronomMaxStreak = (gameTag == "Metronom") ? PlayerPrefs.GetInt(username + "Metronom_maxStreak", 0) : 0;
        int yourRhythmMaxStreak = (gameTag == "YourRhythm") ? PlayerPrefs.GetInt(username + "YourRhythm_maxStreak", 0) : 0;
        int frogGameMaxStreak = (gameTag == "FrogJump") ? PlayerPrefs.GetInt(username + "FrogGame_maxStreak", 0) : 0;
        int ritmamidaMaxStreak = (gameTag == "Ritmamida") ? PlayerPrefs.GetInt(username + "Ritmamida_maxStreak", 0) : 0;
        int arrowGameMaxStreak = (gameTag == "ArrowGame") ? PlayerPrefs.GetInt(username + "ArrowGame_maxStreak", 0) : 0;
        int svetoforMaxStreak = (gameTag == "Svetofor") ? PlayerPrefs.GetInt(username + "Svetofor_maxStreak", 0) : 0;

        int metronomPercentHits = (gameTag == "Metronom") ? PlayerPrefs.GetInt(username + "Metronom_PersentHits", 0) : 0;
        int yourRhythmPercentHits = (gameTag == "YourRhythm") ? PlayerPrefs.GetInt(username + "YourRhythm_PersentHits", 0) : 0;
        int frogGamePercentHits = (gameTag == "FrogJump") ? PlayerPrefs.GetInt(username + "FrogGame_PersentHits", 0) : 0;
        int ritmamidaPercentHits = (gameTag == "Ritmamida") ? PlayerPrefs.GetInt(username + "Ritmamida_PersentHits", 0) : 0;
        int arrowGamePercentHits = (gameTag == "ArrowGame") ? PlayerPrefs.GetInt(username + "ArrowGame_PersentHits", 0) : 0;
        int svetoforPercentHits = (gameTag == "Svetofor") ? PlayerPrefs.GetInt(username + "Svetofor_PersentHits", 0) : 0;

        float totalScore = (gameTag == "Metronom" || gameTag == "YourRhythm" || gameTag == "FrogJump" || gameTag == "Ritmamida" || gameTag == "ArrowGame" || gameTag == "Svetofor")
            ? LoadScore()
            : 0;

        // Получаем текущую дату
        string sessionDate = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        // Формат строки для записи
        string userStats = $"{username};" +
            $"{(gameTag == "Metronom" ? metronomMaxStreak.ToString() : "-")};" +
            $"{(gameTag == "YourRhythm" ? yourRhythmMaxStreak.ToString() : "-")};" +
            $"{(gameTag == "FrogJump" ? frogGameMaxStreak.ToString() : "-")};" +
            $"{(gameTag == "Ritmamida" ? ritmamidaMaxStreak.ToString() : "-")};" +
            $"{(gameTag == "ArrowGame" ? arrowGameMaxStreak.ToString() : "-")};" +
            $"{(gameTag == "Svetofor" ? svetoforMaxStreak.ToString() : "-")};" +
            $"{(gameTag == "Metronom" ? metronomPercentHits.ToString() : "-")};" +
            $"{(gameTag == "YourRhythm" ? yourRhythmPercentHits.ToString() : "-")};" +
            $"{(gameTag == "FrogJump" ? frogGamePercentHits.ToString() : "-")};" +
            $"{(gameTag == "Ritmamida" ? ritmamidaPercentHits.ToString() : "-")};" +
            $"{(gameTag == "ArrowGame" ? arrowGamePercentHits.ToString() : "-")};" +
            $"{(gameTag == "Svetofor" ? svetoforPercentHits.ToString() : "-")};" +
            $"{(gameTag == "Metronom" || gameTag == "YourRhythm" || gameTag == "FrogJump" || gameTag == "Ritmamida" || gameTag == "ArrowGame" || gameTag == "Svetofor" ? totalScore.ToString() : "-")};" +
            $"{sessionDate}";

        // Проверяем, есть ли пользователь уже в файле
        bool userExists = false;

        for (int i = 1; i < lines.Count; i++) // Начинаем с 1, чтобы пропустить заголовок
        {
            if (lines[i].StartsWith(username + ";"))
            {
                // Если пользователь найден, обновляем его строку
                lines[i] = userStats;
                userExists = true;
                Debug.Log($"Обновлены данные для пользователя: {username}");
                break;
            }
        }
        if (start) userExists = false;

        // Если пользователь не найден, добавляем новую строку
        if (!userExists)
        {
            lines.Add(userStats);
            Debug.Log($"Добавлены данные для нового пользователя: {username}");
        }

        // Записываем обновленные данные обратно в файл
        File.WriteAllLines(filePath, lines.ToArray());
        Debug.Log("Данные сохранены в stats.csv");
    }


    private float LoadScore()
    {
        string username = PlayerPrefs.GetString("current_user");
        float m = PlayerPrefs.GetFloat(username + "Metronom_score");
        float y = PlayerPrefs.GetFloat(username + "YourRhythm_score");
        float f = PlayerPrefs.GetFloat(username + "FrogGame_score");
        float a = PlayerPrefs.GetFloat(username + "ArrowGame_score");
        float r = PlayerPrefs.GetFloat(username + "Ritmamida_score");
        float s = PlayerPrefs.GetFloat(username + "Svetofor_score");
        return m + y + f + a + r + s;
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
        GetPersentHits();
        //Debug.Log(gameTag + "_score");
        UpdateScoreText(); // Обновляем отображение счёта
        IncrementStreak();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Счёт: " + score.ToString(); // Обновляем текст с текущим счётом
        }
    }

    // Метод для подсчёта нажатий на пробел
    private void CountSpacePress()
    {
        spacePressCount++; // Увеличиваем счётчик нажатий
    }

    public void GetPersentHits()
    {
        PlayerPrefs.SetInt(gameTag + "_PersentHits", Mathf.RoundToInt((score / spacePressCount) * 100));
        PlayerPrefs.Save();
        //Debug.Log(spacePressCount);
        // Debug.Log(Mathf.RoundToInt((score / spacePressCount) * 100));

    }
    public void IncrementStreak()
    {
        currentStreak++; // Увеличиваем текущую серию попаданий

        // Обновляем максимальную серию, если текущая серия больше
        if (currentStreak > maxStreak)
        {
            maxStreak = currentStreak;
        }
        string username = PlayerPrefs.GetString("current_user");
        PlayerPrefs.SetInt(username + gameTag + "_maxStreak", maxStreak);
        PlayerPrefs.Save();
    }

    // Метод для сброса серии попаданий
    public void ResetStreak()
    {
        currentStreak = 0; // Сбрасываем текущую серию попаданий
    }


}
