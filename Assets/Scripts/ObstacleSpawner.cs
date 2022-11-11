using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;


[RequireComponent(typeof(BoxCollider2D))]
public class ObstacleSpawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Min(_minHeight)] private int _maxHeight;

    [Header("Dependencies")]
    [SerializeField] private GameData _gameData;
    [SerializeField] private GameObject _pooledObjectPrefab;
    [SerializeField] private BoxCollider2D _bounds;


    private IPooledObject _pooledObject;
    private IObjectPool<IPooledObject> _pool;

    private const int _gizmoSize = 2;
    private const int _defaultWidth = 1;
    private const int _minHeight = 1;
    private const float _half = 0.5f;


    private void Awake()
    {
        _pool = new ObjectPool<IPooledObject>(CreatePooledObject, OnGetPooledObject, OnReleasePooledObject, OnDestroyPooledObject);

        if (!SetObstacle(_pooledObjectPrefab)) { Destroy(this); }
    }
    private void OnDrawGizmosSelected()
    {
        Handles.matrix = transform.localToWorldMatrix;
        Handles.color = Color.cyan;
        Vector3 arrowPos = Vector3.zero;
        if (_bounds != null)
        {
            arrowPos = _bounds.offset + Vector2.right * _bounds.size.x * _half;
        }
        Handles.ArrowHandleCap(0, arrowPos, Quaternion.LookRotation(Vector3.left), _gizmoSize, EventType.Repaint);
    }
    private void Update()
    {
        Spawn();
    }


    public void Spawn()
    {
        _pool.Get();
    }
    public bool SetObstacle(GameObject newObstacle)
    {
        bool valid = true;
        if (newObstacle == null)
        {
            Debug.LogError("ObstacleGenerator - SetObstacle newObstacle is null.");
            return false;
        }
        if (!newObstacle.TryGetComponent(out IPooledObject newPooledObject))
        {
            Debug.LogError("ObstacleGenerator - SetObstacle newObstacle must have a IPooledObject.");
            valid = false;
        }
        if (newPooledObject is not ITrajectory)
        {
            Debug.LogError("ObstacleGenerator - SetObstacle newObstacle must have a ITrajectory.");
            valid = false;
        }
        if (valid)
        {
            _pooledObject = newPooledObject;
        }

        return valid;
    }


    private IPooledObject CreatePooledObject()
    {
        var obj = _pooledObject.Instantiate(transform);
        obj.OnRelease += (s) => _pool.Release(s);
        return obj;
    }
    private void OnGetPooledObject(IPooledObject obj)
    {
        float right = _bounds.offset.x + _bounds.size.x * _half;
        float top = _bounds.offset.y + _bounds.size.y * _half;
        float bottom = _bounds.offset.y - _bounds.size.y * _half;
        float spawnHeight = Random.Range(top, bottom);
        Vector2 localPos = new Vector2(right, spawnHeight);

        var trajectory = obj as ITrajectory;
        trajectory.Position = transform.position + transform.rotation * localPos;
        trajectory.Rotation = transform.rotation;
        trajectory.Velocity = Vector2.left * 10;

        var size = obj as ISize;
        if (size != null)
        {
            size.SetSize(new Vector2(_defaultWidth, Random.Range(_minHeight, _maxHeight)));
        }
        obj.OnPoolGet(_bounds.size.x / trajectory.Velocity.magnitude);
    }
    private void OnReleasePooledObject(IPooledObject obj)
    {
        obj.OnPoolRelease();
    }
    private void OnDestroyPooledObject(IPooledObject obj)
    {
        obj.OnPoolDestroy();
    }
}
