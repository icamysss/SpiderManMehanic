using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] Transform player;
    [Range(0.1f, 100f)]
    [SerializeField] float rotationSpeed = 5f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    private void Update()
    {
        player.rotation *= Quaternion.Euler(Vector3.up * Input.GetAxis("Mouse X") * rotationSpeed);
        transform.localRotation *= Quaternion.Euler(-Vector3.right * Input.GetAxis("Mouse Y") * rotationSpeed);
    }
}