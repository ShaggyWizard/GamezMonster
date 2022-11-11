using System;
using UnityEngine;


public interface IPooledObject
{
    public event Action<IPooledObject> OnRelease;

    public IPooledObject Instantiate(Transform parent);
    public void OnPoolGet(float lifeTime);
    public void OnPoolRelease();
    public void OnPoolDestroy();
}