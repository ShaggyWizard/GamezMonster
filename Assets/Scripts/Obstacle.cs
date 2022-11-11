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


    private float _deathTime;


    public Vector3 Position { get { return transform.position; } set { transform.position = value; } }
    public Quaternion Rotation { get { return transform.rotation; } set { transform.rotation = value; } }
    public Vector3 Velocity { get; set; }

    public event Action<IPooledObject> OnRelease;


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
}
