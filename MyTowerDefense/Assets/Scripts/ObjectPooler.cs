using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int poolSize = 5;
    private List<GameObject> _pool;


    void Start()
    {
        _pool = new List<GameObject>();

        for (var i = 0; i < poolSize; i++)
        {
            CreateNewObject();
        }
    }

    private GameObject CreateNewObject()
    {
        var obj = Instantiate(prefab, transform);
        obj.SetActive(false);
        _pool.Add(obj);
        return obj;
    }

    public GameObject GetPooledObject() 
    {
        var obj=_pool.FirstOrDefault(f => !f.activeSelf);
        if (obj == null)
            return CreateNewObject();

        return obj;
    }
}
