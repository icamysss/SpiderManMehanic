using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Transform _player;
    [SerializeField] public float _mouseSensitivity = 100f;
    [SerializeField] private float _distanceFromPlayer = 5f;
    [SerializeField] private Vector2 _verticalAngleLimits = new Vector2(-40f, 80f);
    [SerializeField] private float _smoothTime = 0.1f;

    private float _xRotation;
    private float _yRotation;
    private Vector3 _currentRotationVelocity;
    private Vector3 _targetPosition;

    private void Start()
    {
        // Инициализация начальных углов
        Vector3 initialRotation = transform.eulerAngles;
        _xRotation = initialRotation.x;
        _yRotation = initialRotation.y;
    }

    private void Update()
    {
        HandleMouseInput();
    }

    private void LateUpdate()
    {
        UpdateCameraPositionAndRotation();
    }

    private void HandleMouseInput()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

        _yRotation += mouseX;
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, _verticalAngleLimits.x, _verticalAngleLimits.y);
    }

    private void UpdateCameraPositionAndRotation()
    {
        // Рассчитываем целевую позицию и вращение
        Quaternion targetRotation = Quaternion.Euler(_xRotation, _yRotation, 0f);
        Vector3 direction = targetRotation * Vector3.back;
        _targetPosition = _player.position + direction * _distanceFromPlayer;

        // Плавное перемещение и вращение
        transform.position = Vector3.SmoothDamp(
            transform.position, 
            _targetPosition, 
            ref _currentRotationVelocity, 
            _smoothTime
        );

        transform.rotation = Quaternion.Lerp(
            transform.rotation, 
            targetRotation,
            Time.deltaTime * 15f
        );
    }
}