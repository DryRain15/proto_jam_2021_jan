using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Projectile : MonoBehaviour, IPooledObject, IReversable, IMovable, IHit, IClickable
{
    private CustomAnimator _anim;
    public CustomAnimator Anim => _anim;

    private BoxCollider2D _collider2D;
    public Collider2D Collider2D => _collider2D;

    public int Damage { get; set; }

    public string Name { get; set; }

    public GameObject GameObject => this.gameObject;

    public Stack<TimeStamp> TimeLine { get; set; }

    public MoveMode Play { get; set; }

    public string wallFxName;
    public string hitFxName;

    private long _frame;
    public long Frame => _frame;

    public float gravity;
    public float friction;
    public float speedLimit;
    public Vector2 Position => transform.position;

    public Quaternion Rotation => transform.rotation;

    private Vector2 _outerVelocity;
    public Vector2 OuterVelocity => _outerVelocity;

    [SerializeField] private Vector2 _velocity;
    public Vector2 Velocity => _velocity;

    public bool IsLeft { get; set; }

    private bool _dispose = false;

    public bool Hittable { get; set; }

    private void Start()
    {
        TimeLine = new Stack<TimeStamp>();
        _collider2D = GetComponent<BoxCollider2D>();
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

    public virtual void GetHit(DamageInfo info)
    {
        DisposeCall();
    }

    public virtual void GiveHit(DamageInfo info)
    {
        info.Getter.GetHit(info);
    }

    private void FixedUpdate()
    {
        ApplyPhysics();
    }

    private void LateUpdate()
    {
        if (_dispose)
            Dispose();
    }

    private void ApplyPhysics()
    {
        if (Play == MoveMode.Forward)
        {
            AddVelocity(Vector2.down * (gravity * Time.deltaTime));
            ClampVelocity();
            ApplyVelocity();
            ApplyDirection();
            Forward();
        }
        else if (Play == MoveMode.Backward)
        {
            Backward();
            ApplyDirection();
        }
    }

    public void ClampVelocity()
    {
        if (_velocity.magnitude > speedLimit)
            _velocity = _velocity.normalized * speedLimit;
    }

    public void SetVelocity(Vector2 vel, bool isOuter = false)
    {
        _velocity = vel;
    }

    public void AddVelocity(Vector2 vel, bool isOuter = false)
    {
        _velocity += vel;
    }

    public void ApplyVelocity()
    {
        var dt = Time.deltaTime;
        var pos = transform.position;
        var ray = Physics2D.BoxCastAll(pos, new Vector2(_velocity.magnitude * dt, _collider2D.size.y),
            Utils.V2D(_velocity.normalized), _velocity.normalized, _velocity.magnitude / 2f * dt);

        if (ray.Length > 0)
        {
            var isEnd = false;
            foreach (var hit in ray)
            {
                var iHit = hit.transform.GetComponent<IHit>();
                if (iHit != null && hit.transform.gameObject.layer == 16)
                {
                    var dmgInfo = new DamageInfo();
                    dmgInfo.Damage = Damage;
                    dmgInfo.Getter = iHit;
                    dmgInfo.Giver = this;

                    GiveHit(dmgInfo);
                    isEnd = true;
                }

                if (hit.transform.gameObject.layer == 8)
                    isEnd = true;
            }

            if (isEnd)
            {
                DisposeCall();
                return;
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

        transform.position += Utils.ToVector3(_velocity * Time.deltaTime);
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
        _velocity = Vector2.zero;
    }

    public virtual bool Backward()
    {
        if (TimeLine.Count < 1)
        {
            DisposeCall();
            return false;
        }

        ApplyVelocity(TimeLine.Pop());
        return true;
    }

    public virtual void Forward()
    {
        if (_velocity + _outerVelocity == Vector2.zero)
            return;

        TimeLine.Push(new TimeStamp(this, Frame));
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

    public void DisposeCall()
    {
        _dispose = true;
    }

    public void Dispose()
    {
        _dispose = false;
        Unregister();
        ObjectPoolController.Self.Dispose(this);
    }

    public void Initialize(Vector2 dir, float speed, int damage)
    {
        Register();
        Play = MoveMode.Forward;
        _velocity = dir.normalized * speed;
        Damage = damage;
    }
}