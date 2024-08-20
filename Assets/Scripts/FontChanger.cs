using UnityEngine;
using TMPro;  // Добавляем пространство имён для TextMeshPro

public class FontChanger : MonoBehaviour
{
    public TMP_FontAsset newFont;  // Поле для нового шрифта Text Mesh Pro

    void Start()
    {
        // Находим все объекты с компонентом TMP_Text (работает как для UI, так и для обычных текстов)
        TMP_Text[] texts = FindObjectsOfType<TMP_Text>();

        // Проходимся по всем найденным объектам и меняем шрифт
        foreach (TMP_Text text in texts)
        {
            text.font = newFont;  // Присваиваем новый шрифт
        }

        Debug.Log("Font changed for all TextMeshPro components.");
    }
}