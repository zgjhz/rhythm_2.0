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

    private float arrowActiveDuration = 0.3f; // Время, через которое стрелка гаснет

    void Start()
    {
        lastMetronomeTime = Time.time;
        lastKeyPressTime = Time.time;

        // Постоянно включаем синие стрелки
        leftArrowBlue.SetActive(true);
        rightArrowBlue.SetActive(true);

        // Отключаем все остальные цветные стрелки
        DeactivateColoredArrows();
    }

    void Update()
    {
        metronomeInterval = menuManager.interval;

        // Левую стрелку управляет пробел
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnSpacePressed(2);
        }

        // Правую стрелку управляет Enter
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnSpacePressed(3); // false - правая стрелка
        }
    }
    void OnSpacePressed(int Flag)
    {
        ProcessKeyPress(Flag); // true - левая стрелка
    }

    void ProcessKeyPress( int isLeftArrow)
    {
        float currentTime = Time.time;
        float keyPressDelta = currentTime - lastKeyPressTime;
        float difference = keyPressDelta - metronomeInterval;

        if (isLeftArrow==2) // Если нажата клавиша пробела, изменяем левую кнопку
        {
            if (difference > 0 && difference > 0.7f * metronomeInterval)
            {
                ActivateArrow(leftArrowRed); // Левая красная
                menuManager.ResetStreak();
            }
            else if (difference < 0 && Mathf.Abs(difference) > 0.7f * metronomeInterval)
            {
                ActivateArrow(leftArrowRed); // Левая красная
                menuManager.ResetStreak();
            }
            else if (difference > 0 && difference >= 0.15f * metronomeInterval && difference <= 0.7f * metronomeInterval)
            {
                ActivateArrow(leftArrowYellow); // Левая желтая
                menuManager.ResetStreak();
            }
            else if (difference < 0 && Mathf.Abs(difference) >= 0.15f * metronomeInterval && Mathf.Abs(difference) <= 0.7f * metronomeInterval)
            {
                ActivateArrow(leftArrowYellow); // Левая желтая
                menuManager.ResetStreak();
            }
            else
            {
                ActivateArrow(leftArrowGreen); // Левая зеленая
                menuManager.UpdateScore();
            }
        }
        else // Если нажата клавиша Enter, изменяем правую кнопку
        {
            if (difference > 0 && difference > 0.7f * metronomeInterval)
            {
                ActivateArrow(rightArrowRed); // Правая красная
                menuManager.ResetStreak();
            }
            else if (difference < 0 && Mathf.Abs(difference) > 0.7f * metronomeInterval)
            {
                ActivateArrow(rightArrowRed); // Правая красная
                menuManager.ResetStreak();
            }
            else if (difference > 0 && difference >= 0.15f * metronomeInterval && difference <= 0.7f * metronomeInterval)
            {
                ActivateArrow(rightArrowYellow); // Правая желтая
                menuManager.ResetStreak();
            }
            else if (difference < 0 && Mathf.Abs(difference) >= 0.15f * metronomeInterval && Mathf.Abs(difference) <= 0.7f * metronomeInterval)
            {
                ActivateArrow(rightArrowYellow); // Правая желтая
                menuManager.ResetStreak();
            }
            else
            {
                ActivateArrow(rightArrowGreen); // Правая зеленая
                menuManager.UpdateScore();
            }
        }

        // Обновляем время последнего нажатия
        lastKeyPressTime = currentTime;
    }

    void ActivateArrow(GameObject arrow)
    {
        // Выключаем все цветные стрелки
        DeactivateColoredArrows();

        // Включаем только нужную
        arrow.SetActive(true);

        // Автоматически выключаем стрелку через заданное время
        StartCoroutine(DeactivateArrowAfterDelay(arrow));
    }

    IEnumerator DeactivateArrowAfterDelay(GameObject arrow)
    {
        yield return new WaitForSeconds(arrowActiveDuration);
        arrow.SetActive(false);
    }

    void DeactivateColoredArrows()
    {
        leftArrowRed.SetActive(false);
        rightArrowRed.SetActive(false);
        leftArrowYellow.SetActive(false);
        rightArrowYellow.SetActive(false);
        leftArrowGreen.SetActive(false);
        rightArrowGreen.SetActive(false);
    }
}
