using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.IO.Ports;
using System.IO;
using System.Collections.Generic;
using System;
public class MainMenuScript : MonoBehaviour
{
    // Ссылки на панели превью для каждой игры
    public GameObject metronomPreview;
    public GameObject ritmamidaPreview;
    public GameObject yourRhythmPreview;
    public GameObject frogGamePreview;
    public GameObject arrowGamePreview;
    public GameObject svetoforPreview;
    public GameObject statsWindow;
    public TMP_Text metronomAcc;
    public TMP_Text yourRhythmAcc;
    public TMP_Text frogGameAcc;
    public TMP_Text ritmamidaAcc;
    public TMP_Text ArrowGameAcc;
    public TMP_Text SvetoforAcc;
    public TMP_Text metronomStreak;
    public TMP_Text yourRhythmStreak;
    public TMP_Text frogGameStreak;
    public TMP_Text ritmamidaStreak;
    public TMP_Text ArrowGameStreak;
    public TMP_Text SvetoforStreak;
    public TMP_Text scoreText;
    private SerialPort serialPort;
    private string portName; // �������� COM-�����, ��������, "COM3"
    public int baudRate = 38400; // �������� �������� ������
    public GameObject loginErrorPanel;
    public GameObject soundSettingsPanel;
    public GameObject chartPanel;

    public List<AudioClip> metronomAudioClips;
    public AudioSource metronomAudio;

    public Image statusImage;
    public Sprite disconnected;
    public Sprite connected;
    public Sprite connecting;

    private float sessionStartTime;
    private const string TotalTimeKey = "total_time";
    public TMP_Text totalTimeText;
    public TMP_Text sessionTimeText;

    public TMP_Dropdown comportDropdown;

    string[] ports = SerialPort.GetPortNames();

    public List<Toggle> audioToggles;

    private bool isPortOpened = false;
    private string filePath = "stats.csv";
    // Ссылка на затемняющий фон (Image)
    public Image darkenBackground; // Используем Image вместо GameObject

    public void OnSettingsButtonClick() {
        soundSettingsPanel.SetActive(true);
        int toggleIndex = PlayerPrefs.GetInt("chosen_sound") - 1;
        audioToggles[toggleIndex].isOn = true;
    }

    void UpdateDropdownOptions()
    {
        // Получаем доступные порты
        string[] ports = SerialPort.GetPortNames();

        Debug.Log(ports.Length);

        // Очищаем текущие опции Dropdown
        comportDropdown.ClearOptions();

        // Добавляем найденные порты в Dropdown
        if (ports.Length > 0)
        {
            comportDropdown.AddOptions(new System.Collections.Generic.List<string>(ports));
            Debug.Log("Порты добавлены в Dropdown.");
        }
        else
        {
            comportDropdown.AddOptions(new System.Collections.Generic.List<string> { "Нет доступных портов" });
            Debug.Log("Доступных портов не найдено.");
        }
    }

    void ConnectToSelectedPort()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
            Debug.Log("Соединение с предыдущим портом закрыто.");
        }

        string selectedPort = comportDropdown.options[comportDropdown.value].text;
        PlayerPrefs.SetString("Comport", selectedPort);
        PlayerPrefs.Save();
        if (selectedPort == "Нет доступных портов")
        {
            Debug.LogWarning("Выбран невалидный порт.");
            return;
        }

        try
        { 
            serialPort = new SerialPort(selectedPort, baudRate);
            serialPort.Open();
            serialPort.ReadTimeout = 10000; // Установка таймаута чтения
            Debug.Log("Успешное подключение к порту: " + selectedPort);
            statusImage.sprite = connected;
            isPortOpened = true;
        }
        catch (System.IO.IOException e)
        {
            Debug.LogError($"Ошибка подключения к порту {portName}: {e.Message}");
            serialPort = null; // Оставляем объект null, чтобы избежать вызовов в Update
            statusImage.sprite = disconnected;
        }
        catch (System.UnauthorizedAccessException e)
        {
            Debug.LogError($"Доступ к порту {portName} запрещён: {e.Message}");
            serialPort = null;
            statusImage.sprite = disconnected;
        }
    }

    public void onToggleValueChanged(int soundNumber) {
        PlayerPrefs.SetInt("chosen_sound", soundNumber);
        PlayerPrefs.Save();
    }

    // Метод для кнопки "Play"
    public void PlayMetronom()
    {
        if (isPortOpened)
        {
            serialPort.Close();
        }
        string username = PlayerPrefs.GetString("current_user");
        if (username != "")
        {
            SceneManager.LoadScene("Metronom"); // Замени на название твоей игровой сцены
        }
        else
        {
            loginErrorPanel.SetActive(true);
            ShowDarkenBackground();
        }
    }
    public void PlayRitmamida()
    {
        if (isPortOpened)
        {
            serialPort.Close();
        }
        string username = PlayerPrefs.GetString("current_user");
        if (username != "")
        {
            SceneManager.LoadScene("Ritmamida"); // Замени на название твоей игровой сцены
        }
        else
        {
            loginErrorPanel.SetActive(true);
            ShowDarkenBackground();
        }
    }
    public void PlayYourRhythm()
    {
        if (isPortOpened)
        {
            serialPort.Close();
        }
        string username = PlayerPrefs.GetString("current_user");
        if (username != "")
        {
            SceneManager.LoadScene("YourRhythm"); // Замени на название твоей игровой сцены
        }
        else
        {
            loginErrorPanel.SetActive(true);
            ShowDarkenBackground();
        }
    }
    public void PlaySvetofor()
    {
        if (isPortOpened)
        {
            serialPort.Close();
        }
        string username = PlayerPrefs.GetString("current_user");
        if (username != "")
        {
            SceneManager.LoadScene("Svetofor"); // Замени на название твоей игровой сцены
        }
        else
        {
            loginErrorPanel.SetActive(true);
            ShowDarkenBackground();
        }
    }
    public void PlayFrogGame()
    {
        if (isPortOpened)
        {
            serialPort.Close();
        }
        string username = PlayerPrefs.GetString("current_user");
        if (username != "")
        {
            SceneManager.LoadScene("FrogJump"); // Замени на название твоей игровой сцены
        }
        else
        {
            loginErrorPanel.SetActive(true);
            ShowDarkenBackground();
        }
    }
    public void PlayArrowGame()
    {
        if (isPortOpened)
        {
            serialPort.Close();
        }
        string username = PlayerPrefs.GetString("current_user");
        if (username != "")
        {
            SceneManager.LoadScene("ArrowGame"); // Замени на название твоей игровой сцены
        }
        else
        {
            loginErrorPanel.SetActive(true);
            ShowDarkenBackground();
        }
    }

    // Метод для кнопки "Exit"
    public void QuitGame()
    {
        Debug.Log("Игра завершена!"); // Сообщение для проверки в редакторе



        Application.Quit(); // Работает только в билде игры
    }

    public void onCloseButtonClicked()
    {
        loginErrorPanel.SetActive(false);
        HideDarkenBackground();
    }

    public void closeSoundSettingsPanel() {
        soundSettingsPanel.SetActive(false);
    }

    // Добавляем метод Start() для скрытия панелей и затемнения при старте игры
    private void Start()
    {
        // Обновляем текст общего времени
        UpdateTimeTexts();
        UpdateDropdownOptions();
        comportDropdown.onValueChanged.AddListener(delegate { ConnectToSelectedPort(); });
        Debug.Log("Файл сохраняется в: " + Path.GetFullPath(filePath));
        HideAllPreviews(); // Скрываем все превью при старте
        HideDarkenBackground(); // Скрываем затемняющий фон при старте
        scoreText.text = "Счёт: " + LoadScore();
        statusImage.sprite = connecting;
        loginErrorPanel.SetActive(false);
        soundSettingsPanel.SetActive(false);
        chartPanel.SetActive(false);
        if (!PlayerPrefs.HasKey("chosen_sound"))
        {
            PlayerPrefs.SetInt("chosen_sound", 1);
        }
        int toggleIndex = PlayerPrefs.GetInt("chosen_sound") - 1;
        audioToggles[toggleIndex].isOn = true;
        PlayerPrefs.SetString("current_user", "пользователь");
        PlayerPrefs.Save();
        // Попытка подключения к COM-порту с обработкой ошибок
    }

    public void ListenSound(int index) {
        metronomAudio.clip = metronomAudioClips[index];
        metronomAudio.Play();
    }

    private void Update()
    {
        // Обновляем текст времени текущей сессии
        float sessionTime = TimeTracker.Instance.GetCurrentSessionTime();
        sessionTimeText.text = "Время сессии: " + FormatTime(sessionTime);
    }

    private void UpdateTimeTexts()
    {
        // Обновляем общее время
        float totalTime = TimeTracker.Instance.GetTotalGameTime();
        totalTimeText.text = "Общее время: " + FormatTime(totalTime);
    }

    private string FormatTime(float timeInSeconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(timeInSeconds);
        return string.Format("{0:D2}:{1:D2}:{2:D2}", time.Hours, time.Minutes, time.Seconds);
    }

    private void OnApplicationQuit()
    {
        // Сохраняем время сессии при выходе
        TimeTracker.Instance.SaveSessionTime();
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
            Debug.Log("Порт закрыт при выходе из приложения.");
        }
    }

    // Метод для скрытия всех превью
    private void HideAllPreviews()
    {
        metronomPreview.SetActive(false);
        ritmamidaPreview.SetActive(false);
        yourRhythmPreview.SetActive(false);
        frogGamePreview.SetActive(false);
        arrowGamePreview.SetActive(false);
        svetoforPreview.SetActive(false);
        statsWindow.SetActive(false);
    }

    // Показ затемняющего фона (делаем активным и меняем прозрачность)
    private void ShowDarkenBackground()
    {
        darkenBackground.gameObject.SetActive(true); // Включаем Image
        var color = darkenBackground.color;
        color.a = 0.5f; // Устанавливаем полупрозрачный фон (от 0 до 1)
        darkenBackground.color = color;
    }

    // Скрытие затемняющего фона (деактивируем объект)
    private void HideDarkenBackground()
    {
        darkenBackground.gameObject.SetActive(false); // Отключаем Image
    }


    // Методы для открытия превью по кнопке "Info" с затемнением
    public void ShowMetronomInfo()
    {
        HideAllPreviews(); // Скрываем другие панели
        metronomPreview.SetActive(true); // Показываем нужную
        ShowDarkenBackground(); // Включаем затемнение
    }

    public void ShowRitmamidaInfo()
    {
        HideAllPreviews();
        ritmamidaPreview.SetActive(true);
        ShowDarkenBackground();
    }

    public void ShowYourRhythmInfo()
    {
        HideAllPreviews();
        yourRhythmPreview.SetActive(true);
        ShowDarkenBackground();
    }

    public void ShowFrogGameInfo()
    {
        HideAllPreviews();
        frogGamePreview.SetActive(true);
        ShowDarkenBackground();
    }

    public void ShowArrowGameInfo()
    {
        HideAllPreviews();
        arrowGamePreview.SetActive(true);
        ShowDarkenBackground();
    }

    public void ShowSvetoforInfo()
    {
        HideAllPreviews();
        svetoforPreview.SetActive(true);
        ShowDarkenBackground();
    }

    public void ShowStats()
    {
        string username = PlayerPrefs.GetString("current_user");
        HideAllPreviews();
        statsWindow.SetActive(true);
        ShowDarkenBackground();
        metronomStreak.text = "метроном: " + PlayerPrefs.GetInt(username + "Metronom_maxStreak", 0);
        yourRhythmStreak.text = "Твой ритм: " + PlayerPrefs.GetInt(username + "YourRhythm_maxStreak", 0);
        frogGameStreak.text = "ритмогушка: " + PlayerPrefs.GetInt(username + "FrogGame_maxStreak", 0);
        ritmamidaStreak.text = "ритмамида: " + PlayerPrefs.GetInt(username + "Ritmamida_maxStreak", 0);
        ArrowGameStreak.text = "почтальон: " + PlayerPrefs.GetInt(username + "ArrowGame_maxStreak", 0);
        SvetoforStreak.text = "светофор: " + PlayerPrefs.GetInt(username + "Svetofor_maxStreak", 0);


        metronomAcc.text = "метроном: " + PlayerPrefs.GetInt(username + "Metronom_PersentHits", 0) + "%";
        yourRhythmAcc.text = "Твой ритм: " + PlayerPrefs.GetInt(username + "YourRhythm_PersentHits", 0) + "%";
        frogGameAcc.text = "ритмогушка: " + PlayerPrefs.GetInt(username + "FrogGame_PersentHits", 0) + "%";
        ritmamidaAcc.text = "ритмамида: " + PlayerPrefs.GetInt(username + "Ritmamida_PersentHits", 0) + "%";
        ArrowGameAcc.text = "почтальон: " + PlayerPrefs.GetInt(username + "ArrowGame_PersentHits", 0) + "%";
        SvetoforAcc.text = "светофор: " + PlayerPrefs.GetInt(username + "Svetofor_PersentHits", 0) + "%";
    }




    private float LoadScore()
    {
        string username = PlayerPrefs.GetString("current_user");
        float m = PlayerPrefs.GetFloat(username + "Metronom_score", 0);
        float y = PlayerPrefs.GetFloat(username + "YourRhythm_score", 0);
        float f = PlayerPrefs.GetFloat(username + "FrogGame_score", 0);
        float a = PlayerPrefs.GetFloat(username + "ArrowGame_score", 0);
        float r = PlayerPrefs.GetFloat(username + "Ritmamida_score", 0);
        float s = PlayerPrefs.GetFloat(username + "Svetofor_score", 0);
        return m + y + f + a + r + s;
    }

    // Метод для закрытия превью и затемнения
    public void ClosePreview()
    {
        HideAllPreviews(); // Скрываем все превью
        HideDarkenBackground(); // Отключаем затемнение
    }

}