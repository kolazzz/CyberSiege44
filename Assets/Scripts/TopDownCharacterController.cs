using UnityEngine;

public class TopDownShooterController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 100f;      // �������� �������� ���������
    [SerializeField] private float rotationSpeed = 700f;  // �������� �������� ���������

    private Vector3 moveDirection;    // ����������� ��������
    private Rigidbody rb;             // ������ �� Rigidbody

    void Start()
    {
        // �������� ������ �� Rigidbody
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // �������� ���� �� ������������
        float moveX = Input.GetAxisRaw("Horizontal"); // A/D ��� ������� �����/�����
        float moveZ = Input.GetAxisRaw("Vertical");   // W/S ��� ������� �����/����

        // ��������� ����������� ��������
        moveDirection = new Vector3(moveX, 0f, moveZ).normalized;

        // ���� �������� ���������, �� ������������ ��� � ����������� ��������
        if (moveDirection.magnitude >= 0.1f)
        {
            RotateTowardsMovementDirection();
        }

        // ������� ���������
        MoveCharacter();
    }

    // ������� ��� �������� ��������� � ������� ��������
    void RotateTowardsMovementDirection()
    {
        // ��������� ���� ����� �������� ����������� � �������� ������
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

        // ������ ������������ ���������
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    // ������� ��� �������� ���������
    void MoveCharacter()
    {
        Vector3 velocity = moveDirection * moveSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + velocity);
    }
}
