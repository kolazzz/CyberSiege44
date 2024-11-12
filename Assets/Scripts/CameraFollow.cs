using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // ���� (��������)
    

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position;
            transform.LookAt(target.position); // ������� ������ �� ���������
        }
    }
}
