using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal class VfxManager: MonoBehaviour
{
    private List<GameObject> _instantiatedEffects = new List<GameObject>();
    public GameObject SpawnEffect(GameObject prefab, Vector3 position)
    => SpawnEffect(prefab, position, Quaternion.identity);

    private void OnEnable()
    {
        _instantiatedEffects.Clear();
    }

    private void OnDisable()
    {
        _instantiatedEffects.Clear();
    }

    private GameObject SpawnEffect(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject instance = Instantiate(prefab, position, rotation);
        _instantiatedEffects.Add(instance);
        return instance;
    }

    public void Clear()
    {
        foreach(var vfx in _instantiatedEffects)
        {
            if (vfx != null)
                Destroy(vfx);
        }
        _instantiatedEffects.Clear();
    }
}
