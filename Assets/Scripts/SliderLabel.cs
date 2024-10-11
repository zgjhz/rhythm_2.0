using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderLabel : MonoBehaviour
{
    public Slider slider;          // Ссылка на слайдер
    public TextMeshProUGUI label;  // Ссылка на текст выноски
    public RectTransform handle;   // Ссылка на RectTransform ручки слайдера
    public string sliderTag;
    private float percent;

    void Start()
    {
        // Установить начальное значение текста и его позицию
        UpdateLabel(slider.value);
        
        // Подписаться на событие изменения значения слайдера
        slider.onValueChanged.AddListener(UpdateLabel);
    }

    void UpdateLabel(float value)
    {
        if (sliderTag == "Volume") // Если слайдер для громкости
        {
            percent = value / 3 * 100;// Преобразуем значение в проценты
            label.text = Mathf.RoundToInt(percent) + "%";
        }
        else if (sliderTag == "Speed") // Если слайдер для скорости
        {
            // Выводим значение скорости без изменений
            label.text = value.ToString("F2") + "c"; // Можно настроить количество знаков после запятой
        }

        // Обновляем позицию текста, чтобы он следовал за ползунком
        Vector3 handlePosition = handle.position; // Получаем мировую позицию ручки слайдера
        label.rectTransform.position = new Vector3(handlePosition.x, handlePosition.y - 40, handlePosition.z); // Позиционируем текст чуть выше ползунка
    }
}

