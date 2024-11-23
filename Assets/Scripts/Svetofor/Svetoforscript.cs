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
    private MenuManager arseniiprivetenuManager;
    public float metronomeInterval = 1f; // Интервал между ударами метронома
    private float lastMetronomeTime;
    private float lastKeyPressTime;

    private bool isGameStarted = false;

    void Start()

    {
        lastMetronomeTime = Time.time;
        lastKeyPressTime = Time.time;

        // Делаем все спрайты неактивными в начале
        greenSprite.SetActive(false);
        leftRedSprite.SetActive(false);
        rightRedSprite.SetActive(false);
        leftYellowSprite.SetActive(false);
        rightYellowSprite.SetActive(false);

        // Инициализация AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = metronomeSound;
        audioSource.playOnAwake = false; // Чтобы звук не проигрывался сразу
    }

    void Update()
    {
        // Старт игры по нажатию пробела
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isGameStarted)
            {
                isGameStarted = true;
                lastMetronomeTime = Time.time;
                lastKeyPressTime = Time.time;
                StartCoroutine(Metronome());
            }
            else
            {
                metronomeInterval = arseniiprivetenuManager.interval;
                float currentTime = Time.time;
                float keyPressDelta = currentTime - lastKeyPressTime;
                float metronomeDelta = (currentTime - lastMetronomeTime) % metronomeInterval;

                // Рассчитываем разницу между временем удара метронома и временем нажатия
                float difference = keyPressDelta - metronomeDelta;

                // Определяем, какой спрайт зажечь
                if (difference > 0 && difference > 0.7f * metronomeInterval)
                {
                    ActivateSprite(leftRedSprite, -5.5f);
                }
                else if (difference < 0 && Mathf.Abs(difference) > 0.7f * metronomeInterval)
                {
                    ActivateSprite(rightRedSprite, 5.5f);
                }
                else if (difference > 0 && difference >= 0.3f * metronomeInterval && difference <= 0.7f * metronomeInterval)
                {
                    ActivateSprite(leftYellowSprite, -2.75f);
                }
                else if (difference < 0 && Mathf.Abs(difference) >= 0.3f * metronomeInterval && Mathf.Abs(difference) <= 0.7f * metronomeInterval)
                {
                    ActivateSprite(rightYellowSprite, 2.75f);
                }
                else
                {
                    ActivateSprite(greenSprite, 0f);
                }

                lastKeyPressTime = currentTime;
            }
        }
    }

    IEnumerator Metronome()
    {
        while (isGameStarted)
        {
            // Проигрываем звук метронома
            if (audioSource != null && metronomeSound != null)
            {
                audioSource.Play();
            }

            Debug.Log("Metronome Beat");
            lastMetronomeTime = Time.time;

            yield return new WaitForSeconds(metronomeInterval);
        }
    }

    void ActivateSprite(GameObject sprite, float xPosition)
    {
        // Отключаем все спрайты
        greenSprite.SetActive(false);
        leftRedSprite.SetActive(false);
        rightRedSprite.SetActive(false);
        leftYellowSprite.SetActive(false);
        rightYellowSprite.SetActive(false);

        // Активируем нужный спрайт
        sprite.SetActive(true);
        sprite.transform.position = new Vector3(xPosition, 0f, 0f);
    }
}
