using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    // Сохранение счёта в PlayerPrefs
    public void SaveScore(string ScoreKey, int score)
    {
        PlayerPrefs.SetInt(ScoreKey, score);
        PlayerPrefs.Save();
    }

    // Загрузка сохранённого счёта из PlayerPrefs
    public int LoadScore(string ScoreKey)
    {
        if (PlayerPrefs.HasKey(ScoreKey))
        {
            return PlayerPrefs.GetInt(ScoreKey);
        }
        else
        {
            return 0;
        }
    }

    // Очистка сохранённого счёта
    public void ResetScore(string ScoreKey)
    {
        PlayerPrefs.DeleteKey(ScoreKey);
    }
}