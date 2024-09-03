using UnityEngine;

public class RhythmController : MonoBehaviour
{
    public float rhythmInterval = 1.0f; // Интервал ритма в секундах
    private float nextBeatTime;
    private int score = 0; // Счет
    private FrogJump frogJump; // Ссылка на компонент FrogJump
    private bool frogFell = false; // Флаг для проверки, упала ли лягушка
    private bool isGameStarted = false; // Флаг для проверки, началась ли игра

    void Start()
    {
        frogJump = FindObjectOfType<FrogJump>(); // Находим компонент FrogJump в сцене
        nextBeatTime = Time.time + rhythmInterval; // Установить время следующего удара
    }

    void Update()
    {
        if (Time.time >= nextBeatTime)
        {
            nextBeatTime += rhythmInterval; // Обновить время следующего удара
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isGameStarted)
            {
                // Если игра еще не началась и игрок нажимает пробел
                isGameStarted = true; // Устанавливаем флаг начала игры
                frogJump.Jump(); // Лягушка сразу начинает прыгать
                return; // Завершаем выполнение этого кадра
            }

            if (frogFell)
            {
                // Если лягушка упала и пользователь нажал пробел
                frogJump.Jump(); // Лягушка сразу начинает прыгать
                frogFell = false; // Сбрасываем флаг падения
                return; // Завершаем выполнение этого кадра
            }

            if (Mathf.Abs(Time.time - nextBeatTime) <= rhythmInterval / 2)
            {
                // Если игрок попал в ритм
                Debug.Log("Успешный прыжок!");
                score += 1; // Увеличить счет
                frogJump.Jump(); // Вызов метода прыжка
            }
            else
            {
                // Если игрок не попал в ритм
                Debug.Log("Пропуск!");
                frogJump.Fall(); // Лягушка падает в воду
                frogFell = true; // Устанавливаем флаг падения
            }
        }
    }

    public int GetScore()
    {
        return score;
    }
}

