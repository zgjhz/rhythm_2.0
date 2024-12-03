using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandLeg : MonoBehaviour
{
    // Ссылки на объекты
    public GameObject LeftBlueFoot, LeftYellowFoot, LeftRedFoot, LeftGreenFoot;
    public GameObject LeftBlueHand, LeftYellowHand, LeftRedHand, LeftGreenHand;
    public GameObject RightBlueHand, RightYellowHand, RightRedHand, RightGreenHand;
    public GameObject RightBlueFoot, RightYellowFoot, RightRedFoot, RightGreenFoot;

    // Ссылка на MenuManager
    public MenuManager menuManager;

    private float lastKeyPressTime;

    // Время, на которое зеленый, желтый или красный спрайт будет активен
    private float feedbackTime = 0.3f;
    private Coroutine resetCoroutine;

    void Start()
    {
        lastKeyPressTime = Time.time;

        // Выключаем все спрайты, кроме синих
        ResetAllSprites();
        ActivateAllBlue();
    }

    void Update()
    {
        // Если игра на паузе, ничего не делаем
        if (menuManager.isPaused) return;

        // Проверяем нажатие клавиш и вызываем соответствующие функции
        if (Input.GetKeyDown(KeyCode.Space)) OnSpacePressed();
        if (Input.GetKeyDown(KeyCode.H)) OnHPressed();
        if (Input.GetKeyDown(KeyCode.U)) OnUPressed();
        if (Input.GetKeyDown(KeyCode.Y)) OnYPressed();
    }

    /// <summary>
    /// Логика нажатия клавиши Space (левая нога)
    /// </summary>
    void OnSpacePressed()
    {
        ProcessKeyPress(LeftBlueFoot, LeftYellowFoot, LeftRedFoot, LeftGreenFoot);
    }

    /// <summary>
    /// Логика нажатия клавиши H (левая рука)
    /// </summary>
    void OnHPressed()
    {
        ProcessKeyPress(LeftBlueHand, LeftYellowHand, LeftRedHand, LeftGreenHand);
    }

    /// <summary>
    /// Логика нажатия клавиши U (правая рука)
    /// </summary>
    void OnUPressed()
    {
        ProcessKeyPress(RightBlueHand, RightYellowHand, RightRedHand, RightGreenHand);
    }

    /// <summary>
    /// Логика нажатия клавиши Y (правая нога)
    /// </summary>
    void OnYPressed()
    {
        ProcessKeyPress(RightBlueFoot, RightYellowFoot, RightRedFoot, RightGreenFoot);
    }

    /// <summary>
    /// Общая обработка нажатия клавиши
    /// </summary>
    void ProcessKeyPress(GameObject blue, GameObject yellow, GameObject red, GameObject green)
    {
        float currentTime = Time.time;
        float interval = menuManager.interval; // Интервал из MenuManager
        float difference = (currentTime - lastKeyPressTime) % interval;

        // Сбрасываем все спрайты перед активацией нужного
        ResetAllSprites();

        // Определяем нужный спрайт в зависимости от разницы
        if (difference < 0.3f * interval)
        {
            ActivateSprite(green); // Зелёный
            menuManager.UpdateScore();
        }
        else if (difference < 0.7f * interval)
        {
            ActivateSprite(yellow); // Жёлтый
        }
        else
        {
            ActivateSprite(red); // Красный
        }

        lastKeyPressTime = currentTime;
    }

    /// <summary>
    /// Активация определённого спрайта (зелёного, жёлтого или красного)
    /// </summary>
    void ActivateSprite(GameObject sprite)
    {
        sprite.SetActive(true);

        // Сбрасываем цвет после заданного времени
        if (resetCoroutine != null) StopCoroutine(resetCoroutine);
        resetCoroutine = StartCoroutine(ResetAfterDelay(sprite));
    }

    /// <summary>
    /// Сбрасывание цветного спрайта после задержки
    /// </summary>
    IEnumerator ResetAfterDelay(GameObject sprite)
    {
        yield return new WaitForSeconds(feedbackTime);
        sprite.SetActive(false); // Выключаем цветной спрайт
    }

    /// <summary>
    /// Выключить все спрайты, кроме синих
    /// </summary>
    void ResetAllSprites()
    {
        // Левые конечности
        LeftYellowFoot.SetActive(false);
        LeftRedFoot.SetActive(false);
        LeftGreenFoot.SetActive(false);

        LeftYellowHand.SetActive(false);
        LeftRedHand.SetActive(false);
        LeftGreenHand.SetActive(false);

        // Правые конечности
        RightYellowHand.SetActive(false);
        RightRedHand.SetActive(false);
        RightGreenHand.SetActive(false);

        RightYellowFoot.SetActive(false);
        RightRedFoot.SetActive(false);
        RightGreenFoot.SetActive(false);
    }

    /// <summary>
    /// Активировать все синие спрайты
    /// </summary>
    void ActivateAllBlue()
    {
        LeftBlueFoot.SetActive(true);
        LeftBlueHand.SetActive(true);
        RightBlueHand.SetActive(true);
        RightBlueFoot.SetActive(true);
    }
}
