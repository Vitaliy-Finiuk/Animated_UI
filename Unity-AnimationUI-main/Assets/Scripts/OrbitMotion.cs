using UnityEngine;

public class OrbitMotion : MonoBehaviour
{
    public Transform centerObject;  
    public float speed;  
    public float distance;  
    public float angle;

    private void Update()
    {
        float x = centerObject.position.x + Mathf.Cos(angle) * distance;
        float y = centerObject.position.y + Mathf.Sin(angle) * distance;
        float z = centerObject.position.z;

        transform.position = new Vector3(x, y, z);

        angle += speed * Time.deltaTime;
    }
}