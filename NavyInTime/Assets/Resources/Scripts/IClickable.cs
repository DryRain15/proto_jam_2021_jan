using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClickable
{
    Collider2D Collider2D
    {
        get;
    }

    void OnLeftClick();
    void OnRightClick();
    void OnMiddleClick();
}
