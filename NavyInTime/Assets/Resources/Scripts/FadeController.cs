using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    public static FadeController self;
    public RawImage screen;
    
    public float targetAlpha;
    private float currentAlpha;

    private float timer;
    public float duration;
    
    private void Awake()
    {
        self = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        screen.color = Color.black;
        targetAlpha = 0f;
        currentAlpha = 1f;
        timer = 0f;
        duration = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(currentAlpha - targetAlpha) < 0.001f)
        {
            currentAlpha = targetAlpha;
            screen.color = new Color(0, 0, 0, targetAlpha);
            timer = 0f;
            return;
        }

        timer += Time.deltaTime;
        currentAlpha = Mathf.Lerp(currentAlpha, targetAlpha, timer / duration);
        screen.color = new Color(0, 0, 0, currentAlpha);
    }

    public void SetFade(float target, float t)
    {
        duration = t;
        targetAlpha = target;
    }
}
