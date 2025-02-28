using UnityEngine;

public class CombinedCameraController : MonoBehaviour
{
    [SerializeField] private Transform startLookPoint;
    [SerializeField] float mouseSensitivity = 100f;
    [SerializeField] float verticalAngleLimit = 80f;

    private float _xRotation;
    private float _yRotation;
    private Quaternion _targetRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        transform.LookAt(startLookPoint.position);
        // Инициализируем начальные углы
        Vector3 startEuler = transform.eulerAngles;
        _xRotation = startEuler.x;
        _yRotation = startEuler.y;
    }

    private void Update()
    {
        HandleMouseInput();
        HandleCursorToggle();
        ApplyRotation();
    }

    private void HandleMouseInput()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Вертикальное вращение с ограничением
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -verticalAngleLimit, verticalAngleLimit);

        // Горизонтальное вращение
        _yRotation += mouseX;
    }

    private void ApplyRotation()
    {
        // Создаем целевой поворот
        _targetRotation = Quaternion.Euler(_xRotation, _yRotation, 0f);
        
        // Плавное вращение
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            _targetRotation,
            25f * Time.deltaTime // Коэффициент сглаживания
            );
    }

    private void HandleCursorToggle()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool isLocked = Cursor.lockState == CursorLockMode.Locked;
            Cursor.lockState = isLocked ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isLocked;
        }
    }
}