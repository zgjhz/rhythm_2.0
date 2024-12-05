using UnityEngine;
using System.Collections;

public class FrogJump : MonoBehaviour
{
    public Transform startPosition;   // Позиция левого берега (стартовая)
    public Transform targetPosition;  // Позиция правого берега (целевая)
    public float jumpDuration = 1f;   // Время прыжка в секундах
    private bool onStartPosition = true; // Флаг, указывающий на текущее положение (стартовое или целевое)
    private Vector3 jumpStart;        // Начальная позиция прыжка
    private Vector3 jumpEnd;          // Конечная позиция прыжка
    private float jumpTime;           // Текущее время прыжка
    public MenuManager menuManager;   // Ссылка на MenuManager для интервала
    public SpriteRenderer spriteRenderer; // Спрайтрендерер лягушки
    public RhythmController rhythmController;
    public Sprite frogIdle;
    public Sprite frogStart;
    public Sprite frogJumping;
    public Sprite frogFalling;
    public Sprite frogLanding;

    private bool isJumping = false; // Флаг, показывающий, идет ли прыжок

    void Start()
    {
        ResetToStart(); // Инициализация в стартовой позиции
    }

    void Update()
    {
        // Обновляем длительность прыжка в зависимости от значения слайдера
        if (menuManager != null)
        {
            jumpDuration = menuManager.speedSlider.value;
        }

        // Если лягушка прыгает, выполняем анимацию прыжка
        if (isJumping)
        {
            jumpTime += Time.deltaTime;

            float adjustedJumpDuration = jumpDuration * 0.85f; // Длительность анимации прыжка
            float normalizedTime = jumpTime / adjustedJumpDuration;

            if (normalizedTime <= 1f)
            {
                // Двигаем лягушку по параболической траектории
                Vector3 newPosition = Vector3.Lerp(jumpStart, jumpEnd, normalizedTime);
                float heightMultiplier = 2.0f;
                float parabolaHeight = Mathf.Sin(Mathf.PI * normalizedTime) * heightMultiplier;
                newPosition.y += parabolaHeight;
                transform.position = newPosition;

                // Обновляем спрайты в зависимости от фазы прыжка
                if (normalizedTime < 0.2f)
                    spriteRenderer.sprite = frogStart;
                else if (normalizedTime < 0.5f)
                    spriteRenderer.sprite = frogJumping;
                else if (normalizedTime < 0.7f)
                    spriteRenderer.sprite = frogFalling;
                else
                    spriteRenderer.sprite = frogLanding;
            }
            else
            {
                isJumping = false;
                StartCoroutine(LandingPause()); // Начинаем паузу перед следующим прыжком
            }
        }
    }

    private void StartNextJump()
    {
        if (!isJumping)
        {
            jumpStart = transform.position;
            jumpEnd = onStartPosition ? targetPosition.position : startPosition.position;
            onStartPosition = !onStartPosition;

            // Инвертируем направление лягушки в зависимости от направления прыжка
            transform.localScale = new Vector3(onStartPosition ? -Mathf.Abs(transform.localScale.x) : Mathf.Abs(transform.localScale.x),
                                               transform.localScale.y,
                                               transform.localScale.z);

            jumpTime = 0f;
            isJumping = true; // Лягушка начинает прыжок
        }
    }
    

    private IEnumerator LandingPause()
    {
        spriteRenderer.sprite = frogIdle; // Меняем спрайт на состояние "ожидания"

        // Проверяем паузу
        while (rhythmController.isWaitingForFirstInput)
        {
            yield return null; // Ждем, пока пауза не закончится
        }
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        yield return new WaitForSeconds(jumpDuration * 0.05f); // Уменьшенное время паузы
        StartNextJump(); // Стартуем следующий прыжок
    }


    public void Jump()
    {
        if (!isJumping) // Лягушка не должна начинать новый прыжок, если уже прыгает
        {
            spriteRenderer.sprite = frogStart; // Меняем спрайт на старт
            StartNextJump(); // Начинаем прыжок
        }
    }

    public void ResetToStart()
    {
        transform.position = startPosition.position; // Возвращаем лягушку в начальное положение
        spriteRenderer.sprite = frogIdle; // Сбрасываем спрайт
        onStartPosition = true; // Сбрасываем флаг позиции
        isJumping = false; // Лягушка не прыгает
        // Восстанавливаем начальное направление
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }
}
