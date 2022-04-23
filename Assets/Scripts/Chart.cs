using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chart : MonoBehaviour
{
    [SerializeField] private int _columnNum;
    [SerializeField] private float _widthMultiplier;
    [SerializeField] private string _titleText;
    [SerializeField] private GameObject _columnPrefab;
    [SerializeField] private GameObject _labelPrefab;
    [SerializeField] private Text _title;
    [SerializeField] private RectTransform _drawableArea;
    [SerializeField] private Color _titleColor;

    private RectTransform[] _transforms;
    private Text[] _texts;

    private RectTransform _rectTransform;

    // Start is called before the first frame update
    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();

        //Set Title Text
        _title.text = _titleText;
        //_title.color = _titleColor;

        //Create columns
        _transforms = new RectTransform[_columnNum];
        _texts = new Text[_columnNum];
        
        int step = (int) _drawableArea.rect.width / _columnNum;
        int yStep = (int) _drawableArea.rect.height / 4;
        for (int i = 0; i < 5; i++)
        {
            GameObject newGameObject = Instantiate(_columnPrefab);
            newGameObject.transform.SetParent(_drawableArea.transform);
            RectTransform rt = newGameObject.GetComponent<RectTransform>();
            rt.gameObject.GetComponentInChildren<Text>().alignment = TextAnchor.MiddleLeft;
            rt.gameObject.GetComponentInChildren<Text>().text = "   " + (yStep * i).ToString();
            newGameObject.GetComponent<Image>().color = new Color(1, 1, 1, .25f);
            rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, _drawableArea.rect.width);
            rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, yStep * i, 1);
        }

        for (int i = 0; i < _columnNum; i++)
        {
            //columns
            GameObject newGameObject = Instantiate(_columnPrefab);
            newGameObject.transform.SetParent(_drawableArea.transform);
            newGameObject.GetComponent<Image>().color = _titleColor;
            RectTransform rt = newGameObject.GetComponent<RectTransform>();
            rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, i * step, step - 2);
            rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, 0);
            _transforms[i] = rt;
            _texts[i] = rt.gameObject.GetComponentInChildren<Text>();
        }
    }

    // Update is called once per frame
    // public void ApplyData(int[] data)
    // {
    //     if (data.Length == _transforms.Length)
    //     {
    //         int step = (int) _drawableArea.rect.width / _columnNum;
    //         for (int i = 0; i < _columnNum; i++)
    //         {
    //             _transforms[i].SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, i * step, step - 2);
    //             _transforms[i].SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, data[i]);
    //             _transforms[i].gameObject.GetComponentInChildren<Text>().text = data[i].ToString();
    //         }
    //     }
    // }

    public void ApplyData(float[] data)
    {
        if (data.Length == 0) return;
        
        float min = Mathf.Min(data);
        float max = Mathf.Max(data);

        if ((max - min) < max / _columnNum)
        {
            min = min - (max / _columnNum);
            max = max + (max / _columnNum);
        }

        float columnStep = (max - min) / _columnNum;
        float[] counts = new float[_columnNum];

        if (columnStep == 0f) columnStep = 1f;

        for (int i = 0; i < data.Length; i++)
        {
            float value = data[i];
            int id = (int)((value - min) / columnStep);
            if (id >= counts.Length) id = counts.Length - 1;
            counts[id]++;
        }
        
        int step = (int) _drawableArea.rect.width / _columnNum;
        for (int i = 0; i < _columnNum; i++)
        {
            _transforms[i].SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, i * step, step - 2);
            _transforms[i].SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, counts[i]);
            _texts[i].text = Math.Round((i+1) * columnStep + min, 2).ToString();
        }
    }
}