using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData data;
    public EnemyData Data => data;

    private Path _currentPath;
    private Vector3 _targetPosition;
    private int _currentWaypoint;
    private float _lives;
    private float _maxLivePerWave;

    private float _speed;
    private float _damage;

    [SerializeField]
    private Transform healthBar;
    private Vector3 _healthBarOriginalScale;

    public static event Action<int> OnEnemyReachEnd;
    public static event Action<Enemy> OnEnemyDestroyed;

    private void Awake()
    {
        _currentPath = GameObject.Find("Path").GetComponent<Path>();
        _healthBarOriginalScale = healthBar.localScale;
    }

    private void OnEnable()
    {
        _currentWaypoint = 0;

        transform.position = _currentPath.GetPosition(_currentWaypoint);
        _targetPosition = _currentPath.GetPosition(_currentWaypoint);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);

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
                OnEnemyReachEnd?.Invoke(Mathf.CeilToInt(_damage));
                gameObject.SetActive(false);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        _lives = Mathf.Max(0, _lives - damage);
        if (_lives <= 0)
        {
            OnEnemyDestroyed?.Invoke(this);
            gameObject.SetActive(false);
            return;
        }

        UpdateHealthBarScale();
    }

    private void UpdateHealthBarScale()
    {
        var floatPercent = _lives / _maxLivePerWave;
        var scale = _healthBarOriginalScale;
        scale.x = _healthBarOriginalScale.x * floatPercent;
        healthBar.localScale = scale;
    }

    public void Initialize(int levelFactor, int waveFactor)
    {
        var healthIncreasePercent = data.GetResistanceMultiplier(levelFactor, waveFactor);
        var speedIncreasePercent = data.GetSpeedMultiplier(levelFactor);
        var damageIncreasePercent = data.GetDamageMultiplier(levelFactor);

        _speed = data.Speed * speedIncreasePercent;
        _damage = data.Damage * damageIncreasePercent;
        _lives = data.Live * healthIncreasePercent;

        Debug.LogWarning($"Basic Values Enemy: Speed: {data.Speed}, Damage: {data.Damage}, Lives: {data.Live}");
        Debug.LogWarning($"New Tower: Speed: {_speed}, Damage: {_damage}, Lives: {_lives}");

        _maxLivePerWave = _lives;
        
        UpdateHealthBarScale();
    }
}
