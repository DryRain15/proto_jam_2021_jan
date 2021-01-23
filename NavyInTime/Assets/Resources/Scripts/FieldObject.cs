using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldObject : ReversableObject, IHit
{
    public bool Hittable { get; set; }
    public virtual void GetHit(DamageInfo info)
    {
    }

    public virtual void GiveHit(DamageInfo info)
    {
        info.Getter.GetHit(info);
    }

    private void Update()
    {
        if (Play != MoveMode.Forward)
        {
            return;
        }
        HitUpdate();
    }

    public bool slopeLeft = false;
    public bool slopeRight = false;

    private void HitUpdate()
    {
        var pos = transform.position;

        var ray = Physics2D.BoxCastAll(pos, new Vector2(((BoxCollider2D) Collider2D).size.x * 0.9f, 0.1f), 
            0, Vector2.down, 0.25f, Utils.TargetMask(gameObject.layer));

        if (ray.Length > 0)
        {
            foreach (var hit in ray)
            {
                var iHit = hit.transform.GetComponent<IHit>();
                if (iHit != null)
                {
                    var dmgInfo = new DamageInfo();
                    dmgInfo.Damage = 9999;
                    dmgInfo.Getter = iHit;
                    dmgInfo.Giver = this;
                    
                    GiveHit(dmgInfo);
                }
            }
        }

    }
    
    public override void ApplyVelocity()
    {
        var pos = transform.position;
        var vdRay = Physics2D.BoxCastAll(pos, new Vector2(((BoxCollider2D) Collider2D).size.x * 0.9f, 0.1f), 
            0, Vector2.down, 0.1f, Utils.FloorMask(gameObject.layer));
        var hRay = Physics2D.Raycast(pos + 
                                     Vector3.right * (Mathf.Sign(Velocity.x) * ((BoxCollider2D) Collider2D).size.x) / 2 + 
                                     Vector3.right * (Utils.D2T * 2) +
                                     Vector3.up * 0.5f, 
            Vector2.right, Velocity.x, 1 << 8);
        
        
        var hldRay = Physics2D.Raycast(pos + 
                                     Vector3.left * ((BoxCollider2D) Collider2D).size.x / 2 + 
                                     Vector3.down * (Utils.D2T * 2), 
            Vector2.left, 0.5f, 1 << 8);
        var hrdRay = Physics2D.Raycast(pos + 
                                       Vector3.right * ((BoxCollider2D) Collider2D).size.x / 2 + 
                                       Vector3.down * (Utils.D2T * 2), 
            Vector2.right, 0.5f, 1 << 8);

        var dt = Time.deltaTime;

        if (vdRay.Length > 0)
        {
            if (Velocity.y <= 0f)
            {
                foreach (var hit in vdRay)
                {
                    if (hit.transform != transform && hit.transform.gameObject.layer == 8)
                    {
                        SetVelocity(Velocity.x, 0f);
                        OnFloor = true;
                    }
                }
            }

        }
        else
        {
            OnFloor = false;
        }

        if (OnFloor)
        {
            if (!hldRay)
            {
                slopeLeft = true;
            }
            else
            {
                slopeLeft = false;
            }

            if (!hrdRay)
            {
                slopeRight = true;
            }
            else
            {
                slopeRight = false;
            }
        }

        IsBlocked = false;
        if (hRay)
        {
            SetVelocity(0, Velocity.y);
            IsBlocked = true;
        }

        if (!IsBlocked)
        {
            if (slopeLeft)
                AddVelocity(-1f * dt, -0.5f * dt);
            if (slopeRight)
                AddVelocity(1f * dt, -0.5f * dt);
        }

        SetVelocity(Velocity.x * (1 - friction), Velocity.y);

        if (Mathf.Abs(Velocity.x) < Mathf.Epsilon)
            SetVelocity(0f, Velocity.y);
        if (Mathf.Abs(Velocity.y) < Mathf.Epsilon)
            SetVelocity(Velocity.x, 0f);

        transform.position += Utils.ToVector3(Velocity);
    }

}
