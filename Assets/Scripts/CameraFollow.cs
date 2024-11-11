using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // Персонаж, за которым будет следовать камера
    public float followSpeed = 5f;  // Скорость, с которой камера будет следовать за персонажем
    public float fixedY = 10f; // Фиксированная высота камеры

    private void LateUpdate()
    {
        // Получаем желаемую позицию камеры (с учётом смещения)
        Vector3 targetPosition = target.position;
        targetPosition.y = fixedY;

        // Плавно перемещаем камеру к новой позиции без вращения
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // Зафиксируем поворот камеры, чтобы она не вращалась
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);  // Поворот камеры для вида сверху
    }
}
