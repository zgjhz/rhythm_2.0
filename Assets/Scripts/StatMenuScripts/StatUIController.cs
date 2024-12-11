using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System;

public class StatUIController : MonoBehaviour
{
    public TMP_Dropdown userDropdown; // Ссылка на ваш Dropdown
    private string csvFilePath = Path.Combine(Application.dataPath, "stats.csv");
    private List<string> userNames;

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

    public GameObject nextButton;
    public GameObject chartPanel;

    void Start()
    {
        LoadUsersFromCSV();
        nextButton.SetActive(false);
        chartPanel.SetActive(false);
    }

    public void OnExitButtonClick() {
        SceneManager.LoadScene("MainMainMenu");
    }

    void LoadUsersFromCSV()
    {
        // Проверяем, существует ли файл
        if (!File.Exists(csvFilePath))
        {
            Debug.LogError($"CSV file not found at: {csvFilePath}");
            return;
        }

        // Считываем строки из файла
        string[] lines = File.ReadAllLines(csvFilePath);

        // HashSet для уникальных имен
        HashSet<string> uniqueUserNames = new HashSet<string>();

        // Проходим по каждой строке
        foreach (string line in lines)
        {
            if (line.Contains("username")) {
                continue;
            }
            string[] values = line.Split(';');

            // Проверяем, что колонка имени существует
            if (values.Length > 0)
            {
                uniqueUserNames.Add(values[0].Trim());
            }
        }

        // Очищаем элементы Dropdown и добавляем уникальные имена
        userDropdown.ClearOptions();
        userNames = new List<string>(uniqueUserNames);
        userDropdown.AddOptions(userNames);
    }

    public void OnUserSelected(int index) {
        ShowStats(index);
        ShowButtons();
        chartPanel.SetActive(false);
    }

    public void ShowButtons() {
        nextButton.SetActive(true);
        //prevButton.SetActive(true);
    }

    public void ShowStats(int index)
    {
        string username = userNames[index];
        metronomStreak.text = "Метроном: " + PlayerPrefs.GetInt(username + "Metronom_maxStreak", 0);
        yourRhythmStreak.text = "Твой ритм: " + PlayerPrefs.GetInt(username + "YourRhythm_maxStreak", 0);
        frogGameStreak.text = "Ритмогушка: " + PlayerPrefs.GetInt(username + "FrogGame_maxStreak", 0);
        ritmamidaStreak.text = "Ритмамида: " + PlayerPrefs.GetInt(username + "Ritmamida_maxStreak", 0);
        ArrowGameStreak.text = "Почтальон: " + PlayerPrefs.GetInt(username + "ArrowGame_maxStreak", 0);
        SvetoforStreak.text = "Светофор: " + PlayerPrefs.GetInt(username + "Svetofor_maxStreak", 0);


        metronomAcc.text = "Метроном: " + PlayerPrefs.GetInt(username + "Metronom_PersentHits", 0) + "%";
        yourRhythmAcc.text = "Твой ритм: " + PlayerPrefs.GetInt(username + "YourRhythm_PersentHits", 0) + "%";
        frogGameAcc.text = "Ритмогушка: " + PlayerPrefs.GetInt(username + "FrogGame_PersentHits", 0) + "%";
        ritmamidaAcc.text = "Ритмамида: " + PlayerPrefs.GetInt(username + "Ritmamida_PersentHits", 0) + "%";
        ArrowGameAcc.text = "Почтальон: " + PlayerPrefs.GetInt(username + "ArrowGame_PersentHits", 0) + "%";
        SvetoforAcc.text = "Светофор: " + PlayerPrefs.GetInt(username + "Svetofor_PersentHits", 0) + "%";
    }
}
