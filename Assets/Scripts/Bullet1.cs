using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;

    void Start()
    {
        Destroy(gameObject, 5f);  // �������� ���� ����� 5 ������
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // ����� ����� ���������� ��������� �� ������
            Destroy(gameObject);
        }
    }
}
