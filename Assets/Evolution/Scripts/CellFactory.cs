using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Evolution.Scripts
{
    public class CellFactory : Singleton<CellFactory>
    {
        protected List<Cell> m_ActiveCells;

        protected Dictionary<Cell, KeyValuePair<float, int>> m_Neighbours;

        protected void Start()
        {
            m_ActiveCells = new List<Cell>();
            m_Neighbours = new Dictionary<Cell, KeyValuePair<float, int>>();
        }

        public void SpawnRandomCells(int num)
        {
            for (int i = 0; i < num; i++)
            {
                Cell cell = Instantiate(
                    EvolutionHyperParameters.Instance.m_CellPrefab,
                    new Vector3(
                        Random.Range(-EvolutionHyperParameters.Instance.m_SpawningWifth / 2f,
                            EvolutionHyperParameters.Instance.m_SpawningWifth / 2f),
                        Random.Range(-EvolutionHyperParameters.Instance.m_SpawningHeight / 2f,
                            EvolutionHyperParameters.Instance.m_SpawningHeight / 2f), 0),
                    Quaternion.identity).GetComponent<Cell>();
                cell.SetDNA(DNA.GetRandom());

                RegisterCell(cell);
            }
        }

        public void SpawnChild(Cell parent)
        {
            var cell = Instantiate(EvolutionHyperParameters.Instance.m_CellPrefab, parent.transform.position, Quaternion.identity).GetComponent<Cell>();
            cell.SetDNA(parent.GetDNA().Mutate());
            
            RegisterCell(cell);
        }

        protected void RegisterCell(Cell cell)
        {
            cell.OnDied += OnCellDied;
            m_ActiveCells.Add(cell);
            m_Neighbours.Add(cell, new KeyValuePair<float, int>(Time.time, CalcNeighbours(cell)));
        }

        protected void OnCellDied(Cell cell)
        {
            cell.OnDied -= OnCellDied;
            m_ActiveCells.Remove(cell);
        }

        public int GetNeighbours(Cell cell)
        {
            var cellData = m_Neighbours[cell];
            var time = cellData.Key;
            var number = cellData.Value;

            if (Time.time - time > EvolutionHyperParameters.Instance.m_NeigbourTimeRecalc)
            {
                number = CalcNeighbours(cell);
                m_Neighbours[cell] = new KeyValuePair<float, int>(Time.time, number);
            }

            return number;
        }

        protected int CalcNeighbours(Cell cell)
        {
            int number = 0;

            foreach (var _cell in m_ActiveCells)
            {
                if (_cell != cell)
                {
                    var dist = (_cell.transform.position - cell.transform.position).magnitude;
                    number += dist < EvolutionHyperParameters.Instance.m_NeigbourMaxDistance ? 1 : 0;
                }
            }

            return number;
        }
    }
}