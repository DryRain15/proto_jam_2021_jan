using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCursor : MonoBehaviour
{
    public static TileCursor self;
    
    public TileToken token;
    public SpriteRenderer sr;

    private void Awake()
    {
        self = this;
        token = new TileToken("null");
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // if (token.Name == "null") return;

        var mousePos = Input.mousePosition;
        var wantedPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));
        transform.position = wantedPos;

        var pos = transform.position;
        pos = new Vector2(Mathf.FloorToInt(pos.x) + 0.5f, Mathf.FloorToInt(pos.y) + 0.5f);
        transform.position = new Vector3(pos.x, pos.y, -5);

    }

    public void TokenUpdate()
    {
        sr.sprite = token.Sprite;
    }
}
