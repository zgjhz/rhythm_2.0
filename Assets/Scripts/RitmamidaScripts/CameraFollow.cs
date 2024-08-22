using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Цель, за которой будет следовать камера (например, последняя линия)
    public float smoothSpeed = 0.125f; // Скорость плавного следования камеры
    public Vector3 offset; // Смещение камеры относительно цели

    private void LateUpdate()
    {
        //if (target != null)
        //{
        //    Vector3 desiredPosition = target.position + offset; // Желаемая позиция камеры
        //    Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed); // Плавное движение камеры
        //    transform.position = smoothedPosition; // Установка новой позиции камеры
        //}
    }
}
