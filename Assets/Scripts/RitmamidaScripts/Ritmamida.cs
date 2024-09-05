using UnityEngine;
using UnityEngine.UI;

public class Ritmamida : MonoBehaviour
{
    public AudioClip hitSound;
    public AudioClip SuccesHitSound;
    private AudioSource audioSource;
    public CameraFollow cameraFollow;
    public GameObject linePrefab;
    public Transform lineContainer;
    public float lineWidthMultiplier = 100f;
    public float lineSpacing = 20f;
    private Color lineColor = Color.green;
    private float lastPressTime;
    private float startTime;
    private bool firstPress = true;
    public GameObject target;

    private int consecutiveHits = 0;
    private float previousLineWidth = -1f;
    private int matchCounter = 0;

    private bool isPaused = false; // Флаг для проверки состояния паузы
    public static event System.Action<bool> OnPauseStateChanged;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.volume = 1.0f;

        if (cameraFollow != null)
        {
            cameraFollow.target = target;
        }
    }

    private void Update()
    {
        // Если игра на паузе, игнорируем нажатие клавиш
        if (isPaused)
        {
            return;
        } 
        // Проверка нажатия пробела
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnButtonPress(lineColor);
        }

        // Проверка нажатия клавиши Escape для паузы
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        Debug.Log("ХУЙХУЙХУЙХУЙХУЙХУЙХУЙХУЙХУЙХУЙХУЙХ");
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f; // Останавливаем время
            audioSource.Pause(); // Останавливаем звук
            consecutiveHits = 0; // Сброс последовательных попаданий
        }
        else
        {
            Time.timeScale = 1f; // Возобновляем время
            audioSource.UnPause(); // Возобновляем звук
        }

        // Оповещаем другие скрипты об изменении состояния паузы
        if (OnPauseStateChanged != null)
        {
            OnPauseStateChanged(isPaused);
        }
    }

    void PlayHitSound()
    {
        audioSource.Stop();
        audioSource.clip = hitSound;
        audioSource.Play();
    }

    void PlaySuccesHitSound()
    {
        audioSource.Stop();
        audioSource.clip = SuccesHitSound;
        audioSource.Play();
    }

    void OnButtonPress(Color lineColor)
    {
        // Проверка, что игра не на паузе
        if (isPaused)
        {
            return;
        }

        if (firstPress)
        {
            startTime = Time.unscaledTime; // Используем Time.unscaledTime для правильного учета времени при паузе
            lastPressTime = startTime;
            firstPress = false;
            return;
        }

        float currentTime = Time.unscaledTime; // Также используем Time.unscaledTime
        float duration = currentTime - lastPressTime;
        lastPressTime = currentTime;

        CreateLine(duration, lineColor);
    }

    void CreateLine(float duration, Color lineColor)
    {
        if (linePrefab == null || lineContainer == null)
        {
            Debug.LogError("linePrefab или lineContainer не назначены в инспекторе.");
            return;
        }

        GameObject newLine = Instantiate(linePrefab, lineContainer);
        Transform lineTransform = newLine.transform;

        // Рассчитываем ширину линии
        float lineWidth = (duration / 8f) * lineWidthMultiplier;
        lineTransform.localScale = new Vector3(lineWidth, lineTransform.localScale.y, lineTransform.localScale.z);

        // Рассчитываем позицию линии
        float yOffset = GetTotalLineHeight() + lineSpacing;
        newLine.transform.position += new Vector3(0, yOffset / 2.5f);

        if (previousLineWidth > 0 && !isPaused)
        {
            float lowerBound = previousLineWidth * 0.97f;
            float upperBound = previousLineWidth * 1.03f;

            if (lineWidth >= lowerBound && lineWidth <= upperBound)
            {
                consecutiveHits++;
                matchCounter += consecutiveHits;
                PlaySuccesHitSound();
            }
            else
            {
                PlayHitSound();
                consecutiveHits = 0;
            }
        }
        else
        {
            PlayHitSound();
        }

        // Обновляем позицию цели
        previousLineWidth = lineWidth;
        target.transform.position = newLine.transform.position;
        target.transform.position += new Vector3(0, 0, -10);

        // Обновляем цвет линии
        Renderer lineRenderer = newLine.GetComponent<Renderer>();
        if (lineRenderer != null)
        {
            lineRenderer.material.color = lineColor;
        }
        else
        {
            Debug.LogError("Компонент Renderer не найден на созданной линии.");
        }
    }

    float GetTotalLineHeight()
    {
        float totalHeight = 0f;
        foreach (Transform child in lineContainer)
        {
            totalHeight += child.localScale.y + lineSpacing;
        }
        return totalHeight;
    }

    public void ResetLines()
    {
        foreach (Transform child in lineContainer)
        {
            Destroy(child.gameObject);
        }
        firstPress = true;
        previousLineWidth = -1f;
        matchCounter = 0;
        consecutiveHits = 0;
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 48;
        style.normal.textColor = Color.white;

        GUI.Label(new Rect(10, 10, 300, 50), "Очки: " + matchCounter, style);
    }
}
