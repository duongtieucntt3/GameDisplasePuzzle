using UnityEngine;

public class RotateGameObject : MonoBehaviour
{
    private float rotationSpeed = 90.0f; 
    private void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}