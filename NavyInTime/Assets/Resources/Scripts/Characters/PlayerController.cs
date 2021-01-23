using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : ReversableObject, IHit
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

    public static PlayerController self; 

    private void Awake()
    {
        self = this;
    }

    // Update is called once per frame
    void Update()
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

        if (Input.GetKeyDown(KeyCode.R))
            GetHit(new DamageInfo());
        
        var hInput = Input.GetAxis("Horizontal");
        if (hInput != 0)
        {
            if (OnFloor)
                Anim.SetAnim("Walk");
            AddVelocity(Vector2.right * (hInput * Time.deltaTime));
        }
        else
        {
            if (OnFloor)
                Anim.SetAnim("Stand");
        }
        if (OnFloor && Input.GetKeyDown(KeyCode.Space))
        {
            Anim.SetAnim("JumpStart");
            OnFloor = false;
            AddVelocity(Vector2.up * jumpPower);
        }

        if (Velocity.y < 0)
        {
            Anim.SetAnim("FallStart");
        }
    }

    public override void ApplyVelocity(TimeStamp stamp)
    {
        base.ApplyVelocity(stamp);
        Anim.sr.sprite = stamp.sprite;
    }

    public override void Forward()
    {
        if (Velocity + OuterVelocity == Vector2.zero)
            return;

        TimeLine.Push(new TimeStamp(this, Anim, Frame));
        Anim.sr.material.SetFloat("_power", 0);
    }

}
