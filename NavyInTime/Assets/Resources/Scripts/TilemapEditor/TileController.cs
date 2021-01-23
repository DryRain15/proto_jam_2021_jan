using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public void OnTileUpdate()
    {
        
    }
}

public class TileMapGrid
{
    public GameObject prefab;
    public Dictionary<Vector2, TileToken> tiles = new Dictionary<Vector2, TileToken>();
    
    
}

public class DataContainer
{
    public string name;
    public string type;
    
}

public class TileBuilder
{
    public int Id;
    public Sprite Sprite;
    public string Name;

    public TileToken GetToken()
    {
        return new TileToken(this);
    }
}

public struct TileToken
{
    public int Id;
    public Sprite Sprite;
    public string Name;
    public int x;
    public int y;

    public TileToken(string name)
    {
        Id = -1;
        Sprite = null;
        Name = name;
        x = 0;
        y = 0;
    }
    public TileToken(TileBuilder builder)
    {
        Id = builder.Id;
        Sprite = builder.Sprite;
        Name = builder.Name;
        x = 0;
        y = 0;
    }

    public void SetPosition(Vector2Int v2)
    {
        x = v2.x;
        y = v2.y;
    }
}