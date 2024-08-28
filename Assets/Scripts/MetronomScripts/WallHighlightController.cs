using UnityEngine;

public class WallHighlightController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public float highlightDuration = 0.2f;  // Длительность подсветки
    private float currentBrightness = 0f;
    private bool isHighlighting = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetBrightness(0f);  // Начальное значение яркости
    }

    void Update()
    {
        // Обработка увеличения яркости
        if (isHighlighting)
        {
            float increment = (1f / highlightDuration) * Time.deltaTime;
            currentBrightness += increment;

            if (currentBrightness >= 1f)
            {
                currentBrightness = 1f;
                isHighlighting = false;
            }

            SetBrightness(currentBrightness);
        }
        // Обработка уменьшения яркости
        else if (currentBrightness > 0f)
        {
            float decrement = (1f / highlightDuration) * Time.deltaTime;
            currentBrightness -= decrement;

            if (currentBrightness <= 0f)
            {
                currentBrightness = 0f;
            }

            SetBrightness(currentBrightness);
        }
    }

    public void TriggerHighlight()
    {
        isHighlighting = true;
    }

    private void SetBrightness(float brightness)
    {
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = brightness;
            spriteRenderer.color = color;
        }
    }
}
