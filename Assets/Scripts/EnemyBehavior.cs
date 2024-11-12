using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyBehavior : MonoBehaviour
{
    public Transform player;          // ������ �� ������ ������
    public float detectionRange = 5f; // ���������, � ������� ���� �������� ��������� �� �������
    public float moveSpeed = 2f;      // �������� ������������ �����

    private void Update()
    {
        // ��������� ��������� ����� ������ � �������
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // ���� ����� ��������� � ���� �����������, ���� �������� �������������
        if (distanceToPlayer < detectionRange)
        {
            // ����������� � ������
            Vector3 direction = (player.position - transform.position).normalized;

            // �������� ����� � ������� ������
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �������� ������������ � �������
        if (collision.gameObject.CompareTag("Player"))
        {
            // ������������ ������� �����
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
