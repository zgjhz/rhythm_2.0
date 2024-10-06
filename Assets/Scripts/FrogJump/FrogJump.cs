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

    // Спрайты для различных стадий прыжка
    public Sprite frogIdle;          // Лягушка в покое
    public Sprite frogStart;         // Лягушка стартует
    public Sprite frogJumping;       // Лягушка в полете
    public Sprite frogFalling;       // Лягушка снижается
    public Sprite frogLanding;       // Лягушка приземляется

    private float landingPauseDuration; // Продолжительность паузы после приземления
    private bool isJumping = false;  // Флаг, указывающий, что лягушка в данный момент прыгает

    void Start()
    {
        // Получаем начальное значение длительности прыжка из MenuManager
        jumpDuration = menuManager.speedSlider.value;

        // Устанавливаем начальную позицию лягушки
        transform.position = startPosition.position;

        // Устанавливаем лягушку в покое
        spriteRenderer.sprite = frogIdle;

        // Запускаем первый прыжок
        StartNextJump();
    }

    void Update()
    {
        // Постоянно обновляем jumpDuration на основе значения из MenuManager
        jumpDuration = menuManager.speedSlider.value;

        // Определяем время паузы после приземления, чтобы в сумме с прыжком оно было равно jumpDuration
        landingPauseDuration = jumpDuration * 0.3f; // Пример: 30% от всего времени занимает пауза
        float adjustedJumpDuration = jumpDuration * 0.7f; // Остальное время занимает сам прыжок

        if (isJumping)
        {
            // Инкрементируем время прыжка
            jumpTime += Time.deltaTime;

            // Расчет нормализованного времени от 0 до 1 для самого прыжка
            float normalizedTime = jumpTime / adjustedJumpDuration;

            // Рассчет положения по параболе
            if (normalizedTime <= 1f)
            {
                // Движение по X и Z
                Vector3 newPosition = Vector3.Lerp(jumpStart, jumpEnd, normalizedTime);

                // Движение по Y по более выпуклой параболе
                float heightMultiplier = 2.0f; // Коэффициент увеличения высоты, чтобы сделать параболу более выпуклой
                float parabolaHeight = Mathf.Sin(Mathf.PI * normalizedTime) * heightMultiplier; // Высота параболы меняется от 0 до 2 до 0
                newPosition.y += parabolaHeight;

                transform.position = newPosition;

                // Обновляем спрайт в зависимости от стадии прыжка и нормализованного времени
                if (normalizedTime < 0.2f)
                {
                    spriteRenderer.sprite = frogStart;
                }
                else if (normalizedTime < 0.5f)
                {
                    spriteRenderer.sprite = frogJumping; // Лягушка в полете
                }
                else if (normalizedTime < 0.7f)
                {
                    spriteRenderer.sprite = frogFalling; // Лягушка снижается
                }
                else
                {
                    spriteRenderer.sprite = frogLanding; // Лягушка почти приземляется
                }
            }
            else
            {
                // Прыжок завершен — лягушка приземлилась, начинается пауза
                isJumping = false; // Заканчиваем текущий прыжок
                StartCoroutine(LandingPause());
            }
        }
    }

    // Метод для начала следующего прыжка
    private void StartNextJump()
    {
        if (!isJumping)
        {
            // Определяем начальную и конечную позиции прыжка
            jumpStart = transform.position;
            jumpEnd = onStartPosition ? targetPosition.position : startPosition.position;

            // Меняем текущее положение лягушки (левый или правый берег)
            onStartPosition = !onStartPosition;

            // Разворачиваем лягушку, чтобы она прыгала в правильном направлении
            if (onStartPosition)
            {
                // Лягушка прыгает обратно на стартовую позицию (налево)
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                // Лягушка прыгает к целевой позиции (направо)
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }

            // Сбрасываем время прыжка и начинаем новый
            jumpTime = 0f;
            isJumping = true; // Лягушка начинает новый прыжок
        }
    }

    // Метод для паузы после приземления
    private IEnumerator LandingPause()
    {
        // Устанавливаем спрайт приземлившейся лягушки
        spriteRenderer.sprite = frogIdle; // Лягушка приземлилась
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        // Ждем некоторое время (пауза после приземления)
        yield return new WaitForSeconds(landingPauseDuration);

        // Начинаем следующий прыжок после паузы
        StartNextJump();
    }

    // Метод для принудительного прыжка, вызываемый контроллером ритма
    public void Jump()
    {
        if (!isJumping)
        {
            // Начинаем прыжок
            spriteRenderer.sprite = frogStart; // Лягушка начинает движение
            StartNextJump();
        }
    }
}



