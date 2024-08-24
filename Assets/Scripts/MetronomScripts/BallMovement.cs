using UnityEngine;
using System.Collections;

public class BallMovement : MonoBehaviour
{
    public float speed = 5f; // Скорость движения мяча
    public float delayAfterHit = 0.07f; // Задержка после удара в секундах
    public AudioClip hitSound; // Звук удара
    private AudioSource audioSource; // Аудиоисточник
    private Vector2 direction = Vector2.right;
    private bool canMove = true;
    public float wallBoundary = 8f; // Позиция стен

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // Звук не воспроизводится при старте
        audioSource.volume = 1.0f; // Убедитесь, что громкость установлена правильно
    }

    void Update()
    {
        if (canMove)
        {
            transform.Translate(direction * speed * Time.deltaTime);

            // Проверка на достижение границ
            if (transform.position.x <= -wallBoundary || transform.position.x >= wallBoundary)
            {
                StartCoroutine(HandleWallHit()); // Обрабатываем удар
            }
        }

        // Проверка нажатия пробела
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckHit();
        }
    }

    private IEnumerator HandleWallHit()
    {
        canMove = false; // Остановить движение
        PlayHitSound(); // Воспроизвести звук удара

        // Корректируем позицию мяча, чтобы он не застрял в стене
        if (transform.position.x <= -wallBoundary)
        {
            transform.position = new Vector2(-wallBoundary + 0.1f, transform.position.y); // Смещаем немного от стены
        }
        else if (transform.position.x >= wallBoundary)
        {
            transform.position = new Vector2(wallBoundary - 0.1f, transform.position.y); // Смещаем немного от стены
        }

        yield return new WaitForSeconds(delayAfterHit); // Задержка

        direction = -direction; // Изменить направление
        canMove = true; // Возобновить движение
    }

    void PlayHitSound()
    {
        if (!audioSource.isPlaying) // Проверка, чтобы звук не наложился друг на друга
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
        }
        else
        {
            Debug.Log("Промах!");
        }
    }
}
