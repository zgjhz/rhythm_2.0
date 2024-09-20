using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Ссылка на текстовое поле для вывода очков
    private RhythmController rhythmController;

    void Start()
    {
        // Находим RhythmController на сцене
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
        // Обновляем текстовое поле, если оба компонента заданы
        if (rhythmController != null && scoreText != null)
        {
            scoreText.text = "Очки: " + rhythmController.GetScore().ToString();
        }
    }
}
