using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] float verticalAngleLimit = 80f;

    private float _mouseX;
    private float _mouseY;
    private float _currentXRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _currentXRotation = transform.localEulerAngles.x;
    }

    private void Update()
    {
        // Получаем ввод мыши
        _mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        _mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible =  !Cursor.visible;
        }
    }

    private void LateUpdate()
    {
        // Горизонтальное вращение игрока
        player.Rotate(Vector3.up * _mouseX);

        // Вертикальное вращение камеры с ограничением угла
        _currentXRotation -= _mouseY;
        _currentXRotation = Mathf.Clamp(_currentXRotation, -verticalAngleLimit, verticalAngleLimit);

        // Применяем вращение с интерполяцией
        transform.localRotation = Quaternion.Lerp(
            transform.localRotation,
            Quaternion.Euler(_currentXRotation, 0, 0),
            Time.deltaTime * rotationSpeed * 10f
            );
    }
}