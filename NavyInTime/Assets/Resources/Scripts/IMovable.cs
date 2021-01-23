using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveMode
{
    Forward,
    Backward,
    Stop,
}
public interface IMovable
{
    Vector2 Position
    {
        get;
    }

    Quaternion Rotation
    {
        get;
    }
    
    Vector2 Velocity
    {
        get;
    }

    bool IsLeft
    {
        get;
        set;
    }

    void AddVelocity(Vector2 vel, bool isOuter = false);
    
    void SetVelocity(Vector2 vel, bool isOuter = false);
    
    void ApplyVelocity();

    void ApplyVelocity(TimeStamp stamp);

    void Stop();
    

}
