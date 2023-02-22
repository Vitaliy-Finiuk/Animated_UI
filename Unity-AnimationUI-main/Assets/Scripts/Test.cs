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

    private float _currentDistance;
    private float _mouseX, _mouseY;
    private bool _followCursor = false;

    private void Start() => 
        _currentDistance = distance;

    private void LateUpdate()
    {
        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");
            if (scrollWheelInput != 0f)
            {
                _currentDistance -= scrollWheelInput * zoomSpeed;
                _currentDistance = Mathf.Clamp(_currentDistance, minDistance, maxDistance);
            }

            if (distanceToTarget > maxRotationDistance || _followCursor)
            {
                if (Input.GetMouseButton(0))
                {
                    if (!_followCursor)
                    {
                        _mouseX += Input.GetAxis("Mouse X") * rotationSpeed * _currentDistance * Time.deltaTime * sensitivity;
                        _mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed * _currentDistance * Time.deltaTime * sensitivity;
                        _mouseY = Mathf.Clamp(_mouseY, -90f, 90f);
                    }
                    else
                    {
                        Vector3 mousePosition = Input.mousePosition;
                        _mouseX += (mousePosition.x - Screen.width / 2f) * rotationSpeed * Time.deltaTime * sensitivity;
                        _mouseY -= (mousePosition.y - Screen.height / 2f) * rotationSpeed * Time.deltaTime * sensitivity;
                        _mouseY = Mathf.Clamp(_mouseY, -90f, 90f);
                    }
                }
            }

            Quaternion rotation = Quaternion.Euler(_mouseY, _mouseX, 0f);
            Vector3 position;

            if (_followCursor)
            {
                Vector3 mousePosition = Input.mousePosition;
                mousePosition.z = _currentDistance;
                position = Camera.main.ScreenToWorldPoint(mousePosition);
            }
            else
                position = rotation * new Vector3(0f, 0f, -_currentDistance) + target.position;

            transform.rotation = rotation;
            transform.position = position;

            if (distanceToTarget <= maxRotationDistance && Input.GetMouseButton(0))
                _followCursor = true;
            else
                _followCursor = false;
        }
    }
}
