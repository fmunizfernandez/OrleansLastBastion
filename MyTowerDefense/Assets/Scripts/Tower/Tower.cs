using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField]
    private TowerData data;

    private List<Enemy> _enemiesInRange;
    private CircleCollider2D _circleCollider;
    private ObjectPooler _projectilePool;

    private float _shootTimer;
    private float _damage;
    private float _range;
    private float _shootInterval;

    private void OnEnable()
    {
        Enemy.OnEnemyDestroyed += Enemy_OnEnemyDestroyed;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDestroyed -= Enemy_OnEnemyDestroyed;
    }

    private void Start()
    {
        _enemiesInRange = new List<Enemy>();
        _projectilePool = GetComponent<ObjectPooler>();
        _shootTimer = _shootInterval;
    }

    private void Update()
    {
        _shootTimer -= Time.deltaTime;
        if (_shootTimer <= 0)
        {
            _shootTimer = _shootInterval;
            Shoot();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _range);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var enemy = collision.GetComponent<Enemy>();
            _enemiesInRange.Add(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var enemy = collision.GetComponent<Enemy>();
            if (_enemiesInRange.Contains(enemy))
            {
                _enemiesInRange.Remove(enemy);
            }
        }
    }

    private void Shoot()
    {
        if (_enemiesInRange.Any())
        {
            var projectileObj = _projectilePool.GetPooledObject();
            projectileObj.transform.position = transform.position;
            projectileObj.SetActive(true);

            var shootDirection = (_enemiesInRange.First().transform.position - transform.position).normalized;
            projectileObj.GetComponent<Projectile>().Shoot(_damage, data.projectilSpeed, data.projectilDuration, shootDirection);
        }
    }

    private void Enemy_OnEnemyDestroyed(Enemy enemy)
    {
        _enemiesInRange.Remove(enemy);
    }

    public void Initialize(int levelFactor, int upgradeCount)
    {
        var damageIncreasePercent = data.GetDamageMultiplier(levelFactor, upgradeCount);
        var rangeIncreasePercent = data.GetRangeMultiplier(levelFactor, upgradeCount);
        var shootIntervalIncreasePercent = data.GetShootIntervalMultiplier(levelFactor, upgradeCount);

        _range = data.range * rangeIncreasePercent;
        _damage = data.damage * damageIncreasePercent;
        _shootInterval = data.shootInterval * shootIntervalIncreasePercent;

        Debug.LogWarning($"Basic Values Tower: Damage: {data.damage}, range: {data.range}, Shoot Interval: {data.shootInterval}");
        Debug.LogWarning($"New Tower: Damage: {_damage}, range: {_range}, Shoot Interval: {_shootInterval}");

        _circleCollider = GetComponent<CircleCollider2D>();
        _circleCollider.radius = _range / transform.localScale.x;
    }
}
