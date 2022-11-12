using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Obstacle : MonoBehaviour, IPooledObject, ITrajectory, ISize
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private BoxCollider2D _collider;


    public Vector3 Position => transform.position;
    public Quaternion Rotation => transform.rotation;
    public Vector3 Velocity => _direction * _speed;

    public event Action<IPooledObject> OnRelease;


    private float _deathTime;
    private float _speed;
    private Vector3 _direction;


    private void Update()
    {
        if (Time.time > _deathTime)
        {
            OnRelease?.Invoke(this);
        }
    }
    private void FixedUpdate()
    {
        transform.position += transform.rotation * Velocity * Time.deltaTime;
    }
    public IPooledObject Instantiate(Transform parent)
    {
        return Instantiate(this, parent);
    }
    public void OnPoolGet(float lifeTime)
    {
        _deathTime = Time.time + lifeTime;
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