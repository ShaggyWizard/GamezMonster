using System;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour, ITrajectory, IPlayer, IDeath
{
    public Vector3 Position => transform.position;
    public Quaternion Rotation => transform.rotation;
    public Vector3 Velocity => (_up ? Vector2.up : Vector2.down) * _speed;

    public event Action OnDeath;

    private float _speed;
    private bool _up = false;
    private Rigidbody2D _rigidbody;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        _up = Input.GetAxisRaw("Vertical") > 0f;
        _rigidbody.velocity = Velocity;
    }

    public void OnHit()
    {
        OnDeath?.Invoke();
    }
    public void SetDirection(Vector3 newDirection)
    {
        throw new System.NotImplementedException();
    }
    public void SetPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }
    public void SetRotation(Quaternion newRotation)
    {
        throw new System.NotImplementedException();
    }
    public void SetSpeed(float newSpeed)
    {
        _speed = newSpeed;
    }
}
