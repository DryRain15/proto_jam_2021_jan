using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestJumper : ReversableObject, IHit
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

    public float freq = 2f;
    private float _timer = 0;

    public override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        JumperUpdate();
    }

    private void JumperUpdate()
    {
        if (Play != MoveMode.Forward) return;
        
        _timer += Time.deltaTime;
        if (_timer > freq)
        {
            if (OnFloor)
            {
                OnFloor = false;
                AddVelocity(Vector2.up * jumpPower);
                _timer = 0f;
            }
        }
    }
}
