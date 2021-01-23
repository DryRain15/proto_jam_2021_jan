using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MouseClickType
{
    Left,
    Right,
    Middle,
}
public class MouseCursor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            OnClick(MouseClickType.Left);
        if (Input.GetMouseButtonDown(1))
            OnClick(MouseClickType.Right);
        if (Input.GetMouseButtonDown(2))
            OnClick(MouseClickType.Middle);
    }

    void OnClick(MouseClickType type)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        
        if (hit)
        {
            var click = hit.transform.GetComponent<IClickable>();
            switch (type)
            {
                case MouseClickType.Left:
                    click?.OnLeftClick();
                    break;
                case MouseClickType.Right:
                    click?.OnRightClick();
                    break;
                case MouseClickType.Middle:
                    click?.OnMiddleClick();
                    break;
            }
        }
    }
}
