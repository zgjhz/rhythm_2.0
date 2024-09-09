using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target; // Цель, за которой будет следовать камера (например, последняя линия)
    public float smoothSpeed = 0.125f; // Скорость плавного следования камеры
    public Vector3 offset; // Смещение камеры относительно цели
    private bool isPaused = false; // Флаг для проверки состояния паузы

    private void Start()
    {
        // Подписка на событие паузы, если оно существует
        // В этом случае, мы можем подписаться на статическое событие или установить флаг из другого скрипта
        Ritmamida.OnPauseStateChanged += HandlePauseStateChanged;
    }

    private void OnDestroy()
    {
        // Отписываемся от события при уничтожении объекта
        Ritmamida.OnPauseStateChanged -= HandlePauseStateChanged;
    }

    private void LateUpdate()
    {
        // Если игра на паузе, не обновляем позицию камеры
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
}
