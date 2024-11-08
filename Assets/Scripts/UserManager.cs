using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class UserData
{
    public string username;
    // �������� ������ ��������� ������������, ���� �����
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
        scoreText.text = "����: " + LoadScore();
        LoadUsers();
    }

    public void RegisterUser()
    {
        username = inputField.text;
        if (username != "")
        {
            PlayerPrefs.SetString("current_user", username);
            PlayerPrefs.Save();
            scoreText.text = "����: " + LoadScore();
            // ���������, ���������� �� ������������ � ����� ������
            if (userList.users.Exists(user => user.username == username))
            {
                return;
            }

            // ������ ������ ������������ � ��������� ��� � ������
            UserData newUser = new UserData { username = username };
            userList.users.Add(newUser);

            // ��������� ���������� ������ ������������� � ����
            SaveUsers();
        }
        else {
            Debug.Log("������ �� ��� ���");
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
            // ��������� ������������ ������������� �� ����� JSON
            string json = File.ReadAllText(filePath);
            userList = JsonUtility.FromJson<UserList>(json);
            Debug.Log("������������ ���������");
        }
        else
        {
            // ���� ���� �� ����������, ������ ������ ������ �������������
            userList = new UserList();
            Debug.Log("������������� �� �������, ������ ����� ������");
        }
    }

    private void SaveUsers()
    {
        // ����������� ���������� ������ ������������� � JSON � ���������
        string json = JsonUtility.ToJson(userList);
        File.WriteAllText(filePath, json);
        Debug.Log("������������ ���������");
    }

    public List<UserData> GetAllUsers()
    {
        return userList.users;
    }
}
