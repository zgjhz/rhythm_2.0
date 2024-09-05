using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target; // Цель, за которой будет следовать камера (например, последняя линия)
    public float smoothSpeed = 0.125f; // Скорость плавного следования камеры
    public Vector3 offset; // Смещение камеры относительно цели
    private bool isPaused = false; // Флаг для проверки состояния паузы
    public KeyCode pauseKey = KeyCode.P; // Клавиша для паузы

    private void Start()
    {
        // Подписка на событие паузы, если оно существует
        Ritmamida.OnPauseStateChanged += HandlePauseStateChanged;
    }

    private void OnDestroy()
    {
        // Отписываемся от события при уничтожении объекта
        Ritmamida.OnPauseStateChanged -= HandlePauseStateChanged;
    }

    private void Update()
    {
        // Обрабатываем нажатие клавиши паузы
        if (Input.GetKeyDown(pauseKey))
        {
            TogglePause();
        }
    }

    private void LateUpdate()
    {
        // Если игра на паузе или цель не задана, не обновляем позицию камеры
        if (isPaused || target == null)
            return;

        // Рассчитываем желаемую позицию камеры
        Vector3 desiredPosition = target.transform.position + offset;
        // Плавное движение камеры
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        // Устанавливаем новую позицию камеры
        transform.position = smoothedPosition;
    }

    // Метод для обработки изменения состояния паузы
    private void HandlePauseStateChanged(bool paused)
    {
        isPaused = paused;
    }

    // Метод для переключения состояния паузы
    private void TogglePause()
    {
        // Если игра уже на паузе, снимаем паузу
        if (isPaused)
        {
            Time.timeScale = 1f; // Возвращаем время к нормальной скорости
            isPaused = false;
        }
        // Иначе ставим на паузу
        else
        {
            Time.timeScale = 0f; // Останавливаем время
            isPaused = true;
        }
    }
}

