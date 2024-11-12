using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bulletPrefab;         // Префаб пули
    public Transform bulletSpawnPoint;      // Точка, откуда будет вылетать пуля
    public Transform player;                // Ссылка на объект игрока
    public float detectionRange = 10f;      // Радиус зоны видимости врага
    public float fireRate = 1f;             // Скорость стрельбы (выстрелы в секунду)
    public float bulletSpeed = 10f;         // Скорость пули

    private float nextFireTime = 0f;        // Время до следующего выстрела

    private void Update()
    {
        // Проверяем расстояние до игрока
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Если игрок находится в зоне видимости, враг начинает стрельбу
        if (distanceToPlayer < detectionRange)
        {
            // Поворачиваем врага к игроку
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

            // Если прошло достаточно времени для следующего выстрела
            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + 1f / fireRate;
            }
        }
    }

    private void Shoot()
    {
        // Создаём пулю в позиции bulletSpawnPoint и направляем её к игроку
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        // Игнорируем столкновения между пулей и врагом
        Physics.IgnoreCollision(bullet.GetComponent<Collider>(), GetComponent<Collider>());

        // Рассчитываем направление стрельбы в сторону игрока
        Vector3 direction = (player.position - bulletSpawnPoint.position).normalized;

        // Добавляем к пуле силу в направлении игрока
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed;
        }

        // Назначаем пуле обработчик столкновений
        bullet.AddComponent<BulletCollisionHandler>().enemyShooting = this;

        // Пуля автоматически уничтожится через 5 секунд, чтобы не засорять сцену
        Destroy(bullet, 5f);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private class BulletCollisionHandler : MonoBehaviour
    {
        public EnemyShooting enemyShooting;

        private void OnCollisionEnter(Collision collision)
        {
            // Проверка, столкнулась ли пуля с игроком
            if (collision.gameObject.CompareTag("Player"))
            {
                // Перезагрузка текущей сцены
                enemyShooting.ReloadScene();
            }

            // Уничтожение пули при любом столкновении
            Destroy(gameObject);
        }
    }
}
