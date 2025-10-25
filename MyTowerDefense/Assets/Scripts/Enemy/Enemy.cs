using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData data;
    private Path _currentPath;
    private Vector3 _targetPosition;
    private int _currentWaypoint;
    private float _lives;

    [SerializeField]
    private Transform healthBar;
    private Vector3 _healthBarOriginalScale;

    public static event Action<EnemyData> OnEnemyReachEnd;
    public static event Action<Enemy> OnEnemyDestroyed;

    private void Awake()
    {
        _currentPath = GameObject.Find("Path").GetComponent<Path>();
        _healthBarOriginalScale = healthBar.localScale;
    }

    private void OnEnable()
    {
        _lives = data.Live;
        _currentWaypoint = 0;
        
        transform.position = _currentPath.GetPosition(_currentWaypoint);
        _targetPosition = _currentPath.GetPosition(_currentWaypoint);
        UpdateHealthBarScale();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, data.Speed * Time.deltaTime);

        var relativeDistance = (transform.position - _targetPosition).magnitude;
        if (relativeDistance < 0.1f)
        {
            if (_currentWaypoint < _currentPath.Waypoints.Length - 1)
            {
                _currentWaypoint++;
                _targetPosition = _currentPath.GetPosition(_currentWaypoint);
            }
            else
            {
                OnEnemyReachEnd?.Invoke(data);
                gameObject.SetActive(false);
            }
        }
    }

    public void TakeDamage(float damage) 
    {
        _lives = Mathf.Max(0,_lives-damage);
        UpdateHealthBarScale();

        if (_lives <= 0) 
        {
            OnEnemyDestroyed?.Invoke(this);
            gameObject.SetActive(false);
        }
    }

    private void UpdateHealthBarScale() 
    {
        var floatPercent = _lives / data.Live;
        var scale = _healthBarOriginalScale;
        scale.x = _healthBarOriginalScale.x * floatPercent;
        healthBar.localScale= scale;
    }

    public EnemyData GetEnemyData() 
    {
        return data;
    }
}
