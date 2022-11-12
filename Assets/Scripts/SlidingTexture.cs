using UnityEngine;


public class SlidingTexture : MonoBehaviour
{
    [SerializeField] private GameData _gameData;
    [SerializeField] private Renderer _renderer;


    void Update()
    {
        float offset = Time.deltaTime * _gameData.Speed;
        _renderer.material.mainTextureOffset += new Vector2(offset, 0);
    }
}
