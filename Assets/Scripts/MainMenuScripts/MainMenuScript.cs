using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.IO.Ports;
using System.IO;
using System.Collections.Generic;
public class MainMenuScript : MonoBehaviour
{
    // Ссылки на панели превью для каждой игры
    public GameObject metronomPreview;
    public GameObject ritmamidaPreview;
    public GameObject yourRhythmPreview;
    public GameObject frogGamePreview;
    public GameObject arrowGamePreview;
    public GameObject statsWindow;
    public TMP_Text metronomAcc;
    public TMP_Text yourRhythmAcc;
    public TMP_Text frogGameAcc;
    public TMP_Text ritmamidaAcc;
    public TMP_Text ArrowGameAcc;
    public TMP_Text metronomStreak;
    public TMP_Text yourRhythmStreak;
    public TMP_Text frogGameStreak;
    public TMP_Text ritmamidaStreak;
    public TMP_Text ArrowGameStreak;
    public TMP_Text scoreText;
    private SerialPort serialPort;
    public string portName = "COM3"; // �������� COM-�����, ��������, "COM3"
    public int baudRate = 9600; // �������� �������� ������
    public GameObject loginErrorPanel;
    public GameObject soundSettingsPanel;

    public Image statusImage;
    public Sprite disconnected;
    public Sprite connected;
    public Sprite connecting;

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
        Debug.Log("Файл сохраняется в: " + Path.GetFullPath(filePath));
        HideAllPreviews(); // Скрываем все превью при старте
        HideDarkenBackground(); // Скрываем затемняющий фон при старте
        scoreText.text = "Счёт: " + LoadScore();
        statusImage.sprite = connecting;
        loginErrorPanel.SetActive(false);
        soundSettingsPanel.SetActive(false);
        // Попытка подключения к COM-порту с обработкой ошибок
        //try
        //{
        //    serialPort = new SerialPort(portName, baudRate);
        //    serialPort.Open();
        //    serialPort.ReadTimeout = 1000; // Установка таймаута чтения
        //    Debug.Log("Успешное подключение к порту: " + portName);
        //    statusImage.sprite = connected;
        //    isPortOpened = true;
        //}
        //catch (System.IO.IOException e)
        //{
        //    Debug.LogError($"Ошибка подключения к порту {portName}: {e.Message}");
        //    serialPort = null; // Оставляем объект null, чтобы избежать вызовов в Update
        //    statusImage.sprite = disconnected;
        //}
        //catch (System.UnauthorizedAccessException e)
        //{
        //    Debug.LogError($"Доступ к порту {portName} запрещён: {e.Message}");
        //    serialPort = null;
        //    statusImage.sprite = disconnected;
        //}
    }

    // Метод для скрытия всех превью
    private void HideAllPreviews()
    {
        metronomPreview.SetActive(false);
        ritmamidaPreview.SetActive(false);
        yourRhythmPreview.SetActive(false);
        frogGamePreview.SetActive(false);
        arrowGamePreview.SetActive(false);
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

    public void ShowStats()
    {
        string username = PlayerPrefs.GetString("current_user");
        HideAllPreviews();
        statsWindow.SetActive(true);
        ShowDarkenBackground();
        Debug.Log("Metronom_maxStreak" + PlayerPrefs.GetInt(username + "Metronom_maxStreak"));
        metronomStreak.text = "метроном: " + PlayerPrefs.GetInt(username + "Metronom_maxStreak");
        yourRhythmStreak.text = "Твой ритм: " + PlayerPrefs.GetInt(username + "YourRhythm_maxStreak");
        frogGameStreak.text = "ритмогушка: " + PlayerPrefs.GetInt(username + "FrogGame_maxStreak");
        ritmamidaStreak.text = "ритмамида: " + PlayerPrefs.GetInt(username + "ritmamida_maxStreak");
        ArrowGameStreak.text = "почтальон: " + PlayerPrefs.GetInt(username + "ArrowGame_maxStreak");


        metronomAcc.text = "метроном: " + PlayerPrefs.GetInt(username + "Metronom_PersentHits") + "%";
        yourRhythmAcc.text = "Твой ритм: " + PlayerPrefs.GetInt(username + "YourRhythm_PersentHits") + "%";
        frogGameAcc.text = "ритмогушка: " + PlayerPrefs.GetInt(username + "FrogGame_PersentHits") + "%";
        ritmamidaAcc.text = "ритмамида: " + PlayerPrefs.GetInt(username + "ritmamida_PersentHits") + "%";
        ArrowGameAcc.text = "почтальон: " + PlayerPrefs.GetInt(username + "ArrowGame_PersentHits") + "%";
    }




    private float LoadScore()
    {
        string username = PlayerPrefs.GetString("current_user");
        float m = PlayerPrefs.GetFloat(username + "Metronom_score");
        float y = PlayerPrefs.GetFloat(username + "YourRhythm_score");
        float f = PlayerPrefs.GetFloat(username + "FrogGame_score");
        float a = PlayerPrefs.GetFloat(username + "ArrowGame_score");
        float r = PlayerPrefs.GetFloat(username + "Ritmamida_score");
        return m + y + f + a + r;
    }

    // Метод для закрытия превью и затемнения
    public void ClosePreview()
    {
        HideAllPreviews(); // Скрываем все превью
        HideDarkenBackground(); // Отключаем затемнение
    }

}


