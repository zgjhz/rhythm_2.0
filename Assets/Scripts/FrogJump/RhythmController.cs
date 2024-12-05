using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RhythmController : MonoBehaviour, ISpacePressHandler
{
    private float rhythmInterval;
    private float nextBeatTime;
    private FrogJump frogJump;
    private bool isGameStarted = false;
    private Slider speedSlider;
    private float lastRhythmInterval;
    private MenuManager menuManager;

    private const float allowedAccuracy = 0.3f; // 30% от интервала ритма
    private const float moderateMissAccuracy = 0.7f; // 70% от интервала ритма
    public bool isWaitingForFirstInput = false; // Флаг ожидания первого пробела после паузы

    public GameObject leftLilyPad;
    public GameObject rightLilyPad;

    public Sprite defaultSprite;
    public Sprite greenSprite;
    public Sprite yellowSprite;
    public Sprite redSprite;

    private SpriteRenderer leftRenderer;
    private SpriteRenderer rightRenderer;

    private Coroutine leftResetCoroutine = null;
    private Coroutine rightResetCoroutine = null;
    private bool isToLeft;

    void Start()
    {
        frogJump = FindObjectOfType<FrogJump>();
        menuManager = FindObjectOfType<MenuManager>();

        if (menuManager != null)
        {
            speedSlider = menuManager.speedSlider;
            rhythmInterval = speedSlider.value;
            lastRhythmInterval = rhythmInterval;
        }

        if (leftLilyPad != null) leftRenderer = leftLilyPad.GetComponent<SpriteRenderer>();
        if (rightLilyPad != null) rightRenderer = rightLilyPad.GetComponent<SpriteRenderer>();
        isToLeft = false;
    }

    void Update()
    {
        if (menuManager.isPaused)
        {
            isGameStarted = false; // Игра приостановлена
            isWaitingForFirstInput = true; // После паузы ждем первого пробела
            frogJump.ResetToStart();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isWaitingForFirstInput) // Если ждем первого пробела
            {
                StartGameAfterPause();
            }
            else
            {
                OnSpacePressed(); // Обычная проверка попадания в ритм
            }
        }

        UpdateRhythmInterval();

        if (Time.timeScale == 0 || !isGameStarted) return;

        if (isGameStarted && Time.time >= nextBeatTime)
        {
            nextBeatTime += rhythmInterval;
            frogJump.Jump();
            isToLeft = !isToLeft;
        }
    }

    private void StartGameAfterPause()
    {
        isWaitingForFirstInput = false; // Сбрасываем флаг ожидания
        isGameStarted = true; // Игра запущена
        nextBeatTime = Time.time + rhythmInterval; // Синхронизация ритма с текущим временем
        frogJump.Jump(); // Начинаем прыжок
        isToLeft = false; // Сбрасываем направление прыжка
    }

    private void CheckAccuracy()
{
    // Смещаем текущее время в ритм ближайшего такта
    float timeSinceLastBeat = (Time.time - nextBeatTime + rhythmInterval) % rhythmInterval;
    float timeDifference = Mathf.Min(timeSinceLastBeat, rhythmInterval - timeSinceLastBeat);

    float allowedWindow = rhythmInterval * allowedAccuracy;
    float moderateMissWindow = rhythmInterval * moderateMissAccuracy;

    if (timeDifference <= allowedWindow) // Попадание в ритм
    {
        menuManager.UpdateScore();
        ChangeLilyPadColor(greenSprite);
    }
    else if (timeDifference <= moderateMissWindow) // Небольшой промах
    {
        ChangeLilyPadColor(yellowSprite);
    }
    else // Большой промах
    {
        ChangeLilyPadColor(redSprite);
    }
}


    private void ChangeLilyPadColor(Sprite newSprite)
    {
        if (isToLeft)
        {
            if (leftRenderer != null)
            {
                if (leftResetCoroutine != null) StopCoroutine(leftResetCoroutine);
                leftRenderer.sprite = newSprite;
                leftResetCoroutine = StartCoroutine(ResetLilyPadColorAfterDelay(0.3f, leftRenderer));
            }
        }
        else
        {
            if (rightRenderer != null)
            {
                if (rightResetCoroutine != null) StopCoroutine(rightResetCoroutine);
                rightRenderer.sprite = newSprite;
                rightResetCoroutine = StartCoroutine(ResetLilyPadColorAfterDelay(0.3f, rightRenderer));
            }
        }
    }

    private IEnumerator ResetLilyPadColorAfterDelay(float delay, SpriteRenderer renderer)
    {
        yield return new WaitForSeconds(delay);
        if (isGameStarted && renderer != null)
        {
            renderer.sprite = defaultSprite;
        }
    }

    private void UpdateRhythmInterval()
    {
        if (speedSlider != null)
        {
            float newRhythmInterval = speedSlider.value;

            if (Mathf.Abs(newRhythmInterval - lastRhythmInterval) > Mathf.Epsilon)
            {
                rhythmInterval = newRhythmInterval;
                lastRhythmInterval = newRhythmInterval;
                isGameStarted = false;
                frogJump.ResetToStart();
                isToLeft = false;
            }
        }
    }

    public void OnSpacePressed()
    {
        if (!isGameStarted)
        {
            StartGame();
        }
        else
        {
            CheckAccuracy();
        }
    }

    private void StartGame()
    {
        isGameStarted = true;
        nextBeatTime = Time.time + rhythmInterval;
        frogJump.Jump();
        isToLeft = false;
    }
}
