using UnityEngine;

public class WallHighlightController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public float highlightDuration = 0.07f;  // Длительность подсветки
    private Color targetColor = Color.clear; // Цвет подсветки
    private Color currentColor = Color.clear;
    private bool isHighlighting = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetColor(Color.clear);  // Начальное значение цвета
    }

    void Update()
    {
        if (isHighlighting)
        {
            // Плавное увеличение яркости
            currentColor = Color.Lerp(currentColor, targetColor, Time.deltaTime / highlightDuration);
            SetColor(currentColor);

            if (currentColor == targetColor)
            {
                isHighlighting = false;
            }
        }
        else
        {
            // Плавное уменьшение яркости
            currentColor = Color.Lerp(currentColor, Color.clear, Time.deltaTime / highlightDuration);
            SetColor(currentColor);
        }
    }

    public void TriggerHighlight(Color highlightColor)
    {
        targetColor = highlightColor;
        isHighlighting = true;
    }

    private void SetColor(Color color)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = color;
        }
    }
}
