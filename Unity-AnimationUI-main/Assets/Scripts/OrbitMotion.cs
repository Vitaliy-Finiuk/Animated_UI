using UnityEngine;

public class OrbitMotion : MonoBehaviour
{
    public Transform centerObject;  // объект, вокруг которого движется текущий объект
    public float speed;  // скорость вращения вокруг центрального объекта
    public float distance;  // расстояние между центральным объектом и текущим объектом
    public float angle;  // угол между текущим объектом и осью X

    void Update()
    {
        // вычисляем новую позицию объекта на орбите
        float x = centerObject.position.x + Mathf.Cos(angle) * distance;
        float y = centerObject.position.y + Mathf.Sin(angle) * distance;
        float z = centerObject.position.z;

        // устанавливаем новую позицию объекта
        transform.position = new Vector3(x, y, z);

        // увеличиваем угол для следующего кадра
        angle += speed * Time.deltaTime;
    }
}