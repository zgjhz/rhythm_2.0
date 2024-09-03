using UnityEngine;
using TMPro; // Добавьте это пространство имен

public class ScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Используем TextMeshProUGUI вместо Text
    private RhythmController rhythmController;

    void Start()
    {
        rhythmController = FindObjectOfType<RhythmController>();

        if (rhythmController == null)
        {
            Debug.LogError("RhythmController не найден! Убедитесь, что объект с этим компонентом существует на сцене.");
        }

        if (scoreText == null)
        {
            Debug.LogError("Score Text не установлен в инспекторе!");
        }
    }

    void Update()
    {
        if (rhythmController != null && scoreText != null)
        {
            scoreText.text = "Очки: " + rhythmController.GetScore().ToString();
        }
    }
}
