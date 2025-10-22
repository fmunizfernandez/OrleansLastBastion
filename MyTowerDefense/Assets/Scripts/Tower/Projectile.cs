using UnityEngine;
using UnityEngine.UIElements;

public class Projectile : MonoBehaviour
{
    private TowerData _data;
    private Vector3 _shootDirection;
    private float _projectilDuration;

    // Update is called once per frame
    void Update()
    {
        if (_projectilDuration <= 0)
            gameObject.SetActive(false);
        else 
        {
            _projectilDuration-= Time.deltaTime;
            transform.position += new Vector3(_shootDirection.x, _shootDirection.y) * _data.projectilSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) 
        {
            var enemy=collision.GetComponent<Enemy>();
            enemy.TakeDamage(_data.damage);
            gameObject.SetActive(false);
        }
    }

    public void Shoot(TowerData data, Vector3 direction) 
    {
        _data = data;
        _shootDirection = direction;
        _projectilDuration = data.projectilDuration;
    }
}
