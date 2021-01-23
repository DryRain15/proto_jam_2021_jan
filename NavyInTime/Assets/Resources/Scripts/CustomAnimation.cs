using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomAnimation", menuName = "Custom2D/Create Custom Animation", order = 1)]
public class CustomAnimation : ScriptableObject
{
    public List<Sprite> spriteSequence;
    public int Size => spriteSequence.Count;
    
    public Sprite GetSprite(int frame)
    {
        return spriteSequence[frame];
    }
}
