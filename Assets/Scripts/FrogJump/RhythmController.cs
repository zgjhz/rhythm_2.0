using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RhythmController : MonoBehaviour
{
    public TMP_Text scoreText; // Ссылка на текстовое поле для отображения счёта
    private float rhythmInterval = 1.0f; // Интервал ритма в секундах (значение по умолчанию)
    private float nextBeatTime; // Время следующего удара ритма
    private int score = 0; // Счёт
    private FrogJump frogJump; // Ссылка на компонент FrogJump
    private bool isGameStarted = false; // Флаг для проверки, началась ли игра
    private Slider speedSlider; // Ссылка на слайдер скорости
    private float lastRhythmInterval; // Последний интервал ритма, для отслеживания изменений
    private MenuManager menuManager; // Ссылка на MenuManager для управления звуком

    // Точность попадания в ритм в процентах (10%)
    private const float allowedAccuracy = 0.1f;

    void Start()
    {
        frogJump = FindObjectOfType<FrogJump>(); // Находим компонент FrogJump в сцене

        // Находим MenuManager и слайдер в сцене
        menuManager = FindObjectOfType<MenuManager>();
        if (menuManager != null)
        {
            speedSlider = menuManager.speedSlider;
            rhythmInterval = speedSlider.value; // Устанавливаем начальное значение интервала ритма
            lastRhythmInterval = rhythmInterval; // Сохраняем значение интервала
        }

        // Инициализируем отображение счёта
        UpdateScoreText();
    }

    void Update()
    {
        // Проверяем нажатие пробела для начала игры или для проверки точности
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isGameStarted)
            {
                StartGame(); // Запуск игры при нажатии пробела
            }
            else
            {
                CheckAccuracy(); // Проверяем точность нажатия пробела
            }
        }

        // Обновляем интервал ритма на основе значения слайдера
        UpdateRhythmInterval();

        // Проверка на паузу
        if (Time.timeScale == 0 || !isGameStarted) return; // Если игра на паузе или не началась, не выполняем действия

        // Автоматический прыжок лягушки на каждый удар ритма
        if (isGameStarted && Time.time >= nextBeatTime) // Изменено, чтобы проверить, началась ли игра
        {
            nextBeatTime += rhythmInterval; // Обновить время следующего удара
            menuManager.PlaySound(); // Воспроизведение звука метронома через MenuManager
            frogJump.Jump(); // Лягушка прыгает на ритм с длительностью прыжка, равной интервалу ритма
        }
    }

    private void StartGame()
    {
        isGameStarted = true; // Устанавливаем флаг начала игры
        nextBeatTime = Time.time + rhythmInterval; // Запускаем метроном сразу, но звук начнет позже
        frogJump.Jump(); // Лягушка сразу начинает прыгать
        menuManager.PlaySound(); // Запуск звука метронома через MenuManager
    }

    // Проверка точности нажатия пробела
    private void CheckAccuracy()
    {
        float timeDifference = Mathf.Abs(Time.time - nextBeatTime); // Разница между текущим временем и ритмом
        float allowedWindow = rhythmInterval * allowedAccuracy; // Вычисляем допустимое отклонение (10%)

        if (timeDifference <= allowedWindow) // Если нажали точно в ритм
        {
            UpdateScore(); // Обновляем счёт
        }
    }

    private void UpdateRhythmInterval()
    {
        if (speedSlider != null)
        {
            float newRhythmInterval = speedSlider.value;

            // Если интервал ритма изменился, сбросить лягушку в начальное состояние и ждать пробела
            if (Mathf.Abs(newRhythmInterval - lastRhythmInterval) > Mathf.Epsilon)
            {
                rhythmInterval = newRhythmInterval; // Обновляем интервал ритма
                lastRhythmInterval = newRhythmInterval; // Сохраняем новое значение интервала
                isGameStarted = false; // Игра останавливается, ждет нового пробела
                frogJump.ResetToStart(); // Возвращаем лягушку в начальное положение

                if (menuManager != null)
                {
                    menuManager.StopMetronomeSound(); // Останавливаем метроном
                }
            }
        }
    }

    private void UpdateScore()
    {
        score++; // Увеличиваем счёт на 1
        UpdateScoreText(); // Обновляем текст счёта
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Очки: " + score.ToString(); // Обновляем текст с текущим счётом
        }
    }

    public int GetScore()
    {
        return score; // Возвращаем текущее значение счёта
    }
}
