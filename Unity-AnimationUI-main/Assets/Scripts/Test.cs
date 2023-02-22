using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform target;
    public float distance = 5f;
    public float minDistance = 2f;
    public float maxDistance = 20f;
    public float zoomSpeed = 5f;
    public float rotationSpeed = 100f;
    public float maxRotationDistance = 10f;
    public float sensitivity = 1f;

    private float currentDistance;
    private float mouseX, mouseY;
    private bool followCursor = false;

    private void Start()
    {
        currentDistance = distance;
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            // Calculate distance to target
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            // Zoom with mouse wheel
            float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");
            if (scrollWheelInput != 0f)
            {
                currentDistance -= scrollWheelInput * zoomSpeed;
                currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);
            }

            // Rotate with mouse
            if (distanceToTarget > maxRotationDistance || followCursor)
            {
                if (Input.GetMouseButton(0))
                {
                    if (!followCursor)
                    {
                        mouseX += Input.GetAxis("Mouse X") * rotationSpeed * currentDistance * Time.deltaTime * sensitivity;
                        mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed * currentDistance * Time.deltaTime * sensitivity;
                        mouseY = Mathf.Clamp(mouseY, -90f, 90f);
                    }
                    else
                    {
                        // Follow cursor
                        Vector3 mousePosition = Input.mousePosition;
                        mouseX += (mousePosition.x - Screen.width / 2f) * rotationSpeed * Time.deltaTime * sensitivity;
                        mouseY -= (mousePosition.y - Screen.height / 2f) * rotationSpeed * Time.deltaTime * sensitivity;
                        mouseY = Mathf.Clamp(mouseY, -90f, 90f);
                    }
                }
            }

            // Calculate new camera position
            Quaternion rotation = Quaternion.Euler(mouseY, mouseX, 0f);
            Vector3 position;

            if (followCursor)
            {
                // Follow cursor
                Vector3 mousePosition = Input.mousePosition;
                mousePosition.z = currentDistance;
                position = Camera.main.ScreenToWorldPoint(mousePosition);
            }
            else
            {
                // Follow target
                position = rotation * new Vector3(0f, 0f, -currentDistance) + target.position;
            }

            // Apply new position and rotation to camera
            transform.rotation = rotation;
            transform.position = position;

            // Check if within radius and right mouse button pressed
            if (distanceToTarget <= maxRotationDistance && Input.GetMouseButton(0))
            {
                // Follow cursor
                followCursor = true;
            }
            else
            {
                // Reset follow cursor
                followCursor = false;
            }
        }
    }
}
