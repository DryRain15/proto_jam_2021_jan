using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFollower : ReversableObject, IHit
{
    public bool Hittable { get; set; }
    public virtual void GetHit(DamageInfo info)
    {
    }

    public virtual void GiveHit(DamageInfo info)
    {
        info.Getter.GetHit(info);
    }

    public int MeleeDamage;

    public float hspeed;

    public override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        FollowerUpdate();
    }

    private void FollowerUpdate()
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

        var ray = Physics2D.BoxCastAll(transform.position, ((BoxCollider2D) Collider2D).size + Vector2.one, 
            0, Vector2.zero, 0);

        if (ray.Length > 0)
        {
            foreach (var hit in ray)
            {
                var iHit = hit.transform.GetComponent<IHit>();
                if (iHit != null && hit.transform.gameObject.layer == 16)
                {
                    var dmgInfo = new DamageInfo();
                    dmgInfo.Damage = MeleeDamage;
                    dmgInfo.Getter = iHit;
                    dmgInfo.Giver = this;
                    
                    GiveHit(dmgInfo);
                }
            }
        }

        var dt = Time.deltaTime;

        IsLeft = LittleGirl.self.Position.x < transform.position.x;
        
        AddVelocity((IsLeft ? Vector2.left : Vector2.right) * (hspeed * dt));
    }
}

