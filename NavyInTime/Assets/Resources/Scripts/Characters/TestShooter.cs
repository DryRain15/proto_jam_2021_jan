using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShooter : ReversableObject, IHit
{
    public bool Hittable { get; set; }
    public virtual void GetHit(DamageInfo info)
    {
        ReversablePoolController.self.RewindAll();
    }

    public virtual void GiveHit(DamageInfo info)
    {
        info.Getter.GetHit(info);
    }

    private ShootHelper _sh;
    public float freq = 1.5f;
    private float _timer = 0;

    public override void Start()
    {
        base.Start();
        _sh = GetComponent<ShootHelper>();
    }

    private void Update()
    {
        if (Play != MoveMode.Forward) return;
        
        _timer += Time.deltaTime;
        if (_timer > freq)
        {
            _sh.Shoot(transform.position + Vector3.up * 0.5f, 
                IsLeft ? Vector2.left : Vector2.right, 5f, 1);
            _timer = 0f;
        }
    }
}
