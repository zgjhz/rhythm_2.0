using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RhythmController : MonoBehaviour, ISpacePressHandler
{
    private float rhythmInterval = 1.0f;
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
    public bool isToLeft;
    private bool isLeftLilyPadScaling = false; // Флаг для масштабирования левой кувшинки
    private bool isRightLilyPadScaling = false;

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
    }


    void Update()
    {
        if (menuManager.isPaused)
        {
            isGameStarted = false; // Игра приостановлена
            isWaitingForFirstInput = true; // После паузы ждем первого пробела
            frogJump.ResetToStart();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnSpacePressed();
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
        Debug.Log("HUYYYYY  " + nextBeatTime);
        frogJump.Jump(); // Начинаем прыжок
        isToLeft = false; // Сбрасываем направление прыжка
    }


    private void CheckAccuracy()
    {
        float timeDifference = Mathf.Abs(Time.time - nextBeatTime);
        float allowedWindow = rhythmInterval * allowedAccuracy;
        float moderateMissWindow = rhythmInterval * moderateMissAccuracy;

        if (timeDifference <= allowedWindow) // Попадание в ритм
        {
            menuManager.UpdateScore();
            ChangeLilyPadColor(defaultSprite); // Вернуть стандартный цвет
            ScaleLilyPads(); // Анимация кувшинок
        }
        else if (timeDifference <= moderateMissWindow) // Небольшой промах
        {
            ChangeLilyPadColor(yellowSprite);
            StartCoroutine(ResetLilyPadColorAfterDelay(0.5f)); // Возвращение цвета через 0.5 секунды
        }
        else // Большой промах
        {
            ChangeLilyPadColor(redSprite);
            StartCoroutine(ResetLilyPadColorAfterDelay(0.5f));
        }
    }

    private void ChangeLilyPadColor(Sprite newSprite)
    {
        if (leftRenderer != null) leftRenderer.sprite = newSprite;
        if (rightRenderer != null) rightRenderer.sprite = newSprite;
    }

    private IEnumerator ResetLilyPadColorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ChangeLilyPadColor(defaultSprite);
    }
    
    private void ScaleLilyPads()
    {
        if (leftLilyPad != null && !isLeftLilyPadScaling)
        {
            StartCoroutine(ScaleLilyPad(leftLilyPad, 1.2f, isLeft: true));
        }
        if (rightLilyPad != null && !isRightLilyPadScaling)
        {
            StartCoroutine(ScaleLilyPad(rightLilyPad, 1.2f, isLeft: false));
        }
    }

    private IEnumerator ScaleLilyPad(GameObject lilyPad, float scaleFactor, bool isLeft)
    {
        if (isLeft)
            isLeftLilyPadScaling = true;
        else
            isRightLilyPadScaling = true;

        Vector3 targetScale = lilyPad.transform.localScale * scaleFactor;
        Vector3 originalScale = lilyPad.transform.localScale;

        float timeElapsed = 0f;
        float scaleDuration = 0.2f;

        while (timeElapsed < scaleDuration)
        {
            lilyPad.transform.localScale = Vector3.Lerp(originalScale, targetScale, timeElapsed / scaleDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        lilyPad.transform.localScale = targetScale;

        timeElapsed = 0f;
        while (timeElapsed < scaleDuration)
        {
            lilyPad.transform.localScale = Vector3.Lerp(targetScale, originalScale, timeElapsed / scaleDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        lilyPad.transform.localScale = originalScale;

        if (isLeft)
            isLeftLilyPadScaling = false;
        else
            isRightLilyPadScaling = false;
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


            }
        }
    }

    public void OnSpacePressed()
    {
        
        if (isWaitingForFirstInput) // Если ждем первого пробела
        {
            StartGameAfterPause();
        }
        else
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
    }

    private void StartGame()
    {
        isGameStarted = true;
        nextBeatTime = Time.time + rhythmInterval;
        frogJump.Jump();
        isToLeft = false;
    }
}