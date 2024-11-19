using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.IO;

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

    public Image darkenBackground;

    private string filePath;

    private void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "UserStats.csv");
        Debug.Log($"Путь к файлу: {filePath}");

        if (!File.Exists(filePath))
        {
            // Создаем файл и добавляем заголовок
            using (StreamWriter writer = new StreamWriter(filePath, false))
            {
                writer.WriteLine("Username,Metronom_maxStreak,YourRhythm_maxStreak,FrogGame_maxStreak,Ritmamida_maxStreak,ArrowGame_maxStreak,Metronom_PersentHits,YourRhythm_PersentHits,FrogGame_PersentHits,Ritmamida_PersentHits,ArrowGame_PersentHits,TotalScore");
            }
            Debug.Log("CSV файл создан и заголовок добавлен.");
        }

        HideAllPreviews();
        HideDarkenBackground();
        scoreText.text = "Счёт: " + LoadScore();
    }

    // Методы для запуска игровых сцен
    public void PlayMetronom() { SceneManager.LoadScene("Metronom"); }
    public void PlayRitmamida() { SceneManager.LoadScene("Ritmamida"); }
    public void PlayYourRhythm() { SceneManager.LoadScene("YourRhythm"); }
    public void PlayFrogGame() { SceneManager.LoadScene("FrogJump"); }
    public void PlayArrowGame() { SceneManager.LoadScene("ArrowGame"); }

    public void QuitGame()
    {
        Debug.Log("Игра завершена!");
        Application.Quit();
    }

    // Метод для отображения статистики и записи в CSV
    public void ShowStats()
    {
        string username = PlayerPrefs.GetString("current_user");
        HideAllPreviews();
        statsWindow.SetActive(true);
        ShowDarkenBackground();

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

        SaveStatsToCSV(username);
    }

 
      private void SaveStatsToCSV(string username)
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                Debug.Log(username);
                // Получаем данные из PlayerPrefs
                int metronomMaxStreak = PlayerPrefs.GetInt(username + "Metronom_maxStreak");
                int yourRhythmMaxStreak = PlayerPrefs.GetInt(username + "YourRhythm_maxStreak");
                int frogGameMaxStreak = PlayerPrefs.GetInt(username + "FrogGame_maxStreak");
                int ritmamidaMaxStreak = PlayerPrefs.GetInt(username + "ritmamida_maxStreak");
                int arrowGameMaxStreak = PlayerPrefs.GetInt(username + "ArrowGame_maxStreak");

                int metronomPercentHits = PlayerPrefs.GetInt(username + "Metronom_PersentHits");
                int yourRhythmPercentHits = PlayerPrefs.GetInt(username + "YourRhythm_PersentHits");
                int frogGamePercentHits = PlayerPrefs.GetInt(username + "FrogGame_PersentHits");
                int ritmamidaPercentHits = PlayerPrefs.GetInt(username + "ritmamida_PersentHits");
                int arrowGamePercentHits = PlayerPrefs.GetInt(username + "ArrowGame_PersentHits");

                float totalScore = LoadScore();

                // Запись в CSV, включая имя пользователя
                writer.WriteLine($"{username},{metronomMaxStreak},{yourRhythmMaxStreak},{frogGameMaxStreak},{ritmamidaMaxStreak},{arrowGameMaxStreak},{metronomPercentHits},{yourRhythmPercentHits},{frogGamePercentHits},{ritmamidaPercentHits},{arrowGamePercentHits},{totalScore}");
                Debug.Log("Данные успешно записаны в CSV файл.");
            }
        

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

    // Методы для скрытия и показа превью и затемняющего фона
    private void HideAllPreviews()
    {
        metronomPreview.SetActive(false);
        ritmamidaPreview.SetActive(false);
        yourRhythmPreview.SetActive(false);
        frogGamePreview.SetActive(false);
        arrowGamePreview.SetActive(false);
        statsWindow.SetActive(false);
    }

    private void ShowDarkenBackground()
    {
        darkenBackground.gameObject.SetActive(true);
        var color = darkenBackground.color;
        color.a = 0.5f;
        darkenBackground.color = color;
    }

    private void HideDarkenBackground()
    {
        darkenBackground.gameObject.SetActive(false);
    }

    // Методы для показа информации о каждой игре
    public void ShowMetronomInfo() { HideAllPreviews(); metronomPreview.SetActive(true); ShowDarkenBackground(); }
    public void ShowRitmamidaInfo() { HideAllPreviews(); ritmamidaPreview.SetActive(true); ShowDarkenBackground(); }
    public void ShowYourRhythmInfo() { HideAllPreviews(); yourRhythmPreview.SetActive(true); ShowDarkenBackground(); }
    public void ShowFrogGameInfo() { HideAllPreviews(); frogGamePreview.SetActive(true); ShowDarkenBackground(); }
    public void ShowArrowGameInfo() { HideAllPreviews(); arrowGamePreview.SetActive(true); ShowDarkenBackground(); }

    // Закрытие всех панелей и затемнения
    public void ClosePreview() { HideAllPreviews(); HideDarkenBackground(); }
}
