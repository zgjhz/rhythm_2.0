using System.Collections;
using UnityEngine;

public class Svetoforscript : MonoBehaviour, ISpacePressHandler
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
        spriteActiveDuration = menuManager.interval * 0.5f;
        DeactivateAllSprites();

        // Инициализация AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = metronomeSound;
        audioSource.playOnAwake = false; // Чтобы звук не проигрывался сразу
    }

    void Update()
    {
        spriteActiveDuration = menuManager.interval * 0.45f;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnSpacePressed();
        }

        if (isGameStarted)
        {
            metronomeInterval = menuManager.interval;
        }
    }

    public void OnSpacePressed()
    {
        float currentTime = Time.time;
        float keyPressDelta = currentTime - lastKeyPressTime;

        if (!isGameStarted)
        {
            isGameStarted = true;
            lastMetronomeTime = currentTime;
            lastKeyPressTime = currentTime;
            return;
        }

        // Рассчитываем разницу между временем удара метронома и временем нажатия
        float difference = keyPressDelta - metronomeInterval;

        // Определяем, какой спрайт зажечь
        if (difference > 0 && difference > 0.7f * metronomeInterval)
        {
            ActivateSprite(leftRedSprite, -5.5f);
            menuManager.ResetStreak();
        }
        else if (difference < 0 && Mathf.Abs(difference) > 0.7f * metronomeInterval)
        {
            ActivateSprite(rightRedSprite, 5.5f);
            menuManager.ResetStreak();
        }
        else if (difference > 0 && difference >= 0.3f * metronomeInterval && difference <= 0.7f * metronomeInterval)
        {
            ActivateSprite(leftYellowSprite, -2.75f);
            menuManager.ResetStreak();
        }
        else if (difference < 0 && Mathf.Abs(difference) >= 0.3f * metronomeInterval && Mathf.Abs(difference) <= 0.7f * metronomeInterval)
        {
            ActivateSprite(rightYellowSprite, 2.75f);
            menuManager.ResetStreak();
        }
        else
        {
            ActivateSprite(greenSprite, 0f);
            menuManager.UpdateScore();
        }

        lastKeyPressTime = currentTime;
    }

    void ActivateSprite(GameObject sprite, float xPosition)
    {
        DeactivateAllSprites();
        sprite.SetActive(true);
        sprite.transform.position = new Vector3(xPosition, 0f, 0f);
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
