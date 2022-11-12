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
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private GameObject _pooledObjectPrefab;


    private BoxCollider2D _bounds;
    private IPooledObject _pooledObject;
    private IObjectPool<IPooledObject> _pool;

    private float _distance;
    private float _lastDistance;
    private float _offset;

    private const int _gizmoSize = 2;
    private const int _defaultWidth = 1;
    private const int _minHeight = 1;


    private void Awake()
    {
        _bounds = GetComponent<BoxCollider2D>();
           _distance = 0;
        _lastDistance = 0;
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
            arrowPos = _bounds.offset + Vector2.right * _bounds.size.x * 0.5f;
        }
        Handles.ArrowHandleCap(0, arrowPos, Quaternion.LookRotation(Vector3.left), _gizmoSize, EventType.Repaint);
    }
    private void Update()
    {
        _distance += _gameData.Speed * Time.deltaTime;
        if (_distance >= _lastDistance + _gameSettings.spacing)
        {
            _lastDistance += _gameSettings.spacing;
            _offset = (_lastDistance - _distance) * _gameData.Speed * Time.deltaTime;
            _pool.Get();
        }
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

        var trajectory = obj as ITrajectory;
        trajectory.SetSpeed(_gameData.Speed);

        _gameData.OnSpeedChange += trajectory.SetSpeed;

        return obj;
    }
    private void OnGetPooledObject(IPooledObject obj)
    {
        float right = _bounds.offset.x + _bounds.size.x * 0.5f;
        float top = _bounds.offset.y + _bounds.size.y * 0.5f;
        float bottom = _bounds.offset.y - _bounds.size.y * 0.5f;
        float spawnHeight = Mathf.CeilToInt(Random.Range(top, bottom));

        Vector2 localPos = new Vector2(right + _offset, spawnHeight);

        var size = obj as ISize;
        if (size != null)
        {
            int height = Random.Range(_minHeight, _maxHeight);
            size.SetSize(new Vector2(_defaultWidth, height));
            if (height % 2 != 0)
            {
                localPos += Vector2.down * 0.5f;
            }
        }

        var trajectory = obj as ITrajectory;
        trajectory.SetPosition(transform.position + transform.rotation * localPos);
        trajectory.SetRotation(transform.rotation);
        trajectory.SetDirection(Vector2.left);

        obj.OnPoolGet(_bounds.size.x / trajectory.Velocity.magnitude);
    }
    private void OnReleasePooledObject(IPooledObject obj)
    {
        obj.OnPoolRelease();
    }
    private void OnDestroyPooledObject(IPooledObject obj)
    {
        var trajectory = obj as ITrajectory;
        trajectory.SetSpeed(_gameData.Speed);

        _gameData.OnSpeedChange -= trajectory.SetSpeed;

        obj.OnPoolDestroy();
    }
}
