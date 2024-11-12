using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // цель (персонаж)
    

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position;
            transform.LookAt(target.position); // поворот камеры на персонажа
        }
    }
}
