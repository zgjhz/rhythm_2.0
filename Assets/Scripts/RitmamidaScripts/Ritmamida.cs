using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ritmamida : MonoBehaviour
{
    public CameraFollow cameraFollow;
    public GameObject linePrefab; // Префаб для линии
    public Transform lineContainer; // Контейнер для линий
    public float lineWidthMultiplier = 100f; // Масштаб линии по горизонтали
    public float lineSpacing = 1f; // Вертикальное расстояние между линиями
    private Color lineColor = Color.green; // Цвет линий
    private float lastPressTime;
    private float startTime;
    private bool firstPress = true;
    public GameObject target;

    private void Start()
    {
        // Назначаем целевой объект для CameraFollow
        if (cameraFollow != null)
        {
            cameraFollow.target = target;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Проверка нажатия пробела
        {
            OnButtonPress(lineColor);
        }
    }

    void OnButtonPress(Color lineColor)
    {
        if (firstPress) // Если это первое нажатие, сохраняем время
        {
            startTime = Time.time;
            lastPressTime = startTime;
            firstPress = false;
            return;
        }

        float currentTime = Time.time; // Текущее время
        float duration = currentTime - lastPressTime; // Разница между текущим временем и временем последнего нажатия
        lastPressTime = currentTime;

        CreateLine(duration, lineColor); // Создаем линию с учетом времени
    }

    void CreateLine(float duration, Color lineColor)
    {
        // Проверка на null, чтобы избежать ошибок
        if (linePrefab == null || lineContainer == null)
        {
            Debug.LogError("linePrefab или lineContainer не назначены в инспекторе.");
            return;
        }

        GameObject newLine = Instantiate(linePrefab, lineContainer); // Создаем новую линию

        Transform lineTransform = newLine.transform;

        // Изменение длины линии в зависимости от длительности паузы
        float lineWidth = duration * lineWidthMultiplier;
        lineTransform.localScale = new Vector3(lineWidth, lineTransform.localScale.y, lineTransform.localScale.z);

        // Позиционирование линии выше предыдущей
        float yOffset = GetTotalLineHeight() + lineSpacing;
        newLine.transform.position += new Vector3(0, yOffset / 2.5f);
        target.transform.position = newLine.transform.position;
        target.transform.position += new Vector3(0, 0,-10);
        // Устанавливаем цвет линии
        Renderer lineRenderer = newLine.GetComponent<Renderer>();
        if (lineRenderer != null)
        {
            lineRenderer.material.color = lineColor;
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
        // Удаляем все существующие линии
        foreach (Transform child in lineContainer)
        {
            Destroy(child.gameObject);
        }
        firstPress = true; // Сбрасываем флаг первого нажатия
    }
}
