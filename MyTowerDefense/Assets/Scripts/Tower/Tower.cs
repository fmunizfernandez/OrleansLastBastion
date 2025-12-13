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
    private int _upgradeCount = 0;

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
        var range = data.range * (1 + data.GetRangeMultiplier(_upgradeCount));
        var shootInterval = data.shootInterval * (1-data.GetShootIntervalMultiplier(_upgradeCount));

        _circleCollider = GetComponent<CircleCollider2D>();
        _circleCollider.radius = range / transform.localScale.x;

        _enemiesInRange = new List<Enemy>();
        _projectilePool = GetComponent<ObjectPooler>();
        _shootTimer = shootInterval;
    }

    private void Update()
    {
        var shootInterval = data.shootInterval * (1-data.GetShootIntervalMultiplier(_upgradeCount));

        _shootTimer -= Time.deltaTime;
        if (_shootTimer <= 0)
        {
            _shootTimer = shootInterval;
            Shoot();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, data.range);
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
            var damage = data.damage * (1 + data.GetDamageMultiplier(_upgradeCount));

            projectileObj.GetComponent<Projectile>().Shoot(damage, data.projectilSpeed, data.projectilDuration, shootDirection);
        }
    }

    private void Enemy_OnEnemyDestroyed(Enemy enemy)
    {
        _enemiesInRange.Remove(enemy);
    }

    public void SetUpgrade(int upgradeCount)
    {
        _upgradeCount = upgradeCount;
    }
}
