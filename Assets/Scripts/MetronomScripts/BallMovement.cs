using UnityEngine;
using System.Collections;
using TMPro;

public class BallMovement : MonoBehaviour, ISpacePressHandler
{
    public MenuManager menuManager;
    public float speed = 5f; // Скорость движения мяча
    public float delayAfterHit = 0.07f; // Задержка после удара в секундах
    public AudioClip hitSound; // Звук удара
    private AudioSource audioSource; // Аудиоисточник
    private Vector2 direction = Vector2.right;
    private bool canMove = false; // Игра начинается в состоянии паузы
    private bool isPaused = true; // Флаг для паузы
    public TMP_Text scoreText;
    public float wallBoundary = 7.4f; // Позиция стен
    public WallHighlightController wallHighlightControllerLeft;
    public WallHighlightController wallHighlightControllerRight;
    private float interval = 0f;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.volume = 1.0f;
    }

    void Update()
    {
        interval = menuManager.interval - 0.085f;

        // Обработка нажатий на пробел вынесена в отдельную функцию
<<<<<<< HEAD
        if (Input.GetKeyDown(KeyCode.Space)|| Input.GetKeyDown(KeyCode.Return))
=======
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
>>>>>>> e5035b2c24e2bd822a1d46015c32dfe982ebb47d
        {
            OnSpacePressed();
        }

        // Если мяч готов к движению и игра не на паузе
        if (canMove && !isPaused)
        {
            speed = 2 * wallBoundary / interval;
            transform.Translate(direction * speed * Time.deltaTime);

            if (transform.position.x >= wallBoundary)
            {
                StartCoroutine(HandleWallHit(wallHighlightControllerRight));
            }
            else if (transform.position.x <= -wallBoundary)
            {
                StartCoroutine(HandleWallHit(wallHighlightControllerLeft));
            }
        }
    }

    public void OnSpacePressed()
    {
        if (isPaused)
        {
            StartGame(); // Начинаем или возобновляем игру
        }
        else
        {
            CheckHit(); // Выполняем удар, если игра не на паузе
        }
    }

    // Функция для старта и возобновления игры
    void StartGame()
    {
        isPaused = false;
        canMove = true;
    }

    public void setToCenter()
    {
        transform.position = new Vector2(0, 0);
        canMove = false; // Сбрасываем флаг движения после изменения скорости
        isPaused = true; // Устанавливаем паузу
    }

    private IEnumerator HandleWallHit(WallHighlightController wallHighlightController)
    {
        canMove = false;
        PlayHitSound();

        // Логика изменения направления мяча
        if (transform.position.x <= -wallBoundary)
        {
            transform.position = new Vector2(-wallBoundary + 0.1f, transform.position.y);
        }
        else if (transform.position.x >= wallBoundary)
        {
            transform.position = new Vector2(wallBoundary - 0.1f, transform.position.y);
        }

        yield return new WaitForSeconds(delayAfterHit);

        direction = -direction;
        canMove = true;
    }

    void PlayHitSound()
    {
        if (!audioSource.isPlaying)
        {
            int audioIndex = PlayerPrefs.GetInt("chosen_sound") - 1;
            audioSource.clip = menuManager.metronomAudioClips[audioIndex];
            audioSource.Play();
        }
    }

    void CheckHit()
{
    float distance = Mathf.Abs(transform.position.x - (direction == Vector2.right ? wallBoundary : -wallBoundary));
    WallHighlightController activeWallHighlightController = direction == Vector2.right ? wallHighlightControllerRight : wallHighlightControllerLeft;

    if (distance < 2.1f) // Точное попадание
    {
        Debug.Log("Удар успешен!");
        activeWallHighlightController.TriggerHighlight(Color.green);
        menuManager.UpdateScore();
    }
    else if (distance < 4.9f ) // Небольшой промах
    {
        Debug.Log("Небольшой промах!");
        activeWallHighlightController.TriggerHighlight(Color.yellow);
        menuManager.ResetStreak();
    }
    else // Большой промах
    {
        Debug.Log("Большой промах!");
        activeWallHighlightController.TriggerHighlight(Color.red);
        menuManager.ResetStreak();
    }
}

}
