using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderLabel : MonoBehaviour
{
    public Slider slider;
    public TMP_Text label;
    public string sliderTag; // Тег для определения типа слайдера

    void Start()
    {
        if (slider != null && label != null)
        {
            slider.onValueChanged.AddListener(UpdateLabel);
            UpdateLabel(slider.value);
        }
    }

    void UpdateLabel(float value)
    {
        if (sliderTag == "Volume") // Если слайдер для громкости
        {
            // Преобразуем значение в проценты
            label.text = Mathf.RoundToInt(value) + "%";
        }
        else if (sliderTag == "Speed") // Если слайдер для скорости
        {
            // Выводим значение скорости без изменений
            label.text = value.ToString("F2") + "c"; // Можно настроить количество знаков после запятой
        }

        // Обновляем позицию текста, чтобы он следовал за ползунком
        Vector3 labelPos = slider.handleRect.position;
        label.transform.position = new Vector3(labelPos.x, label.transform.position.y, label.transform.position.z);
    }
}
