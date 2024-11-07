using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RhythmController : MonoBehaviour
{
    private float rhythmInterval = 1.0f; // Интервал ритма в секундах (значение по умолчанию)
    private float nextBeatTime; // Время следующего удара ритма
    private FrogJump frogJump; // Ссылка на компонент FrogJump
    private bool isGameStarted = false; // Флаг для проверки, началась ли игра
    private Slider speedSlider; // Ссылка на слайдер скорости
    private float lastRhythmInterval; // Последний интервал ритма, для отслеживания изменений
    private MenuManager menuManager; // Ссылка на MenuManager для управления звуком

    // Точность попадания в ритм в процентах (10%)
    private const float allowedAccuracy = 0.1f;

    // Ссылки на кувшинки
    public GameObject leftLilyPad;
    public GameObject rightLilyPad;

    private Vector3 originalScaleLeft;
    private Vector3 originalScaleRight;
    public float scaleFactor = 1.2f; // Во сколько раз увеличиваем спрайт

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

        // Сохраняем оригинальный размер кувшинок
        if (leftLilyPad != null) originalScaleLeft = leftLilyPad.transform.localScale;
        if (rightLilyPad != null) originalScaleRight = rightLilyPad.transform.localScale;
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
            frogJump.Jump(); // Лягушка прыгает на ритм с длительностью прыжка, равной интервалу ритма
        }
    }

    private void StartGame()
    {
        isGameStarted = true; // Устанавливаем флаг начала игры
        nextBeatTime = Time.time + rhythmInterval; // Запускаем метроном сразу, но звук начнет позже
        frogJump.Jump(); // Лягушка сразу начинает прыгать
    }

    // Проверка точности нажатия пробела
    private void CheckAccuracy()
    {
        float timeDifference = Mathf.Abs(Time.time - nextBeatTime); // Разница между текущим временем и ритмом
        float allowedWindow = rhythmInterval * allowedAccuracy; // Вычисляем допустимое отклонение (10%)

        if (timeDifference <= allowedWindow) // Если нажали точно в ритм
        {
            menuManager.UpdateScore();  // Обновляем счёт через MenuManager
            ScaleLilyPads(); // Увеличиваем кувшинки
        }
        else
        {
            menuManager.ResetStreak();
        }
    }

    // Метод для увеличения кувшинок
    private void ScaleLilyPads()
    {
        // Увеличиваем спрайт левой кувшинки
        if (leftLilyPad != null)
        {
            StartCoroutine(ScaleLilyPad(leftLilyPad, scaleFactor));
        }

        // Увеличиваем спрайт правой кувшинки
        if (rightLilyPad != null)
        {
            StartCoroutine(ScaleLilyPad(rightLilyPad, scaleFactor));
        }
    }

    // Метод для плавного изменения масштаба кувшинки
    private IEnumerator ScaleLilyPad(GameObject lilyPad, float scaleFactor)
    {
        Vector3 targetScale = lilyPad.transform.localScale * scaleFactor;
        Vector3 originalScale = lilyPad.transform.localScale;

        float timeElapsed = 0f;
        float scaleDuration = 0.2f; // Длительность увеличения

        while (timeElapsed < scaleDuration)
        {
            lilyPad.transform.localScale = Vector3.Lerp(originalScale, targetScale, timeElapsed / scaleDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        lilyPad.transform.localScale = targetScale; // Убедимся, что конечный размер установлен

        // Возвращаем кувшинку обратно в исходный размер
        timeElapsed = 0f;
        while (timeElapsed < scaleDuration)
        {
            lilyPad.transform.localScale = Vector3.Lerp(targetScale, originalScale, timeElapsed / scaleDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        lilyPad.transform.localScale = originalScale; // Устанавливаем исходный размер
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
}
