using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class UserData
{
    public string username;
    // ?????????????? ???? ??? ????????????, ???? ?????
}

[System.Serializable]
public class UserList
{
    public List<UserData> users = new List<UserData>();
}

public class UserManager : MonoBehaviour
{
    private string filePath;
    private UserList userList;
    private string username;

    public TMP_InputField inputField;
    public TMP_Text scoreText;
    public GameObject loginErrorPanel;

    public Image darkenBackground; // ?????????? Image ?????? GameObject

    private string csvPath; // ???? ? CSV-?????

    void Start()
    {
        filePath = Path.Combine(Application.dataPath, "stats.json");
        csvPath = Path.Combine(Application.dataPath, "stats.csv");
        LoadUsers();
    }

    public void RegisterUser()
    {
        if (username != "")
        {
            PlayerPrefs.SetString("current_user", username);
            PlayerPrefs.Save();
            scoreText.text = "Счёт: " + LoadScore();

            UserData newUser = new UserData { username = username };
            userList.users.Add(newUser);

            SaveUsers();

        }
        else
        {
            loginErrorPanel.SetActive(true);
            ShowDarkenBackground();
        }
    }

    private void AddUserToCSV(string username)
    {
        // ???? CSV-???? ?? ??????????, ??????? ??? ? ???????????
        if (!File.Exists(csvPath))
        {
            File.WriteAllText(csvPath, "Имя;Максимум подряд Метроном;Максимум подряд Твой ритм;Максимум подряд Ритмогушка;Максимум подряд Ритмамида;Максимум подряд Почтальон;Максимум подряд Светофор;Процент попаданий Меторном;Процент попаданий Твой ритм;Процент попаданий Ритмогушка;Процент попаданий Ритмамида;Процент попаданий Почтальон;Процент попаданий Светофор;Общий счет;Дата сессии\n");
        }
        
        List<string> csvLines = new List<string>(File.ReadAllLines(csvPath));

        bool userExists = csvLines.Exists(line => line.StartsWith(username + ","));
        Debug.Log(0);

        int metronomMaxStreak = PlayerPrefs.GetInt(username + "Metronom_maxStreak", 0);
        int yourRhythmMaxStreak = PlayerPrefs.GetInt(username + "YourRhythm_maxStreak", 0);
        int frogGameMaxStreak = PlayerPrefs.GetInt(username + "FrogGame_maxStreak", 0);
        int ritmamidaMaxStreak = PlayerPrefs.GetInt(username + "Ritmamida_maxStreak", 0);
        int arrowGameMaxStreak = PlayerPrefs.GetInt(username + "ArrowGame_maxStreak", 0);
        int svetoforMaxStreak = PlayerPrefs.GetInt(username + "Svetofor_maxStreak", 0);

        int metronomPercentHits = PlayerPrefs.GetInt(username + "Metronom_PersentHits", 0);
        int yourRhythmPercentHits = PlayerPrefs.GetInt(username + "YourRhythm_PersentHits", 0);
        int frogGamePercentHits = PlayerPrefs.GetInt(username + "FrogGame_PersentHits", 0);
        int ritmamidaPercentHits = PlayerPrefs.GetInt(username + "Ritmamida_PersentHits", 0);
        int arrowGamePercentHits = PlayerPrefs.GetInt(username + "ArrowGame_PersentHits", 0);
        int svetoforPercentHits = PlayerPrefs.GetInt(username + "Svetofor_PersentHits", 0);
        float totalScore = LoadScore();

        string sessionDate = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string userStats = $"{username};{metronomMaxStreak};{yourRhythmMaxStreak};{frogGameMaxStreak};{ritmamidaMaxStreak};{arrowGameMaxStreak};{svetoforMaxStreak};{metronomPercentHits};{yourRhythmPercentHits};{frogGamePercentHits};{ritmamidaPercentHits};{arrowGamePercentHits};{svetoforPercentHits};{totalScore};{sessionDate}";
        csvLines.Add(userStats);

        File.WriteAllLines(csvPath, csvLines.ToArray());
    }

    private void ShowDarkenBackground()
    {
        darkenBackground.gameObject.SetActive(true); // ???????? Image
        var color = darkenBackground.color;
        color.a = 0.5f; // ????????????? ?????????????? ??? (?? 0 ?? 1)
        darkenBackground.color = color;
    }

    // ??????? ???????????? ???? (???????????? ??????)
    private void HideDarkenBackground()
    {
        darkenBackground.gameObject.SetActive(false); // ????????? Image
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

    private void LoadUsers()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            userList = JsonUtility.FromJson<UserList>(json);
        }
        else
        {
            userList = new UserList();
        }
    }

    private void SaveUsers()
    {
        string json = JsonUtility.ToJson(userList);
        File.WriteAllText(filePath, json);
    }

    public List<UserData> GetAllUsers()
    {
        return userList.users;
    }
}
