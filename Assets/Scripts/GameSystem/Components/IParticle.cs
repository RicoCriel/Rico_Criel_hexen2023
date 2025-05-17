using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IParticle
{
    void SpawnVFX(GameObject prefab = null);
}
