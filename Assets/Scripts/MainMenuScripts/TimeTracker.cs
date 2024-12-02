using UnityEngine;
using System;

public class TimeTracker : MonoBehaviour
{
    public static TimeTracker Instance;

    private float sessionStartTime;
    private float totalGameTime; // Общее время игры

    private const string TotalTimeKey = "total_time";

    private void Awake()
    {
        // Сохраняем единственный экземпляр объекта
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Сохраняем объект между сценами
        }
        else
        {
            Destroy(gameObject); // Удаляем дублирующий объект
        }
    }

    private void Start()
    {
        // Загружаем общее время из PlayerPrefs
        totalGameTime = PlayerPrefs.GetFloat(TotalTimeKey, 0);

        // Фиксируем начало текущей игровой сессии
        sessionStartTime = Time.realtimeSinceStartup;
    }

    private void OnApplicationQuit()
    {
        SaveSessionTime();
    }

    public void SaveSessionTime()
    {
        // Вычисляем время текущей сессии
        float sessionTime = Time.realtimeSinceStartup - sessionStartTime;

        // Добавляем к общему времени
        totalGameTime += sessionTime;
        sessionStartTime = Time.realtimeSinceStartup; // Обновляем время начала

        // Сохраняем общее время
        PlayerPrefs.SetFloat(TotalTimeKey, totalGameTime);
        PlayerPrefs.Save();
    }

    public float GetTotalGameTime()
    {
        return totalGameTime;
    }

    public float GetCurrentSessionTime()
    {
        return Time.realtimeSinceStartup - sessionStartTime;
    }
}
