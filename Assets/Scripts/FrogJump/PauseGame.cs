using UnityEngine;

public class PauseGame : MonoBehaviour
{
    private bool isPaused = false;

    // Метод для остановки и возобновления игры
    public void TogglePause()
    {
        if (isPaused)
        {
            Time.timeScale = 1f;  // Возобновляем игру
        }
        else
        {
            Time.timeScale = 0f;  // Останавливаем игру
        }

        isPaused = !isPaused;  // Меняем флаг
    }
}
