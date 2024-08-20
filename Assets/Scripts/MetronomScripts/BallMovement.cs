using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public float speed = 5f; // Скорость движения мячика
    private Vector2 direction = Vector2.right;

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        // Проверяем, нажал ли игрок на пробел
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckHit();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Left Wall" || collision.gameObject.name == "Right Wall")
        {
            direction = -direction; // Меняем направление при столкновении с пластиной
        }
    }

    void CheckHit()
    {
        // Проверяем, рядом ли мяч с одной из стен
        if (Mathf.Abs(transform.position.x - (direction == Vector2.right ? 1 : -1) * (speed * Time.deltaTime)) < 0.1f)
        {
            Debug.Log("Удар успешен!");
        }
        else
        {
            Debug.Log("Промах!");
        }
    }
}
