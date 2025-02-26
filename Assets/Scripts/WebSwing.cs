using UnityEngine;

public class WebSwing : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _camera;
    [SerializeField] private LineRenderer _webLine;
   // [SerializeField] private ParticleSystem _webEffect;
    [SerializeField] private Rigidbody _rb;

    [Header("Joint Settings")]
    [SerializeField] private float _spring = 45f;
    [SerializeField] private float _damper = 25f;
    [SerializeField] private float _massScale = 8f;
    [SerializeField] private float _frequency = 2f;

    [Header("Swing Physics")]
    [SerializeField] private float _maxSwingDistance = 50f;
    [SerializeField] private float _swingJumpForce = 120f;
    [SerializeField] private float _swingPullForce = 80f;
    [SerializeField] private LayerMask _swingLayer;

    private Vector3 _swingPoint;
    private SpringJoint _joint;
    private bool _isSwinging;
    private float _originalDrag;

    private void Start()
    {
        _originalDrag = _rb.linearDamping;
        _webLine.enabled = false;
    }

    private void Update()
    {
        HandleInput();
        DrawWeb();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0)) StartSwing();
        if (Input.GetMouseButtonUp(0)) ReleaseSwing();
    }

    private void StartSwing()
    {
        if (Physics.Raycast(_camera.position, _camera.forward, out RaycastHit hit, _maxSwingDistance, _swingLayer))
        {
            _isSwinging = true;
            _swingPoint = hit.point;
            
            // Настройка Joint под большой вес
            _joint = gameObject.AddComponent<SpringJoint>();
            _joint.autoConfigureConnectedAnchor = false;
            _joint.connectedAnchor = _swingPoint;
            
            _joint.spring = _spring;
            _joint.damper = _damper;
            _joint.massScale = _massScale;
            _joint.tolerance = 0.1f;
            
            // Динамическая настройка расстояния
            float distance = Vector3.Distance(transform.position, _swingPoint);
            _joint.maxDistance = distance * 0.8f;
            _joint.minDistance = distance * 0.25f;

            // Настройки для тяжёлого тела
            _rb.linearDamping = 1f; // Уменьшаем сопротивление во время качания
            _webLine.enabled = true;
          //  _webEffect.Play();
        }
    }

    private void ReleaseSwing()
    {
        _isSwinging = false;
        _webLine.enabled = false;
      //  _webEffect.Stop();
        _rb.linearDamping = _originalDrag;
        
        if (_joint != null) Destroy(_joint);
    }

    private void FixedUpdate()
    {
        if (_isSwinging)
        {
            Vector3 directionToPoint = _swingPoint - transform.position;
            
            // Динамическая сила тяги
            float distanceFactor = 1 - (directionToPoint.magnitude / _maxSwingDistance);
            _rb.AddForce(directionToPoint.normalized * _swingPullForce * distanceFactor, ForceMode.Acceleration);
            
            // Ручное управление инерцией
            Vector3 lateralVelocity = Vector3.ProjectOnPlane(_rb.linearVelocity, directionToPoint);
            _rb.AddForce(-lateralVelocity * 2f, ForceMode.Acceleration);
        }
    }

    private void DrawWeb()
    {
        if (!_isSwinging) return;
        _webLine.SetPosition(0, transform.position + new Vector3(0, 0.5f, 0));
        _webLine.SetPosition(1, _swingPoint);
    }
}