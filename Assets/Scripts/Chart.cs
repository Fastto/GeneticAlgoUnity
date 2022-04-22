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
        int step = (int)_drawableArea.rect.width / _columnNum;
        int yStep = (int) _drawableArea.rect.height / 4;
        for (int i = 0; i < 5; i++)
        {
            GameObject newGameObject = Instantiate(_columnPrefab);
            newGameObject.transform.SetParent(_drawableArea.transform);
            RectTransform rt = newGameObject.GetComponent<RectTransform>();
            newGameObject.GetComponent<Image>().color = new Color(1, 1,1, .25f);
            rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left,   0, _drawableArea.rect.width);
            rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, yStep * i, 1);
        }
        
        for (int i = 0; i < _columnNum; i++)
        {
            //columns
            GameObject newGameObject = Instantiate(_columnPrefab);
            newGameObject.transform.SetParent(_drawableArea.transform);
            newGameObject.GetComponent<Image>().color = _titleColor;
            RectTransform rt = newGameObject.GetComponent<RectTransform>();
            rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left,   i * step, step-2);
            rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, 0);
            _transforms[i] = rt;
        }
    }

    // Update is called once per frame
    public void ApplyData(int[] data)
    {
        if (data.Length == _transforms.Length)
        {
            int step = (int) _drawableArea.rect.width / _columnNum;
            for (int i = 0; i < _columnNum; i++)
            {
                _transforms[i].SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, i * step, step - 2);
                _transforms[i].SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, data[i]);
                _transforms[i].gameObject.GetComponentInChildren<Text>().text = data[i].ToString();
            }
        }
    }
    
    public void ApplyData(float[] data)
    {
        if (data.Length == _transforms.Length)
        {
            int step = (int) _drawableArea.rect.width / _columnNum;
            for (int i = 0; i < _columnNum; i++)
            {
                _transforms[i].SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, i * step, step - 2);
                _transforms[i].SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, data[i]);
                _transforms[i].gameObject.GetComponentInChildren<Text>().text = data[i].ToString();
            }
        }
    }
    
}
