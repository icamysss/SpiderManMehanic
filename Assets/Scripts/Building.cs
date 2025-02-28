using UnityEngine;
using Random = UnityEngine.Random;

public class Building : MonoBehaviour
{
    public float maxHeight = 20;
    public float minHeight = 5;
    public float speed = 1f;
    public bool isActive = true;
    
    public float blockTimer = 3f;
    private Vector3 _initialScale;
    private Renderer _renderer;

    private bool canConnect;
    private float timer;

    private void Start()
    {
        speed *= Random.Range(0.5f, 1.5f);
        transform.localScale = new Vector3(transform.localScale.x, Random.Range(minHeight, maxHeight), transform.localScale.z);
        _renderer = GetComponentInChildren<Renderer>();
        if (_renderer == null)
        {
            Debug.LogError("Renderer component missing!");
            enabled = false;
            return;
        }
        
        // Создаем уникальную копию материала
        _renderer.material = new Material(_renderer.material);
        _initialScale = transform.localScale;
    }

    private void Update()
    {
        if (!isActive) return;


        if (!canConnect)
        {
            timer += Time.deltaTime;
            if (timer >= blockTimer)
            {
                canConnect = true;
                timer = 0;
            }
        }
 
        float t = Mathf.PingPong(Time.time * speed / 10, 1f);
        
        // Изменение масштаба
        float currentHeight = Mathf.Lerp(minHeight, maxHeight, t);
        transform.localScale = new Vector3(_initialScale.x, currentHeight, _initialScale.z);
        
        // Изменение цвета
        Color targetColor = Color.Lerp(Color.red, Color.green, t);
        _renderer.material.color = targetColor;
    }

    public bool TryConnect()
    {
        if (!canConnect) return false;
        
        canConnect = false;
        timer = 0f;
        return true;
    }
}