using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReversableObject : MonoBehaviour, IMovable, IReversable, IClickable
{
    private CustomAnimator _anim;
    public CustomAnimator Anim => _anim;

    private BoxCollider2D _collider2D;
    public Collider2D Collider2D => _collider2D;

    private long _frame;
    public long Frame => _frame;
    public float gravity;
    public float jumpPower;
    public float friction;
    public float speedLimit;
    public Vector2 Position => transform.position;

    public Quaternion Rotation => transform.rotation;

    public bool OnFloor { get; set; }

    private Vector2 _outerVelocity;
    public Vector2 OuterVelocity => _outerVelocity;
    
    [SerializeField]
    private Vector2 _velocity;
    public Vector2 Velocity => _velocity;

    public bool IsLeft { get; set; }

    public bool IsBlocked;

    public Stack<TimeStamp> TimeLine { get; set; }

    public MoveMode Play { get; set; }

    // Start is called before the first frame update
    public virtual void Start()
    {
        Register();
        Play = MoveMode.Forward;
        TimeLine = new Stack<TimeStamp>();
        _collider2D = GetComponent<BoxCollider2D>();
        _anim = GetComponentInChildren<CustomAnimator>();
        Play = MoveMode.Forward;
    }

    public void Register()
    {
        ReversablePoolController.self.Register(gameObject, this);
    }
    public void Unregister()
    {
        ReversablePoolController.self.Unregister(gameObject);
    }

    private void LateUpdate()
    {
        ApplyPhysics();
    }

    private void ApplyPhysics()
    {
        if (Play == MoveMode.Forward)
        {
            AddVelocity(Vector2.down * (gravity * Time.deltaTime));
            if (_outerVelocity.y > 0f) AddVelocity(Vector2.down * (gravity * Time.deltaTime), true);
            ClampVelocity();
            ApplyOuterVelocity();
            ApplyVelocity();
            ApplyDirection();
            Forward();
        }
        else if (Play == MoveMode.Backward)
        {
            Backward();
            ApplyBackwardVelocity();
        }
        else
        {
            Stop();
        }
    }

    public void ClampVelocity()
    {
        if (_velocity.magnitude > speedLimit)
            _velocity = _velocity.normalized * speedLimit;
    }

    public void SetVelocity(Vector2 vel, bool isOuter = false)
    {
        if (isOuter)
            _outerVelocity = vel;
        else
            _velocity = vel;
    }

    public void SetVelocity(float x, float y, bool isOuter = false)
    {
        SetVelocity(new Vector2(x, y), isOuter);
    }

    public void AddVelocity(Vector2 vel, bool isOuter = false)
    {
        if (isOuter)
            _outerVelocity += vel;
        else
            _velocity += vel;
    }
    
    public void AddVelocity(float x, float y, bool isOuter = false)
    {
        AddVelocity(new Vector2(x, y), isOuter);
    }

    public virtual void ApplyBackwardVelocity()
    {
        var pos = transform.position;

        var ray = Physics2D.BoxCastAll(pos, ((BoxCollider2D) Collider2D).size + Vector2.one * (Utils.D2T * 3f), 
            0, Vector2.up, ((BoxCollider2D) Collider2D).size.y * 0.5f, Utils.FloorMask(gameObject.layer));

        if (ray.Length > 0)
        {
            foreach (var hit in ray)
            {
                var iMov = hit.transform.GetComponent<IMovable>();
                if (iMov != null && iMov != this)
                {
                    print(iMov);
                    iMov.SetVelocity(_velocity * -1.05f, true);
                }
            }
        }

    }
    
    public virtual void ApplyVelocity()
    {
        var pos = transform.position;
        var vuRay = Physics2D.Raycast(pos + Vector3.up * _collider2D.size.y, Vector2.up, _velocity.magnitude, 1 << 8);
        var vdRay = Physics2D.Raycast(pos, Vector2.down, Mathf.Max(Utils.D2T, -_velocity.y), Utils.FloorMask(gameObject.layer));
        var hRay = Physics2D.Raycast(pos + 
                                     Vector3.right * (Mathf.Sign(_velocity.x) * _collider2D.size.x) / 2 + 
                                     Vector3.up * 0.5f, 
            Vector2.right, _velocity.x, 1 << 8);
        var hdRay = Physics2D.Raycast(pos + 
                                     Vector3.right * (Mathf.Sign(_velocity.x) * _collider2D.size.x) / 2 + 
                                     Vector3.up * 0.1f, 
            Vector2.right, _velocity.x, 1 << 8);

        if (vdRay && _velocity.y <= 0f)
        {
            _velocity.y = 0;
            transform.position = vdRay.point;
            OnFloor = true;
        }
        else
        {
            OnFloor = false;
        }
        if (vuRay && _velocity.y > 0f)
        {
            _velocity.y = 0;
        }

        if (hdRay)
        {
            IsBlocked = false;
            if (hRay)
            {
                _velocity.x = hRay.distance;
                IsBlocked = true;
            }
            else
            {
                var vy = 0f;
                if (_velocity.y >= jumpPower * 0.8f) vy = _velocity.y;
                SetVelocity(new Vector2(_velocity.x, Mathf.Abs(_velocity.x) * 0.45f));
                AddVelocity(Vector2.up * vy);
            }
        }

        _velocity.x *= 1 - friction;

        if (Mathf.Abs(_velocity.x) < Mathf.Epsilon)
            _velocity.x = 0f;
        if (Mathf.Abs(_velocity.y) < Mathf.Epsilon)
            _velocity.y = 0f;

        if (_velocity.x < 0f)
            IsLeft = true;
        else if (_velocity.x > 0f)
            IsLeft = false;

        transform.position += Utils.ToVector3(_velocity);
    }

    public virtual void ApplyOuterVelocity()
    {
        var pos = transform.position;
        var vuRay = Physics2D.Raycast(pos + Vector3.up * _collider2D.size.y, Vector2.up, _outerVelocity.magnitude, 1 << 8);
        var vdRay = Physics2D.Raycast(pos, Vector2.down, Mathf.Max(Utils.D2T, -_outerVelocity.y), Utils.FloorMask(gameObject.layer));
        var hRay = Physics2D.Raycast(pos + 
                                     Vector3.right * (Mathf.Sign(_outerVelocity.x) * _collider2D.size.x) / 2 + 
                                     Vector3.up * 0.5f, 
            Vector2.right, _outerVelocity.x, 1 << 8);
        var hdRay = Physics2D.Raycast(pos + 
                                     Vector3.right * (Mathf.Sign(_outerVelocity.x) * _collider2D.size.x) / 2 + 
                                     Vector3.up * 0.1f, 
            Vector2.right, _outerVelocity.x, 1 << 8);

        if (vdRay && _outerVelocity.y < 0f)
        {
            _outerVelocity.y = 0;
            transform.position = vdRay.point;
            OnFloor = true;
        }
        else
        {
            OnFloor = false;
        }
        
        if (hdRay)
        {
            IsBlocked = false;
            if (hRay)
            {
                _outerVelocity.x = hRay.distance;
                IsBlocked = true;
            }
        }

        _outerVelocity.x *= 1 - friction;

        if (Mathf.Abs(_outerVelocity.x) < Mathf.Epsilon)
            _outerVelocity.x = 0f;
        if (Mathf.Abs(_outerVelocity.y) < Mathf.Epsilon)
            _outerVelocity.y = 0f;

        transform.position += Utils.ToVector3(_outerVelocity);
    }

    public virtual void ApplyDirection()
    {
        if (IsLeft)
            transform.GetChild(0).localScale = Utils.LeftScale;
        else
            transform.GetChild(0).localScale = Utils.RightScale;
    }

    public virtual void ApplyVelocity(TimeStamp stamp)
    {
        _frame = stamp.Frame;
        transform.position = stamp.Position;
        transform.rotation = stamp.Rotation;
        _velocity = stamp.Velocity;
        IsLeft = stamp.isLeft;
    }

    public void Stop()
    {
        _anim?.sr?.material.SetFloat("_power", 0.5f);
    }

    public virtual bool Backward()
    {
        if (TimeLine.Count < 1)
        {
            _outerVelocity = Vector2.zero;
            return false;
        }
        ApplyVelocity(TimeLine.Pop());
        ApplyDirection();
        _anim?.sr?.material.SetFloat("_power", 1);
        return true;
    }

    public virtual void Forward()
    {
        if (_velocity + _outerVelocity == Vector2.zero)
            return;

        TimeLine.Push(new TimeStamp(this, Anim, Frame));
        
        _anim?.sr.material.SetFloat("_power", 0);
    }

    public void SetPlayMode(MoveMode mode)
    {
        Play = mode;
    }
    
    public void OnLeftClick()
    {
        if (Play != MoveMode.Forward)
            Play = MoveMode.Forward;
        else
        {
            Play = MoveMode.Stop;   
        }
    }

    public void OnRightClick()
    {
        if (Play != MoveMode.Backward)
            Play = MoveMode.Backward;
        else
        {
            Play = MoveMode.Stop;   
        }
    }

    public void OnMiddleClick()
    {
        Play = MoveMode.Stop;
    }
}
