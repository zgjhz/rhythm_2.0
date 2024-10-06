using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RhythmController : MonoBehaviour
{ 
    public TMP_Text scoreText; // Ссылка на текстовое поле для отображения счёта
    public AudioSource metronomeAudioSource; // Ссылка на AudioSource для звука метронома
    private float rhythmInterval = 1.0f; // Интервал ритма в секундах (значение по умолчанию)
    private float nextBeatTime;
    private int score = 0; // Счёт
    private FrogJump frogJump; // Ссылка на компонент FrogJump
    private bool isGameStarted = false; // Флаг для проверки, началась ли игра
    private Slider speedSlider; // Ссылка на слайдер скорости

    // Точность попадания в ритм в процентах (20%)
    private float allowedAccuracy = 0.2f;

    void Start()
    {
        frogJump = FindObjectOfType<FrogJump>(); // Находим компонент FrogJump в сцене

        // Находим слайдер в сцене
        MenuManager menuManager = FindObjectOfType<MenuManager>();
        if (menuManager != null)
        {
            speedSlider = menuManager.speedSlider;
        }

        nextBeatTime = Time.time + rhythmInterval; // Устанавливаем время следующего удара

        // Инициализируем отображение счёта
        if (scoreText != null)
        {
            scoreText.text = "Очки: " + score.ToString();
        }

        // Запускаем игру сразу
        StartGame();
    }

    void Update()
    {
        // Обновляем интервал ритма на основе значения слайдера
        if (speedSlider != null)
        {
            rhythmInterval = speedSlider.value; // Получаем значение слайдера и устанавливаем его в rhythmInterval
        }

        // Проверка на паузу
        if (Time.timeScale == 0) return;  // Если игра на паузе, не выполняем действия

        // Автоматический прыжок лягушки на каждый удар ритма
        if (Time.time >= nextBeatTime)
        {
            nextBeatTime += rhythmInterval; // Обновить время следующего удара

            if (isGameStarted)
            {
                PlayMetronomeSound(); // Воспроизведение звука метронома
                frogJump.Jump(); // Лягушка прыгает на ритм с длительностью прыжка, равной интервалу ритма
            }
        }

        // Проверка нажатия пробела и начисление очков
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckTimingForScore();
        }
    }

    private void StartGame()
    {
        isGameStarted = true; // Устанавливаем флаг начала игры
        nextBeatTime = Time.time + rhythmInterval; // Запускаем метроном сразу
        PlayMetronomeSound(); // Воспроизводим звук метронома при старте
        frogJump.Jump(); // Лягушка сразу начинает прыгать
    }

    private void PlayMetronomeSound()
    {
        if (metronomeAudioSource != null)
        {
            metronomeAudioSource.Play(); // Воспроизведение звука метронома
        }
    }

    private void CheckTimingForScore()
    {
        // Рассчитываем допустимое отклонение для попадания в ритм
        float tolerance = rhythmInterval * allowedAccuracy;

        // Проверяем, попал ли игрок в допустимый интервал
        if (Mathf.Abs(Time.time - nextBeatTime) <= tolerance)
        {
            // Если нажатие в пределах точности — начисляем очки
            score++;
            UpdateScoreText();
        }
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Очки: " + score.ToString();
        }
    }

    public int GetScore()
    {
        return score;
    }
}

