using UnityEngine;

public class KeyRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 250.0f;
    void FixedUpdate()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
