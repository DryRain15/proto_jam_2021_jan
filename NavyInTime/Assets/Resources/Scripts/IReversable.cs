using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TimeStamp
{
    public long Frame;
    public Vector2 Position;
    public Quaternion Rotation;
    public Vector2 Velocity;
    public Sprite sprite;
    public bool isLeft;

    public TimeStamp(IMovable mov, long t)
    {
        Frame = t;
        Position = mov.Position;
        Rotation = mov.Rotation;
        Velocity = mov.Velocity;
        sprite = null;
        isLeft = mov.IsLeft;
    }

    public TimeStamp(IMovable mov, CustomAnimator anim, long t)
    {
        Frame = t;
        Position = mov.Position;
        Rotation = mov.Rotation;
        Velocity = mov.Velocity;
        sprite = anim ? anim.sr.sprite : null;
        isLeft = mov.IsLeft;
    }
}

public interface IReversable
{
    Stack<TimeStamp> TimeLine
    {
        get;
    }

    MoveMode Play
    {
        get;
        set;
    }

    void Register();
    void Unregister();
    bool Backward();
    void Forward();
}
