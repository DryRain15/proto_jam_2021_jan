using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static Vector3 ToVector3(Vector2 vec, float z = 0f)
    {
        return new Vector3(vec.x, vec.y, z);
    }

    public static float D2T = 0.015625f;

    public static float AngleValidate(float angle)
    {
        if (angle < 0f)
            return angle + 360f;
        if (angle > 360f)
            return angle - 360f;
            
        return angle;
    }
        
    public static float V2D(Vector2 dir)
    {
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            
        if (angle < 0f)
            return angle + 360f;
        if (angle > 360f)
            return angle - 360f;
            
        return angle;
    }

    public static Vector2 D2V(float angle)
    {
        return new Vector2(-Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    public static Vector2 GetXVector(this Vector2 vec)
    {
        return new Vector2(vec.x, 0f);
    }
    
    public static Vector2 GetYVector(this Vector2 vec)
    {
        return new Vector2(0f, vec.y);
    }

    public static Vector3 LeftScale = new Vector3(-1, 1, 1);
    public static Vector3 RightScale = new Vector3(1, 1, 1);
    public static float PixelSize = 1f / 16f;

    public static int FloorMask(int self)
    {
        switch (self)
        {
            case 16 : return (1 << 8) | (1 << 18);
            case 18 : return (1 << 8);
            case 17 :
            case 8 : return (1 << 8) | (1 << 16) | (1 << 18);
        }

        return 1 << 8;
    }
    public static int TargetMask(int self)
    {
        switch (self)
        {
            case 16 : return (1 << 8) | (1 << 18);
            case 18 : return (1 << 8);
            case 17 :
            case 8 : return (1 << 16) | (1 << 18);
        }

        return 1 << 8;
    }
}
