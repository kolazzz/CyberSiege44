using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // ��������, �� ������� ����� ��������� ������
    public float followSpeed = 5f;  // ��������, � ������� ������ ����� ��������� �� ����������
    public float fixedY = 10f; // ������������� ������ ������

    private void LateUpdate()
    {
        // �������� �������� ������� ������ (� ������ ��������)
        Vector3 targetPosition = target.position;
        targetPosition.y = fixedY;

        // ������ ���������� ������ � ����� ������� ��� ��������
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // ����������� ������� ������, ����� ��� �� ���������
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);  // ������� ������ ��� ���� ������
    }
}
