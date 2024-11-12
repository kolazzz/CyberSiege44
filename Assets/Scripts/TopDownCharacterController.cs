using UnityEngine;

public class TopDownShooterController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 100f;      // Скорость движения персонажа
    [SerializeField] private float rotationSpeed = 700f;  // Скорость поворота персонажа

    private Vector3 moveDirection;    // Направление движения
    private Rigidbody rb;             // Ссылка на Rigidbody

    void Start()
    {
        // Получаем ссылку на Rigidbody
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Получаем ввод от пользователя
        float moveX = Input.GetAxisRaw("Horizontal"); // A/D или стрелки влево/вправ
        float moveZ = Input.GetAxisRaw("Vertical");   // W/S или стрелки вверх/вниз

        // Вычисляем направление движения
        moveDirection = new Vector3(moveX, 0f, moveZ).normalized;

        // Если персонаж двигается, то поворачиваем его в направлении движения
        if (moveDirection.magnitude >= 0.1f)
        {
            RotateTowardsMovementDirection();
        }

        // Двигаем персонажа
        MoveCharacter();
    }

    // Функция для поворота персонажа в сторону движения
    void RotateTowardsMovementDirection()
    {
        // Вычисляем угол между вектором направления и вектором вперед
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

        // Плавно поворачиваем персонажа
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    // Функция для движения персонажа
    void MoveCharacter()
    {
        Vector3 velocity = moveDirection * moveSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + velocity);
    }
}
