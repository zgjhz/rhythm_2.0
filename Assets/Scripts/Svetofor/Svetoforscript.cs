using System.Collections;
using UnityEngine;

public class Svetoforscript : MonoBehaviour
{
    public GameObject greenSprite;
    public GameObject leftRedSprite;
    public GameObject rightRedSprite;
    public GameObject leftYellowSprite;
    public GameObject rightYellowSprite;

    public AudioClip metronomeSound; // Звук метронома
    private AudioSource audioSource;
    public float metronomeInterval = 1f; // Интервал между ударами метронома
    private float lastMetronomeTime;
    private float lastKeyPressTime;

    private bool isGameStarted = false;
    public MenuManager menuManager;

    public float spriteActiveDuration = 0.1f; // Время, через которое спрайт гаснет

    void Start()
    {
        lastMetronomeTime = Time.time;
        lastKeyPressTime = Time.time;
        spriteActiveDuration = menuManager.interval * 0.45f;
        // Делаем все спрайты неактивными в начале
        DeactivateAllSprites();

        // Инициализация AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = metronomeSound;
        audioSource.playOnAwake = false; // Чтобы звук не проигрывался сразу
    }

    void Update()
    {
        // Старт игры и метронома по нажатию пробела
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            OnSpacePressed();
        }

        if (isGameStarted)
        {
            TrackMetronome();
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
    void OnSpacePressed()
    {
        // Проверяем, началась ли игра
        if (!isGameStarted)
        {
            isGameStarted = true;
            lastMetronomeTime = Time.time;
            lastKeyPressTime = Time.time;
            return; // Завершаем обработку, так как это первое нажатие
        }

        float currentTime = Time.time;
        float keyPressDelta = currentTime - lastMetronomeTime; // Разница между нажатием и временем метронома
        metronomeInterval = menuManager.interval; // Обновляем интервал метронома

        // Рассчитываем разницу между нажатием и текущим временем метронома
        float difference = keyPressDelta % metronomeInterval;

        // Если разница отрицательная, приводим ее к положительному виду
        if (difference > metronomeInterval / 2)
            difference -= metronomeInterval;

        // Определяем, какой спрайт активировать
        if (difference > 0 && difference > 0.35f * metronomeInterval)
        {
            ActivateSprite(leftRedSprite, -5.5f);
            menuManager.ResetStreak(); // Сбрасываем серию
        }
        else if (difference < 0 && Mathf.Abs(difference) > 0.35f * metronomeInterval)
        {
            ActivateSprite(rightRedSprite, 5.5f);
            menuManager.ResetStreak(); // Сбрасываем серию
        }
        else if (difference > 0 && difference >= 0.15f * metronomeInterval && difference <= 0.35f * metronomeInterval)
        {
            ActivateSprite(leftYellowSprite, -2.75f);
            menuManager.ResetStreak(); // Сбрасываем серию
        }
        else if (difference < 0 && Mathf.Abs(difference) >= 0.15f * metronomeInterval && Mathf.Abs(difference) <= 0.35f * metronomeInterval)
        {
            ActivateSprite(rightYellowSprite, 2.75f);
            menuManager.ResetStreak(); // Сбрасываем серию
        }
        else
        {
            ActivateSprite(greenSprite, 0f);
            menuManager.UpdateScore(); // Увеличиваем счет
        }

        lastKeyPressTime = currentTime; // Обновляем время последнего нажатия
    }

    void ActivateSprite(GameObject sprite, float xPosition)
    {
        // Отключаем все спрайты
        DeactivateAllSprites();

        // Активируем нужный спрайт
        sprite.SetActive(true);
        sprite.transform.position = new Vector3(xPosition, 0f, 0f);

        // Запускаем корутину для автоматического отключения
        StartCoroutine(DeactivateSpriteAfterDelay(sprite));
    }

    IEnumerator DeactivateSpriteAfterDelay(GameObject sprite)
    {
        yield return new WaitForSeconds(spriteActiveDuration);
        sprite.SetActive(false);
    }

    void DeactivateAllSprites()
    {
        greenSprite.SetActive(false);
        leftRedSprite.SetActive(false);
        rightRedSprite.SetActive(false);
        leftYellowSprite.SetActive(false);
        rightYellowSprite.SetActive(false);
    }
}