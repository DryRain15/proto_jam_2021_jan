using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : ReversableObject, IHit
{
    public bool Hittable { get; set; }
    public virtual void GetHit(DamageInfo info)
    {
        transform.position = new Vector2(9999f, 9999f);
    }

    public virtual void GiveHit(DamageInfo info)
    {
        info.Getter.GetHit(info);
    }

    public bool IsShooter;
    public bool IsJumper;
    public bool IsFollower;
    public bool IsHitter;
    
    private ShootHelper _sh;
    public float freq = 1.5f;
    private float _timer = 0;
    
    public int MeleeDamage;

    public float hspeed;
    
    public override void Start()
    {
        base.Start();
        _sh = GetComponent<ShootHelper>();
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
        
        _timer += Time.deltaTime;
        
        if (IsShooter) ShooterUpdate();
        if (IsFollower) FollowerUpdate();

        if (IsHitter) HitUpdate();
    }

    private void ShooterUpdate()
    {
        if (_timer > freq)
        {
            _sh.Shoot(transform.position + Vector3.up * 0.5f, 
                IsLeft ? Vector2.left : Vector2.right, 5f, 1);
            _timer = 0f;
        }
    }

    private void Jump()
    {
        if (OnFloor)
        {
            OnFloor = false;
            AddVelocity(Vector2.up * jumpPower);
        }
    }

    private void HitUpdate()
    {
        var pos = transform.position;

        var ray = Physics2D.BoxCastAll(pos, ((BoxCollider2D) Collider2D).size + Vector2.one * 0.25f, 
            0, Vector2.zero, 0, (1 << 16));

        if (ray.Length > 0)
        {
            foreach (var hit in ray)
            {
                var iHit = hit.transform.GetComponent<IHit>();
                if (iHit != null)
                {
                    var dmgInfo = new DamageInfo();
                    dmgInfo.Damage = MeleeDamage;
                    dmgInfo.Getter = iHit;
                    dmgInfo.Giver = this;
                    
                    GiveHit(dmgInfo);
                }
            }
        }

    }
    private void FollowerUpdate()
    {
        Anim.SetAnim("walk");
        Anim.ChangeFps(12);

        var pos = transform.position;

        var hray = Physics2D.Raycast(pos + Vector3.up * 0.5f, Velocity.normalized, 2f, 1 << 8);

        if (hray)
            Jump();
        
        var dt = Time.deltaTime;

        IsLeft = LittleGirl.self.Position.x < transform.position.x;
        
        AddVelocity((IsLeft ? Vector2.left : Vector2.right) * (hspeed * dt));
    }
}
