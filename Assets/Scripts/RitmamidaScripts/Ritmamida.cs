using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Ritmamida : MonoBehaviour
{
    private float lastSoundTime = 0f;
    public GameObject linePrefab;
    public GameObject linePrefab_purple;
    public Transform lineContainer;
    public float lineWidthMultiplier = 20f;
    public float lineSpacing = 20f;
    private float lastPressTime;
    private float startTime;
    private bool firstPress = true;  // Для запуска после паузы
    public MenuManager menuManager;
    private float previousLineWidth = -1f;
    private int matchCounter = 0;
    public TMP_Text scoreText;
    private bool isCounting;
    public GameObject menuPanel;  // Ссылка на панель меню

    private void Start()
    {
        isCounting = false;

        // Убедимся, что панель меню выключена при старте
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (isCounting)
        {
            lastSoundTime = Time.time - startTime;
        }

        if (menuManager.isPaused)
        {
            ResetAfterPause();
        }

        // Запуск по пробелу, если игра не на паузе
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnButtonPress();
        }

        
    }

    void OnButtonPress()
    {
        // Перезапуск игры после паузы
        if (firstPress)
        {
            startTime = Time.time;  // Запоминаем стартовое время
            lastPressTime = startTime;
            firstPress = false;
            return;  // После первого пробела не создаем линию
        }

        // Если это не первый пробел после перезапуска
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
            float yOffset = linePrefab.transform.localScale.y+0.35f;// Используем только высоту блока без дополнительного отступа
            float yOffset_1 = linePrefab_purple.transform.localScale.y;
            child.position -= new Vector3(0, yOffset, 0);  // Смещаем только на высоту блока
            //child.position -= new Vector3(0, yOffset_1, 0);
        }

        // Создаем новую линию
        GameObject newLine = Instantiate(linePrefab, lineContainer);
        GameObject newLine_1 = Instantiate(linePrefab_purple, lineContainer);
        Transform lineTransform = newLine.transform;
        Transform lineTransform_1 = newLine_1.transform;

        // Рассчитываем ширину линии
        float lineWidth = (duration / 25f) * lineWidthMultiplier;
        // Проверяем совпадение с предыдущей шириной
        if (previousLineWidth > 0 && !menuManager.isPaused)
        {
            float lowerBound = previousLineWidth * 0.90f;
            float upperBound = previousLineWidth * 1.1f;

            if (lineWidth >= lowerBound && lineWidth <= upperBound)
            {
                menuManager.UpdateScore();
                // Устанавливаем ширину новой линии равной предыдущей при успешном попадании
                lineWidth = previousLineWidth;
            }
        }
        else
        {
            menuManager.ResetStreak();
        }

        // Устанавливаем ширину линии
        lineTransform.localScale = new Vector3(lineWidth, lineTransform.localScale.y, lineTransform.localScale.z);
        lineTransform_1.localScale = new Vector3(lineWidth+0.05f, lineTransform_1.localScale.y+0.35f, lineTransform_1.localScale.z);
        newLine.transform.localPosition = Vector3.zero;
        newLine_1.transform.localPosition = Vector3.zero;
        // После всех проверок обновляем previousLineWidth текущей шириной линии
        previousLineWidth = lineWidth;
    }

    // Сброс состояния после паузы
    void ResetAfterPause()
    {
        firstPress = true;  // Устанавливаем, что следующая линия будет создаваться после второго удара
        isCounting = false; // Останавливаем счетчик

        // Сбрасываем временные значения, чтобы исключить создание линии на первый пробел после паузы
        lastPressTime = 0f;
        startTime = 0f;
        previousLineWidth = -1f;  // Обнуляем предыдущую ширину линии, чтобы сбросить последовательность
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
    }
}
