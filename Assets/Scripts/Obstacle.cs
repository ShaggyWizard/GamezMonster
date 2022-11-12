using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Obstacle : MonoBehaviour, IPooledObject, ITrajectory, ISize
{
    public Vector3 Position => transform.position;
    public Quaternion Rotation => transform.rotation;
    public Vector3 Velocity => _direction * _speed;

    public event Action<IPooledObject> OnRelease;


    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _collider;

    private float _deathTimer;
    private float _speed;
    private Vector3 _direction;


    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        transform.position += transform.rotation * Velocity * Time.deltaTime;
        _deathTimer -= Time.deltaTime;
        if (_deathTimer <= 0f)
        {
            OnRelease?.Invoke(this);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.TryGetComponent(out IPlayer player))
        {
            player.OnHit();
        }
    }


    public IPooledObject Instantiate(Transform parent)
    {
        return Instantiate(this, parent);
    }
    public void OnPoolGet(float lifeTime)
    {
        _deathTimer = lifeTime;
        gameObject.SetActive(true);
    }
    public void OnPoolRelease()
    {
        gameObject.SetActive(false);
    }
    public void OnPoolDestroy()
    {
        Destroy(gameObject);
    }
    public void SetSize(Vector3 newSize)
    {
        _collider.size = newSize;
        _spriteRenderer.size = newSize;
    }
    public void SetPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }
    public void SetRotation(Quaternion newRotation)
    {
        transform.rotation = newRotation;
    }
    public void SetDirection(Vector3 newDirection)
    {
        _direction = newDirection;
    }
    public void SetSpeed(float newSpeed)
    {
        _speed = newSpeed;
    }
}