using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleGirl : ReversableObject, IHit
{
    public bool Hittable { get; set; }
    public virtual void GetHit(DamageInfo info)
    {
        SoundController.self.PlayRewindSFX();
        ReversablePoolController.self.RewindAll(3);
    }

    public virtual void GiveHit(DamageInfo info)
    {
        info.Getter.GetHit(info);
    }

    public static LittleGirl self;

    public float hspeed;

    public void Awake()
    {
        self = this;
    }

    public override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if (Play != MoveMode.Forward)
        {
            Anim.ChangeFps(0);
            return;
        }
        else
        {
            Anim.ChangeFps(12);
        }

        var dt = Time.deltaTime;

        var hray = Physics2D.Raycast(transform.position + Vector3.up * 0.2f, Velocity.normalized, 0.5f, 1 << 8);

        if (hray)
            Jump();
        
        if (Velocity.x == 0)
        {
            IsLeft = !IsLeft;
            SetVelocity(Vector2.zero);
        }
        
        AddVelocity((IsLeft ? Vector2.left : Vector2.right) * (hspeed * dt));
    }
    
    public override void ApplyVelocity(TimeStamp stamp)
    {
        base.ApplyVelocity(stamp);
        Anim.sr.sprite = stamp.sprite;
    }

    private void Jump()
    {
        if (OnFloor)
        {
            OnFloor = false;
            AddVelocity(Vector2.up * jumpPower);
        }
    }
}
