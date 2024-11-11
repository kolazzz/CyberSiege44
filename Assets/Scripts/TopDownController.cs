using UnityEngine;

public class TopDownController : MonoBehaviour
{
    public float moveSpeed = 5f;  // �������� ������������ ���������
    public float rotationSpeed = 720f;  // �������� �������� ���������

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
        // �������� ����������� �������� �� ���� X � Z �� ������ ����� WASD
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // ����������� ������ ����������� ��� �������������� ��������� �� ���������
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // ������� ��������� � �����������
        if (direction.magnitude >= 0.1f)
        {
            // ��������� ���� �������� � ������������ ���������
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

            // ��������� ��������
            Vector3 moveDir = direction * moveSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + moveDir);
        }
    }
}

