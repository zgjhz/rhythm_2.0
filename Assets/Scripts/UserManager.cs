using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class UserData
{
    public string username;
    // Добавьте другие параметры пользователя, если нужно
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

    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "users.json");
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
            // Проверяем, существует ли пользователь с таким именем
            if (userList.users.Exists(user => user.username == username))
            {
                return;
            }

            // Создаём нового пользователя и добавляем его в список
            UserData newUser = new UserData { username = username };
            userList.users.Add(newUser);

            // Сохраняем обновлённый список пользователей в файл
            SaveUsers();
        }
        else {
            Debug.Log("Долбаёб не ввёл имя");
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

    private void LoadUsers()
    {
        if (File.Exists(filePath))
        {
            // Загружаем существующих пользователей из файла JSON
            string json = File.ReadAllText(filePath);
            userList = JsonUtility.FromJson<UserList>(json);
            Debug.Log("Пользователи загружены");
        }
        else
        {
            // Если файл не существует, создаём пустой список пользователей
            userList = new UserList();
            Debug.Log("Пользователей не найдено, создаём новый список");
        }
    }

    private void SaveUsers()
    {
        // Преобразуем обновлённый список пользователей в JSON и сохраняем
        string json = JsonUtility.ToJson(userList);
        File.WriteAllText(filePath, json);
        Debug.Log("Пользователи сохранены");
    }

    public List<UserData> GetAllUsers()
    {
        return userList.users;
    }
}
