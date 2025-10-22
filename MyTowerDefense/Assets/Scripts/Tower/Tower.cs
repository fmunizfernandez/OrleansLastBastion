using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField]
    private TowerData data;

    private List<Enemy> _enemiesInRange;
    private CircleCollider2D _circleCollider;
    private ObjectPooler _projectilePool;

    private float _shootTimer;

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
        _circleCollider = GetComponent<CircleCollider2D>();
        _circleCollider.radius = data.range/transform.localScale.x;

        _enemiesInRange = new List<Enemy>();
        _projectilePool= GetComponent<ObjectPooler>();
        _shootTimer = data.shootInterval;
    }

    private void Update() 
    {
        _shootTimer -= Time.deltaTime;
        if (_shootTimer <= 0) 
        {
            _shootTimer = data.shootInterval;
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
            projectileObj.transform.position=transform.position;
            projectileObj.SetActive(true);

            var shootDirection = (_enemiesInRange.First().transform.position - transform.position).normalized;

            projectileObj.GetComponent<Projectile>().Shoot(data,shootDirection);
        }
    }

    private void Enemy_OnEnemyDestroyed(Enemy enemy)
    {
        _enemiesInRange.Remove(enemy);
    }
}
