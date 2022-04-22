using System;
using System.Collections;
using System.Collections.Generic;
using SL.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class EvolutionManager : MonoBehaviour
{
    [SerializeField] private GameObject CellPrefab;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private Stat _stat;

    private bool _showUI = true;

    private void Start()
    {
      GenerateCell();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _showUI = !_showUI;
            _canvas.SetActive(_showUI);
        } 
        else if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }

    private void GenerateCell()
    {
        GameObject go = Instantiate(CellPrefab, new Vector3(15, 5, 0), Quaternion.identity);
        Cell cell = go.GetComponent<Cell>();
        cell.SetGenome(new Genome());
    }

    private void DestroyAllCells()
    {
        Cell[] _list = FindObjectsOfType<Cell>();
        for (int i = 0; i < _list.Length; i++)
        {
            Destroy(_list[i].gameObject);
        }
    }

    private void Restart()
    {
        DestroyAllCells();
        _stat.Restart();
        GenerateCell();
    }
}