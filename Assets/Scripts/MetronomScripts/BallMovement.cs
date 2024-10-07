using UnityEngine;
using System.Collections;
using TMPro;

public class BallMovement : MonoBehaviour
{
    public MenuManager menuManager;
    public float speed = 5f; // Скорость движения мяча
    public float delayAfterHit = 0.07f; // Задержка после удара в секундах
    public AudioClip hitSound; // Звук удара
    private AudioSource audioSource; // Аудиоисточник
    private Vector2 direction = Vector2.right;
    private bool canMove = true;
    private int score = 0;

    private float interval = 0f;
    public TMP_Text scoreText;
    public float wallBoundary = 7.4f; // Позиция стен
    public WallHighlightController wallHighlightControllerLeft;
    public WallHighlightController wallHighlightControllerRight;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.volume = 1.0f;
    }

    void Update()
    {
        interval = menuManager.interval;
        if (canMove)
        {
            speed = wallBoundary / interval;
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckHit();
        }
    }

   private IEnumerator HandleWallHit(WallHighlightController wallHighlightController)
{
    canMove = false;
    PlayHitSound();

    // Запуск подсветки
    if (wallHighlightController != null)
    {
        wallHighlightController.TriggerHighlight();
    }

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
            audioSource.clip = hitSound;
            audioSource.Play();
        }
    }

    void CheckHit()
    {
        if (Mathf.Abs(transform.position.x - (direction == Vector2.right ? wallBoundary : -wallBoundary)) < 0.1f)
        {
            Debug.Log("Удар успешен!");
            score ++;
            scoreText.text = "Счёт: " + score;
        }
        else
        {
            Debug.Log("Промах!");
        }
    }
}
