using UnityEngine;

public class test_slider_HandLeg : MonoBehaviour
{
    public GameObject sliderObject; // Объект слайдера
    private float currentHitPercentage = 0f; // Текущий процент попаданий (от 0 до 100)

    private const float minY = -4.5f; // Минимальное значение по оси Y
    private const float maxY = 4.5f;  // Максимальное значение по оси Y
    private const float constantX = 6f; // Константа для позиции по оси X

    private float totalHits = 0; // Общее количество попыток
    private float successfulHits = 0; // Количество успешных попаданий
    public MenuManager menuManager;

    void Update()
    {
        // Преобразуем процент попаданий в значение для оси Y с учетом минимального и максимального значений
        float normalizedY = Mathf.Lerp(minY, maxY, currentHitPercentage / 100f);

        // Обновляем позицию слайдера
        successfulHits = menuManager.score;
        totalHits = menuManager.spacePressCount;
        currentHitPercentage = (totalHits > 0) ? (float)successfulHits / totalHits * 100f : 0f;
        sliderObject.transform.position = new Vector3(constantX, normalizedY, sliderObject.transform.position.z);
    }

}
