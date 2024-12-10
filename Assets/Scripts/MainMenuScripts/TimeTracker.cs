using UnityEngine;
using System;

public class TimeTracker : MonoBehaviour
{
    public static TimeTracker Instance;

    private float sessionStartTime;
    private float totalGameTime; // ����� ����� ����

    private const string TotalTimeKey = "total_time";

    private void Awake()
    {
        // ��������� ������������ ��������� �������
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ��������� ������ ����� �������
        }
        else
        {
            Destroy(gameObject); // ������� ����������� ������
        }
    }

    private void Start()
    {
        // ��������� ����� ����� �� PlayerPrefs
        totalGameTime = PlayerPrefs.GetFloat(TotalTimeKey, 0);

        // ��������� ������ ������� ������� ������
        sessionStartTime = Time.realtimeSinceStartup;
    }

    private void OnApplicationQuit()
    {
        SaveSessionTime();
    }

    public void SaveSessionTime()
    {
        // ��������� ����� ������� ������
        float sessionTime = Time.realtimeSinceStartup - sessionStartTime;

        // ��������� � ������ �������
        totalGameTime += sessionTime;
        sessionStartTime = Time.realtimeSinceStartup; // ��������� ����� ������

        // ��������� ����� �����
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
