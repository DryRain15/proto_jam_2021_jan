using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageInfo
{
    public int Damage { get; set; }
    
    public IHit Getter { get; set; }
    
    public IHit Giver { get; set; }
}
public interface IHit
{
    bool Hittable { get; set; }
    
    void GetHit(DamageInfo info);

    void GiveHit(DamageInfo info);
}
