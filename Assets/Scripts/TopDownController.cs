using UnityEngine;

public class TopDownController : MonoBehaviour
{
    public float moveSpeed = 5f;  // Скорость передвижения персонажа
    public float rotationSpeed = 720f;  // Скорость поворота персонажа

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        // Получаем направления движения по осям X и Z на основе ввода WASD
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Нормализуем вектор направления для предотвращения ускорения по диагонали
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Двигаем персонажа в направлении
        if (direction.magnitude >= 0.1f)
        {
            // Вычисляем угол поворота и поворачиваем персонажа
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

            // Применяем движение
            Vector3 moveDir = direction * moveSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + moveDir);
        }
    }
}

