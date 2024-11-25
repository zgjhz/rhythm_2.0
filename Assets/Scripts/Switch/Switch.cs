using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public GameObject leftArrowBlue;
    public GameObject rightArrowBlue;
    public GameObject leftArrowRed;
    public GameObject rightArrowRed;
    public GameObject leftArrowYellow;
    public GameObject rightArrowYellow;
    public GameObject leftArrowGreen;
    public GameObject rightArrowGreen;

    public float metronomeInterval = 1.0f; // Интервал между ударами метронома
    private float lastMetronomeTime;
    private float lastKeyPressTime;
    public MenuManager menuManager;

    void Start()
    {
        lastMetronomeTime = Time.time;
        lastKeyPressTime = Time.time;

        // Отключаем все цветные стрелки
        DeactivateAllArrows();

        // Включаем изначально синие стрелки
        leftArrowBlue.SetActive(true);
        rightArrowBlue.SetActive(true);

        
    }

    void Update()
    {
        float currentTime = Time.time;
        metronomeInterval = menuManager.interval;

        // Левую стрелку управляет пробел
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ProcessKeyPress(currentTime, true); // true - левая стрелка
        }

        // Правую стрелку управляет Enter
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ProcessKeyPress(currentTime, false); // false - правая стрелка
        }
    }

    void ProcessKeyPress(float currentTime, bool isLeftArrow)
    {
        float keyPressDelta = currentTime - lastKeyPressTime;
        float metronomeDelta = menuManager.interval;
        float difference = keyPressDelta - metronomeDelta;

        if (isLeftArrow) // Если нажата клавиша пробела, изменяем левую кнопку
        {
            if (difference > 0 && difference > 0.7f * metronomeInterval)
            {
                ActivateArrow(leftArrowRed); // Левая красная
            }
            else if (difference < 0 && Mathf.Abs(difference) > 0.7f * metronomeInterval)
            {
                ActivateArrow(leftArrowRed); // Левая красная
            }
            else if (difference > 0 && difference >= 0.3f * metronomeInterval && difference <= 0.7f * metronomeInterval)
            {
                ActivateArrow(leftArrowYellow); // Левая желтая
            }
            else if (difference < 0 && Mathf.Abs(difference) >= 0.3f * metronomeInterval && Mathf.Abs(difference) <= 0.7f * metronomeInterval)
            {
                ActivateArrow(leftArrowYellow); // Левая желтая
            }
            else
            {
                ActivateArrow(leftArrowGreen); // Левая зеленая
            }
        }
        else // Если нажата клавиша Enter, изменяем правую кнопку
        {
            if (difference > 0 && difference > 0.7f * metronomeInterval)
            {
                ActivateArrow(rightArrowRed); // Правая красная
            }
            else if (difference < 0 && Mathf.Abs(difference) > 0.7f * metronomeInterval)
            {
                ActivateArrow(rightArrowRed); // Правая красная
            }
            else if (difference > 0 && difference >= 0.3f * metronomeInterval && difference <= 0.7f * metronomeInterval)
            {
                ActivateArrow(rightArrowYellow); // Правая желтая
            }
            else if (difference < 0 && Mathf.Abs(difference) >= 0.3f * metronomeInterval && Mathf.Abs(difference) <= 0.7f * metronomeInterval)
            {
                ActivateArrow(rightArrowYellow); // Правая желтая
            }
            else
            {
                ActivateArrow(rightArrowGreen); // Правая зеленая
            }
        }

        // Обновляем время последнего нажатия
        lastKeyPressTime = currentTime;
    }

    void ActivateArrow(GameObject arrow)
    {
        // Выключаем все стрелки
        DeactivateAllArrows();

        // Включаем только нужную
        arrow.SetActive(true);
    }

    void DeactivateAllArrows()
    {
        leftArrowBlue.SetActive(false);
        rightArrowBlue.SetActive(false);
        leftArrowRed.SetActive(false);
        rightArrowRed.SetActive(false);
        leftArrowYellow.SetActive(false);
        rightArrowYellow.SetActive(false);
        leftArrowGreen.SetActive(false);
        rightArrowGreen.SetActive(false);
    }
}
