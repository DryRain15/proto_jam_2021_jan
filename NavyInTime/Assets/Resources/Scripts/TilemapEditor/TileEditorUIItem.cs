using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileEditorUIItem : MonoBehaviour
{
    public Button button;
    public Image img;
    public Text text;
    public TileToken token;

    private void Awake()
    {
    }

    public void OnClick()
    {
        TileCursor.self.token = token;
        TileCursor.self.TokenUpdate();
    }
}
