using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class UserData
{
    public string username;
    // Дополнительные поля для пользователя, если нужно
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

    public Image darkenBackground; // Используем Image вместо GameObject

    private string csvPath; // Путь к CSV-файлу

    void Start()
    {
        filePath = Path.Combine(Application.dataPath, "stats.json");
        csvPath = Path.Combine(Application.dataPath, "stats.csv");
        Debug.Log("Path to JSON file: " + filePath);
        Debug.Log("Path to CSV file: " + csvPath);
        string name = PlayerPrefs.GetString("current_user");
        inputField.text = name;
        scoreText.text = "Счёт: " + LoadScore();
        LoadUsers();
    }

    public void RegisterUser()
    {
        username = inputField.text;
        if (username != "")
        {
            PlayerPrefs.SetString("current_user", username);
            PlayerPrefs.Save();
            scoreText.text = "Счёт: " + LoadScore();


            // Добавляем нового пользователя в список
            UserData newUser = new UserData { username = username };
            userList.users.Add(newUser);

            // Сохраняем обновленный список пользователей в JSON
            SaveUsers();


            // Добавляем пользователя в CSV, если его там ещё нет
            AddUserToCSV(username);
        }
        else
        {
            PlayerPrefs.SetString("current_user", "");
            PlayerPrefs.Save();
            Debug.Log("Имя пользователя не введено");
            loginErrorPanel.SetActive(true);
            ShowDarkenBackground();
        }
    }

    private void AddUserToCSV(string username)
    {
        // Если CSV-файл не существует, создаём его с заголовками
        if (!File.Exists(csvPath))
        {
            File.WriteAllText(csvPath, "Username;MetronomMaxStreak;YourRhythmMaxStreak;FrogGameMaxStreak;RitmamidaMaxStreak;ArrowGameMaxStreak;MetronomPercentHits;YourRhythmPercentHits;FrogGamePercentHits;RitmamidaPercentHits;ArrowGamePercentHits;TotalScore;SessionDate\n");
        }

        // Читаем все строки из CSV
        List<string> csvLines = new List<string>(File.ReadAllLines(csvPath));

        // Проверяем, существует ли пользователь в CSV
        bool userExists = csvLines.Exists(line => line.StartsWith(username + ","));
        Debug.Log(0);

        // Формируем новую строку с данными пользователя
        int metronomMaxStreak = PlayerPrefs.GetInt(username + "Metronom_maxStreak", 0);
        int yourRhythmMaxStreak = PlayerPrefs.GetInt(username + "YourRhythm_maxStreak", 0);
        int frogGameMaxStreak = PlayerPrefs.GetInt(username + "FrogGame_maxStreak", 0);
        int ritmamidaMaxStreak = PlayerPrefs.GetInt(username + "Ritmamida_maxStreak", 0);
        int arrowGameMaxStreak = PlayerPrefs.GetInt(username + "ArrowGame_maxStreak", 0);

        int metronomPercentHits = PlayerPrefs.GetInt(username + "Metronom_PersentHits", 0);
        int yourRhythmPercentHits = PlayerPrefs.GetInt(username + "YourRhythm_PersentHits", 0);
        int frogGamePercentHits = PlayerPrefs.GetInt(username + "FrogGame_PersentHits", 0);
        int ritmamidaPercentHits = PlayerPrefs.GetInt(username + "Ritmamida_PersentHits", 0);
        int arrowGamePercentHits = PlayerPrefs.GetInt(username + "ArrowGame_PersentHits", 0);
        float totalScore = metronomMaxStreak + yourRhythmMaxStreak + frogGameMaxStreak + ritmamidaMaxStreak + arrowGameMaxStreak;


        // Получаем текущую дату
        string sessionDate = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string userStats = $"{username};{metronomMaxStreak};{yourRhythmMaxStreak};{frogGameMaxStreak};{ritmamidaMaxStreak};{arrowGameMaxStreak};{metronomPercentHits};{yourRhythmPercentHits};{frogGamePercentHits};{ritmamidaPercentHits};{arrowGamePercentHits};{totalScore};{sessionDate}";
        csvLines.Add(userStats); // Добавляем строку в список

        // Записываем обновленные данные обратно в CSV
        File.WriteAllLines(csvPath, csvLines.ToArray());
        Debug.Log($"Добавлен пользователь {username} в CSV.");


    }

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

    private void LoadUsers()
    {
        if (File.Exists(filePath))
        {
            // Загружаем список пользователей из JSON
            string json = File.ReadAllText(filePath);
            userList = JsonUtility.FromJson<UserList>(json);
            Debug.Log("Пользователи загружены.");
        }
        else
        {
            // Если файла нет, создаём новый список пользователей
            userList = new UserList();
            Debug.Log("Файл пользователей не найден, создаём новый.");
        }
    }

    private void SaveUsers()
    {
        // Сохраняем список пользователей в JSON
        string json = JsonUtility.ToJson(userList);
        File.WriteAllText(filePath, json);
        Debug.Log("Пользователи сохранены.");
    }

    public List<UserData> GetAllUsers()
    {
        return userList.users;
    }
}
