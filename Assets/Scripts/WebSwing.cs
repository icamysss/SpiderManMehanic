using UnityEngine;

public class WebSwing : MonoBehaviour
{
     private Transform _camera;
     private LineRenderer _webLine;
     private Rigidbody _rb;

    [Header("Joint Settings")]
    [SerializeField] private float _spring = 45f;
    [SerializeField] private float _damper = 25f;
    [SerializeField] private float _massScale = 8f;
   

    [Header("Swing Physics")]
    [SerializeField] public static float _maxSwingDistance = 50f;
    [SerializeField] private float _swingPullForce = 80f;
    [SerializeField] private LayerMask _swingLayer;
    [SerializeField] private float timeToAutoRelease = 4f;
    [SerializeField] private float cooldown = 1.5f;

    private Vector3 _swingPoint;
    private SpringJoint _joint;
    private bool _isSwinging;
    private float _originalDrag;

    private float timer;
    
    private float cooldownTimer;
    private bool  isCanSwing = true;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _webLine = GetComponent<LineRenderer>();
        _camera = Camera.main.transform;
        
        _originalDrag = _rb.linearDamping;
        _webLine.enabled = false;
    }

    private void Update()
    {
        HandleInput();
        DrawWeb();
        ReleaseSwingOnTime();
        Cooldown();
    }

    private void Cooldown()
    {
        if (isCanSwing) return;
        
        cooldownTimer += Time.deltaTime;
        
        if (!(cooldownTimer >= cooldown)) return;
        cooldownTimer = 0;
        isCanSwing = true;
    }
    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0)) StartSwing();
        if (Input.GetMouseButtonUp(0)) ReleaseSwing();
    }

    private void ReleaseSwingOnTime()
    {
        if (_isSwinging)
        {
            timer += Time.deltaTime;
            if (timer >= timeToAutoRelease)
            {
                ReleaseSwing();
            }
        }
        else
        {
            timer = 0;
        }
    }
    private void StartSwing()
    {
        if (!isCanSwing) return;
        if (Physics.Raycast(_camera.position, _camera.forward, out RaycastHit hit, _maxSwingDistance, _swingLayer))
        {
            _isSwinging = true;
            isCanSwing = false; 
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
        }
    }

    public void ReleaseSwing()
    {
        _isSwinging = false;
        _webLine.enabled = false;
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