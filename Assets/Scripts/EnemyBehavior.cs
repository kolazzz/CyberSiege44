using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyBehavior : MonoBehaviour
{
    public Transform player;          // Ссылка на объект игрока
    public float detectionRange = 5f; // Дистанция, с которой враг начинает следовать за игроком
    public float moveSpeed = 2f;      // Скорость передвижения врага

    private void Update()
    {
        // Проверяем дистанцию между врагом и игроком
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Если игрок находится в зоне обнаружения, враг начинает преследование
        if (distanceToPlayer < detectionRange)
        {
            // Направление к игроку
            Vector3 direction = (player.position - transform.position).normalized;

            // Движение врага в сторону игрока
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Проверка столкновения с игроком
        if (collision.gameObject.CompareTag("Player"))
        {
            // Перезагрузка текущей сцены
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
