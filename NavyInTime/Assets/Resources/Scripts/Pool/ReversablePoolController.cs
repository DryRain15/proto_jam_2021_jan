using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using UnityEngine;

public class ReversablePoolController : MonoBehaviour
{
    public static ReversablePoolController self;

    public static MoveMode globalMode;

    private Dictionary<GameObject, IReversable> reversableCache = new Dictionary<GameObject, IReversable>();

    private int OnRewindAll = -1;

    private float _timer;
    
    private void Awake()
    {
        self = this;
        _timer = 0f;
    }

    private void Update()
    {
        if (OnRewindAll > 0)
        {
            RewindAll(OnRewindAll);
        }
    }

    public void Register(GameObject obj, IReversable target)
    {
        reversableCache.Add(obj, target);
    }
    
    public void Unregister(GameObject obj)
    {
        if (reversableCache.ContainsKey(obj))
            reversableCache.Remove(obj);
    }

    public void Rewind(GameObject obj, int speed = 1)
    {
        OnRewindAll = -1;
        if (!reversableCache.ContainsKey(obj)) return;
        
        reversableCache[obj].Play = MoveMode.Stop;
        for (int i = 0; i < speed; i++)
        {
            reversableCache[obj].Backward();
        }
    }

    public void Stop(GameObject obj)
    {
        OnRewindAll = -1;
        if (!reversableCache.ContainsKey(obj)) return;
        
        reversableCache[obj].Play = MoveMode.Stop;
    }

    public void Play(GameObject obj)
    {
        OnRewindAll = -1;
        if (!reversableCache.ContainsKey(obj)) return;
        
        reversableCache[obj].Play = MoveMode.Forward;
        reversableCache[obj].Forward();
    }

    public void RewindAll(int speed = 1)
    {
        _timer += Time.deltaTime;
        FadeController.self.SetFade(0.2f, 0f);
        globalMode = MoveMode.Backward;
        OnRewindAll = speed;
        var isRemain = false;
        foreach (var obj in reversableCache)
        {
            obj.Value.Play = MoveMode.Stop;
            for (int i = 0; i < speed; i++)
            {
                isRemain = obj.Value.Backward() || isRemain;
            }

        }
        
        if (_timer > 1f)
        {
            _timer = 0f;
            ResetAll();
            PlayAll();
            return;
        }

        if (!isRemain)
            PlayAll();
    }

    public void PlayAll()
    {
        FadeController.self.SetFade(0f, 0f);
        globalMode = MoveMode.Forward;
        OnRewindAll = -1;
        foreach (var obj in reversableCache)
        {
            obj.Value.Play = MoveMode.Forward;
            obj.Value.Forward();
        }
    }

    public void StopAll()
    {
        globalMode = MoveMode.Stop;
        OnRewindAll = -1;
        foreach (var obj in reversableCache)
        {
            obj.Value.Play = MoveMode.Stop;
        }
    }

    public void ResetAll()
    {
        OnRewindAll = -1;
        foreach (var obj in reversableCache)
        {
            if (obj.Value.TimeLine.Count > 1)
            {
                var stamp = obj.Value.TimeLine.Last();
                obj.Value.TimeLine.Clear();
                obj.Value.TimeLine.Push(stamp);
                obj.Value.Backward();
            }
            obj.Value.Play = MoveMode.Forward;
        }
    }
}
