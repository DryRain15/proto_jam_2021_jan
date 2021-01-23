using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnimationDictionary : SerializableDictionary<string, CustomAnimation>{}

[Serializable]
public class AutoTransitionDictionary : SerializableDictionary<CustomAnimation, CustomAnimation>{}

public class CustomAnimator : MonoBehaviour
{
    public SpriteRenderer sr;
    public CustomAnimation clip;

    public string animName;
    public AnimationDictionary animMap;
    public AutoTransitionDictionary transitionMap;

    public int frame;
    public int fps;

    private float _innerTimer;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (fps == 0)
        {
            return;
        }
        
        if (_innerTimer > (1f / fps))
        {
            if (fps > 0)
            {
                frame++;
                if (frame > clip.Size - 1)
                {
                    frame = 0;
                    if (transitionMap.ContainsKey(clip))
                        clip = transitionMap[clip];
                }
                sr.sprite = clip.GetSprite(frame);
            }
            else
            {
                frame--;
                if (frame < 0)
                    frame = clip.Size - 1;
                sr.sprite = clip.GetSprite(frame);
            }

            _innerTimer = 0f;
        }
        
        _innerTimer += Time.deltaTime;
    }

    public void ChangeFps(int value)
    {
        fps = value;
        
        if (fps == 0)
        {
            _innerTimer = 0f;
            sr.color = new Color(1, 1, 1, 0.5f);
        }
        sr.color = new Color(1, 1, 1, 1);
    }

    public void SetAnim(string anim)
    {
        if (anim == animName)
            return;
        if (animMap.ContainsKey(anim))
        {
            animName = anim;
            clip = animMap[anim];
        }
    }
}