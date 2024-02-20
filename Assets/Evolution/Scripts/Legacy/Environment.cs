using System;
using System.Collections;
using System.Collections.Generic;
using Evolution.Scripts;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Environment : Singleton<Environment>
{
    private Texture2D _texture2D;
    private Camera _camera;
    
    [SerializeField] private List<GameObject> _sprites;
    [SerializeField] private List<Image> _images;

    public float TimeToBirtMinValue { get; set; } = .1f; //for coloring, R channel
    public float BirthSizeMinValue { get; set; } = 80f; //for coloring, G channel
    public float BirthForceMinValue { get; set; } = 25f;//for coloring, B channel
        
    public float TimeToBirtMaxValue { get; set; } = .6f; //for coloring, R channel
    public float BirthSizeMaxValue { get; set; } = 120f; //for coloring, G channel
    public float BirthForceMaxValue { get; set; } = 40f; //for coloring, B channel
    
    private void Start()
    {
        SetEnvironment(1);
        _camera = Camera.main;
    }

        public Color GetColor(float x, float y)
        {
            float textureX = (x * _texture2D.width ) / Camera.main.pixelWidth;
            float textureY = (y * _texture2D.height ) / Camera.main.pixelHeight;
            
            if (textureX < 0 || textureY < 0 || textureX >= _texture2D.width || textureY >= _texture2D.height)
                return new Color(0, 0, 0, 0);

            return  _texture2D.GetPixel((int)textureX, (int)textureY);
        }

    private void SetTexture(Texture2D texture2D)
    {
        _texture2D = texture2D;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetEnvironment(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetEnvironment(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetEnvironment(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetEnvironment(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SetEnvironment(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SetEnvironment(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SetEnvironment(7);
        }else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            SetEnvironment(8);
        }
    }

    private void SetEnvironment(int envId)
    {
        SetTexture(_images[envId - 1].mainTexture as Texture2D);
        for (int i = 0; i < _sprites.Count; i++)
        {
            _sprites[i].SetActive(i == (envId - 1));
        }
    }
}
