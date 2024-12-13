using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandLeg : MonoBehaviour, ISpacePressHandler
{
    // Ссылки на объекты
    public GameObject LeftBlueFoot, LeftYellowFoot, LeftRedFoot, LeftGreenFoot;
    public GameObject LeftBlueHand, LeftYellowHand, LeftRedHand, LeftGreenHand;
    public GameObject RightBlueHand, RightYellowHand, RightRedHand, RightGreenHand;
    public GameObject RightBlueFoot, RightYellowFoot, RightRedFoot, RightGreenFoot;
    private float lastMetronomeTime = 0f;
    private float flag1 = 0;
    // Ссылка на MenuManager
    public MenuManager menuManager;

    private float lastKeyPressTime;
    private bool isFirstPress = true; // Флаг для отслеживания первого нажатия

    // Время, на которое зеленый, желтый или красный спрайт будет активен
    private float feedbackTime = 0.3f;
    private Coroutine resetCoroutine;
    int flag = 0;
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
        if (menuManager.isPaused)
        {
            flag1 = 0;
            return;
        }

        // Запуск трекинга метронома после первого нажатия
        if (!isFirstPress)
        {
            TrackMetronome();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnSpacePressed(0);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnSpacePressed(1);
        }
    }

    private void TrackMetronome()
    {
        float interval = menuManager.interval; // Интервал метронома
        float currentTime = Time.time;

        // Если текущий момент соответствует следующему звуку метронома
        if (currentTime >= lastMetronomeTime + interval)
        {
            lastMetronomeTime = currentTime; // Обновляем время последнего звука
            Debug.Log("Metronome sound detected! Time: " + lastMetronomeTime);
        }
    }

    public void OnSpacePressed(int flag)
    {

        if (flag == 0) OnSpacePressed1();
        if (flag == 1) OnAPressed();
        if (flag == 2) OnEnterPressed();
        if (flag == 3) OnUPressed();
     }

    /// <summary>
    /// Логика нажатия клавиши Space (левая нога)
    /// </summary>
    void OnSpacePressed1()
    {

        OnFirstPress();
        ProcessKeyPress(LeftBlueFoot, LeftYellowFoot, LeftRedFoot, LeftGreenFoot);
    }
    /// <summary>
    /// Логика нажатия клавиши H (левая рука)
    /// </summary>
    void OnEnterPressed()
    {
        OnFirstPress();

        ProcessKeyPress(LeftBlueHand, LeftYellowHand, LeftRedHand, LeftGreenHand);
    }

    /// <summary>
    /// Логика нажатия клавиши U (правая рука)
    /// </summary>
    void OnUPressed()
    {
        OnFirstPress();
        ProcessKeyPress(RightBlueHand, RightYellowHand, RightRedHand, RightGreenHand);
    }

    /// <summary>
    /// Логика нажатия клавиши Y (правая нога)
    /// </summary>
    void OnAPressed()
    {
        OnFirstPress();
        ProcessKeyPress(RightBlueFoot, RightYellowFoot, RightRedFoot, RightGreenFoot);
    }

    /// <summary>
    /// Обработка первого нажатия клавиши
    /// </summary>
    void OnFirstPress()
    {
        if (isFirstPress)
        {
            isFirstPress = false; // Сбрасываем флаг первого нажатия
            lastMetronomeTime = Time.time; // Устанавливаем время начала метронома
            Debug.Log("First key press detected! Starting metronome tracking.");
        }
    }

    /// <summary>
    /// Общая обработка нажатия клавиши
    /// </summary>
    void ProcessKeyPress(GameObject blue, GameObject yellow, GameObject red, GameObject green)
    {
        float currentTime = Time.time;
        float interval = menuManager.interval; // Интервал из MenuManager
        float metronomeTime = lastMetronomeTime; // Предположим, это переменная с временем удара метронома
        float difference = Mathf.Abs(currentTime - metronomeTime); // Разница между нажатием и ударом метронома

        // Сбрасываем все спрайты перед активацией нужного
        ResetAllSprites();
        if (flag1 > 0) { 
        // Определяем нужный спрайт в зависимости от разницы
        if (difference < 0.35f * interval)
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
        flag1 += 1;
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
