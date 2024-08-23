using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target; // Цель, за которой будет следовать камера (например, последняя линия)
    public float smoothSpeed = 0.125f; // Скорость плавного следования камеры
    public Vector3 offset; // Смещение камеры относительно цели
    public GameObject mainCamera;

    private void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.transform.position + offset; // Желаемая позиция камеры
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed); // Плавное движение камеры
            mainCamera.transform.position = smoothedPosition; // Установка новой позиции камеры
        }
        
    }
}
