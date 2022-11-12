using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Quit : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
