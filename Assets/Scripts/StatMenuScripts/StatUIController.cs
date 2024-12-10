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
    public Dropdown userDropdown; // Ссылка на ваш Dropdown
    private string csvFilePath = Path.GetFullPath("stats.csv");    // Путь к CSV-файлу

    void Start()
    {
        LoadUsersFromCSV();
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
            // Разделяем строку по запятой
            string[] values = line.Split(',');

            // Проверяем, что колонка имени существует
            if (values.Length > 0)
            {
                uniqueUserNames.Add(values[0].Trim());
            }
        }

        // Очищаем элементы Dropdown и добавляем уникальные имена
        userDropdown.ClearOptions();
        userDropdown.AddOptions(new List<string>(uniqueUserNames));
    }
}
