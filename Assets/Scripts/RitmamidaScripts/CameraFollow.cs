using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CameraFollow : MonoBehaviour
{
    private float lastSoundTime = 0f;
    public AudioClip hitSound;
    public AudioClip SuccesHitSound;
    private AudioSource audioSource;
    public GameObject linePrefab;
    public Transform lineContainer;
    public float lineWidthMultiplier = 20f;
    public float lineSpacing = 20f;
    private float lastPressTime;
    private float startTime;
    private bool firstPress = true;
    public MenuManager menuManager;
    private int consecutiveHits = 0;
    private float previousLineWidth = -1f;
    private int matchCounter = 0;
    public TMP_Text scoreText;
    private bool isCounting;
    private bool isPaused = false;  // Игра начинается без паузы
    public GameObject menuPanel;  // Ссылка на панель меню
    public Button openMenuUIButton;  // Кнопка открытия меню
    public Button closeMenuUIButton;  // Кнопка закрытия меню

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.volume = 1.0f;
        isCounting = false;

        // Убедимся, что панель меню выключена при старте
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }

        // Добавляем слушатель на кнопку для открытия меню
        if (openMenuUIButton != null)
        {
            openMenuUIButton.onClick.AddListener(OpenMenu);
        }

        // Добавляем слушатель на кнопку для закрытия меню
        if (closeMenuUIButton != null)
        {
            closeMenuUIButton.onClick.AddListener(CloseMenu);
        }
        // Проверяем наличие scoreText
      
    }
   

    private void Update()
    {
        if (isCounting)
        {
            lastSoundTime = Time.time - startTime;
        }

        // Игнорируем пробел, если игра на паузе
        if (isPaused)
        {
            return;
        }

        // Запуск по пробелу, если игра не на паузе
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnButtonPress();
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

    void OnButtonPress()
    {
        isCounting = true;

        if (firstPress)
        {
            startTime = Time.time;  // Используем Time.time для начала
            lastPressTime = startTime;
            firstPress = false;
            return;
        }

        float currentTime = Time.time;
        float duration = currentTime - lastPressTime;
        lastPressTime = currentTime;
        CreateLine(duration);
    }

    void CreateLine(float duration)
    {
        if (linePrefab == null || lineContainer == null)
        {
            Debug.LogError("linePrefab или lineContainer не назначены в инспекторе.");
            return;
        }

        // Перемещаем все существующие линии вниз
        foreach (Transform child in lineContainer)
        {
            float yOffset = linePrefab.transform.localScale.y + lineSpacing / 10;
            child.position -= new Vector3(0, yOffset, 0);
        }

        // Создаем новую линию
        GameObject newLine = Instantiate(linePrefab, lineContainer);
        Transform lineTransform = newLine.transform;

        // Рассчитываем ширину линии
        float lineWidth = (duration / 8f) * lineWidthMultiplier;
        lineTransform.localScale = new Vector3(lineWidth, lineTransform.localScale.y, lineTransform.localScale.z);
        newLine.transform.localPosition = Vector3.zero;

        // Проверяем совпадение с предыдущей шириной
        if (previousLineWidth > 0 && !isPaused)
        {
            float lowerBound = previousLineWidth * 0.97f;
            float upperBound = previousLineWidth * 1.03f;

            if (lineWidth >= lowerBound && lineWidth <= upperBound)
            {
                consecutiveHits++;
                matchCounter += consecutiveHits;
                scoreText.text = "Счёт: " + matchCounter;
                Debug.Log("Счёт обновлён: " + matchCounter);
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

        // После всех проверок обновляем previousLineWidth текущей шириной линии
        previousLineWidth = lineWidth;
    }


    // Метод открытия меню
    void OpenMenu()
    {
        if (menuManager != null)
        {
            isPaused = true;  // Ставим игру на паузу
            menuManager.OpenMenu();
        }
    }

    // Метод закрытия меню
    void CloseMenu()
    {
        if (menuManager != null)
        {
            isPaused = false;  // Возобновляем игру
            menuManager.CloseMenu();
        }
    }

    // Метод для переключения паузы
    public void TogglePause(bool paused)
    {
        isPaused = paused;

        if (menuPanel != null)
        {
            if (paused)
            {
                menuPanel.SetActive(true);
                CanvasGroup canvasGroup = menuPanel.GetComponent<CanvasGroup>();
                if (canvasGroup != null)
                {
                    canvasGroup.alpha = 1f;
                    canvasGroup.interactable = true;
                    canvasGroup.blocksRaycasts = true;
                }
            }
            else
            {
                menuPanel.SetActive(false);
            }
        }
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
}
