using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ShootHelper : MonoBehaviour
{
    public string bulletName;

    public void Shoot(Vector2 pos, Vector2 dir, float speed, int damage)
    {
        var proj = ObjectPoolController.Self.Instantiate(bulletName, 
            new PoolParameters(pos, dir));
        proj.gameObject.GetComponent<Projectile>().Initialize(dir,speed,damage);
    }
}
