using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("��������� �����")]
    [SerializeField] private float detectionRadius = 5f;  // ������ ���������� ����
    [SerializeField] private float shootInterval = 1f;    // �������� ����� ����������
    [SerializeField] private GameObject bulletPrefab;     // ������ ����
    [SerializeField] private Transform shootingPoint;     // �����, ������ ����� ������ ����
    [SerializeField] private Transform player;            // ���� (�����)

    private bool isPlayerInRange = false;
    private float lastShotTime;

    void Update()
    {
        // ���������, � ���� �� �����
        if (isPlayerInRange)
        {
            // ��������� �� ������: ��������, ��������, ���� ������ ���������� �������
            if (Time.time - lastShotTime >= shootInterval)
            {
                Shoot();
                lastShotTime = Time.time;
            }
        }
    }

    // �������� �� ��������� ������ � �������
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            player = other.transform;  // ������������� ���� (������)
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            player = null;  // ���������� ����
        }
    }

    // ������ ��������
    void Shoot()
    {
        if (bulletPrefab != null && shootingPoint != null && player != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
            Vector3 direction = (player.position - shootingPoint.position).normalized;
            bullet.GetComponent<Rigidbody2D>().linearVelocity = direction * 10f;  // ��������� �������� ����
        }
    }

    // ��������� ���������� ���� ����� ��������� (���� �����)
    public void SetDetectionRadius(float radius)
    {
        detectionRadius = radius;
    }
}
