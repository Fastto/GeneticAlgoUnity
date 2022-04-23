using System;
using System.Collections;
using System.Collections.Generic;
using SL.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour
{
    [SerializeField] private Chart _chart;
    [SerializeField] private Chart _chart2;
    [SerializeField] private Chart _chart3;

    [SerializeField] private Text _total;
    [SerializeField] private Text _size;
    [SerializeField] private Text _force;
    [SerializeField] private Text _coldDown;
    
    
    [SerializeField] private Image _graph;
    private Texture2D _texture2D;
    
    private int _graphX = 0;
    
    private void Start()
    {
        RectTransform rt = _graph.GetComponent<RectTransform>();
        _texture2D = new Texture2D((int)rt.rect.width, (int)rt.rect.height);
        _graph.material.mainTexture = _texture2D;
        
        CleanGraph();

        StartCoroutine(CollectData());
    }

    public void Restart()
    {
        CleanGraph();
    }

    private void CleanGraph()
    {
        RectTransform rt = _graph.GetComponent<RectTransform>();
        Color c = new Color(0, 0, 0, .2f);
        for (int x = 0; x < (int)rt.rect.width; x++)
        {
            for (int y= 0; y < (int)rt.rect.height; y++)
            {
                _texture2D.SetPixel(x, y, c);
            }
        }
        _texture2D.Apply();
        _graphX = 0;
    }
    

    IEnumerator CollectData()
    {
        while (true)
        {
            // int[] data = new int[20];
            // int[] data2 = new int[20];
            // int[] data3 = new int[20];
            

            Cell[] _list = FindObjectsOfType<Cell>();
            if (_list.Length > 0)
            {
                float[] __data1 = new float[_list.Length];
                float[] __data2 = new float[_list.Length];
                float[] __data3 = new float[_list.Length];

                float size = 0, force = 0, coldDown = 0;
                int id = 0;
                foreach (var item in _list)
                {
                    // int ind = Mathf.RoundToInt(item.GetGenome().BirthSize * .1f);
                    // ind = ind < 0 ? 0 : (ind > data.Length - 1 ? data.Length - 1 : ind);
                    // data[ind] += 1;
                    //
                    // int ind2 = Mathf.RoundToInt(item.GetGenome().TimeToBirth * 20);
                    // ind2 = ind2 < 0 ? 0 : (ind2 > data2.Length - 1 ? data2.Length - 1 : ind2);
                    // data2[ind2] += 1;
                    //
                    // int ind3 = Mathf.RoundToInt(item.GetGenome().BirthForce * .25f);
                    // ind3 = ind3 < 0 ? 0 : (ind3 > data3.Length - 1 ? data3.Length - 1 : ind3);
                    // data3[ind3] += 1;
                    __data1[id] = item.GetGenome().BirthSize;
                    __data3[id] = item.GetGenome().BirthForce;
                    __data2[id] = item.GetGenome().TimeToBirth;

                    size += item.GetGenome().BirthSize;
                    force += item.GetGenome().BirthForce;
                    coldDown += item.GetGenome().TimeToBirth;
                    id++;
                }

                // _chart.ApplyData(data);
                // _chart2.ApplyData(data2);
                // _chart3.ApplyData(data3);

                _chart.ApplyData(__data1);
                _chart2.ApplyData(__data2);
                _chart3.ApplyData(__data3);

                float total = _list.Length;
                _total.text = total.ToString();
                _size.text = total > 0 ? (size / total).ToString() : "0";
                _force.text = total > 0 ? (force / total).ToString() : "0";
                _coldDown.text = total > 0 ? (coldDown / total).ToString() : "0";

                _texture2D.SetPixel(_graphX, (int) (total / 4f), new Color(1, 1, 1));
                _texture2D.SetPixel(_graphX, (int) ((size / (float) total) * 4f) + 120, Color.green);
                _texture2D.SetPixel(_graphX, (int) ((force / (float) total) * 4f) - 30, Color.blue);
                _texture2D.SetPixel(_graphX, (int) ((coldDown / (float) total) * 500f) - 510, Color.red);
                _graphX++;
                _texture2D.Apply();
            }

            yield return new WaitForSeconds(1);
        }
    }
}
