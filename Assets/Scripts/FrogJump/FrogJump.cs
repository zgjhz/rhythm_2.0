using UnityEngine;

public class FrogJump : MonoBehaviour
{
    public Transform startPosition;   // Позиция левого берега (стартовая)
    public Transform targetPosition;  // Позиция правого берега (целевая)
    public float jumpDuration = 1f;   // Время прыжка в секундах
    
    private bool isJumping = false;   // Флаг, указывающий на выполнение прыжка
    private bool onStartPosition = true; // Флаг, указывающий на текущее положение (стартовое или целевое)
    private Vector3 jumpStart;        // Начальная позиция прыжка
    private Vector3 jumpEnd;          // Конечная позиция прыжка
    private float jumpTime;           // Текущее время прыжка
    private bool isGamePaused = true; // Флаг, указывающий на состояние игры (на паузе или активна)

    void Start()
    {
        // Устанавливаем начальную позицию лягушки
        transform.position = startPosition.position;
        
    }

    void Update()
    {
        // Если игра на паузе и игрок нажимает пробел, возобновляем игру
        if (isGamePaused && Input.GetKeyDown(KeyCode.Space))
        {
            isGamePaused = false; // Снимаем паузу
            Jump(); // Начинаем прыжок
            return; // Завершаем выполнение этого кадра
        }

        // Если игра не на паузе и лягушка в прыжке
        if (!isGamePaused && isJumping)
        {
            // Инкрементируем время прыжка на каждом кадре
            jumpTime += Time.deltaTime;

            // Расчет нормализованного времени от 0 до 1
            float normalizedTime = jumpTime / jumpDuration;

            // Рассчет положения по параболе
            if (normalizedTime <= 1f)
            {
                // Движение по X и Z
                Vector3 newPosition = Vector3.Lerp(jumpStart, jumpEnd, normalizedTime);

                // Движение по Y по параболе
                float parabolaHeight = Mathf.Sin(Mathf.PI * normalizedTime); // Высота параболы меняется от 0 до 1 до 0
                newPosition.y += parabolaHeight;

                transform.position = newPosition;
            }
            else
            {
                // Прыжок завершен
                isJumping = false;
                isGamePaused = true; // Ставим игру на паузу после приземления
                onStartPosition = !onStartPosition; // Меняем текущее положение лягушки
                jumpTime = 0f; // Обнуляем время прыжка
                Debug.Log("Лягушка приземлилась!");
            }
        }
    }

    public void Jump()
    {
        if (isJumping) return; // Игнорируем, если уже в прыжке

        // Определяем начальную и конечную позиции прыжка
        jumpStart = transform.position;
        jumpEnd = onStartPosition ? targetPosition.position : startPosition.position;

        // Сбрасываем время прыжка
        jumpTime = 0f;

        // Устанавливаем флаг прыжка в истину
        isJumping = true;
    }

    public void Fall()
    {
        Debug.Log("Лягушка упала в воду!");
        transform.position = startPosition.position; // Возрождаем лягушку на стартовой позиции
        onStartPosition = true; // Устанавливаем флаг на начальную позицию
        isJumping = false; // Останавливаем прыжок
        isGamePaused = true; // Ставим игру на паузу после падения
        jumpTime = 0f; // Сбрасываем время прыжка после падения
    }
}
