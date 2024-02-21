using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Evolution.Scripts
{
    public class CellFactory : Singleton<CellFactory>
    {
        protected List<Cell> m_ActiveCells;
        protected List<Cell> m_UnactiveCells;

        protected Dictionary<Cell, KeyValuePair<float, int>> m_LightingNeighbours;
        protected Dictionary<Cell, KeyValuePair<float, List<Cell>>> m_ParasitismNeighbours;

        protected void Start()
        {
            m_ActiveCells = new List<Cell>();
            m_UnactiveCells = new List<Cell>();
            m_LightingNeighbours = new Dictionary<Cell, KeyValuePair<float, int>>();
            m_ParasitismNeighbours = new Dictionary<Cell, KeyValuePair<float, List<Cell>>>();
        }

        public void SpawnRandomCells(int num)
        {
            for (int i = 0; i < num; i++)
            {
                var cell = GetNewCell();
                cell.transform.position = new Vector3(
                    Random.Range(-EvolutionHyperParameters.Instance.m_SpawningWifth / 2f,
                        EvolutionHyperParameters.Instance.m_SpawningWifth / 2f),
                    Random.Range(-EvolutionHyperParameters.Instance.m_SpawningHeight / 2f,
                        EvolutionHyperParameters.Instance.m_SpawningHeight / 2f), 0);
                cell.SetDNA(DNA.GetRandom());

                RegisterCell(cell);
            }
        }

        public void SpawnChild(Cell parent)
        {
            var cell = GetNewCell();
            cell.transform.position = parent.transform.position;
            cell.SetDNA(parent.GetDNA().Mutate());
            
            RegisterCell(cell);
        }

        protected Cell GetNewCell()
        {
            if (m_UnactiveCells.Count > 0)
            {
                var cell = m_UnactiveCells[0];
                m_UnactiveCells.Remove(cell);
                cell.gameObject.SetActive(true);
                return cell;
            }
            else
            {
                return Instantiate(EvolutionHyperParameters.Instance.m_CellPrefab, Vector3.zero, Quaternion.identity).GetComponent<Cell>();
            }
        }

        protected void RegisterCell(Cell cell)
        {
            cell.OnDied += OnCellDied;
            m_ActiveCells.Add(cell);
            m_LightingNeighbours.Add(cell, new KeyValuePair<float, int>(Time.time, CalcLightingNeighboursCount(cell)));
            m_ParasitismNeighbours.Add(cell, new KeyValuePair<float, List<Cell>>(Time.time, FindParasitismNeighbours(cell)));
        }

        protected void OnCellDied(Cell cell)
        {
            cell.OnDied -= OnCellDied;
            m_ActiveCells.Remove(cell);
            m_LightingNeighbours.Remove(cell);
            m_ParasitismNeighbours.Remove(cell);
            m_UnactiveCells.Add(cell);
        }

        public int GetLightingNeighboursCount(Cell cell)
        {
            var cellData = m_LightingNeighbours[cell];
            var time = cellData.Key;
            var number = cellData.Value;

            if (Time.time - time > EvolutionHyperParameters.Instance.m_NeigbourTimeRecalc)
            {
                number = CalcLightingNeighboursCount(cell);
                m_LightingNeighbours[cell] = new KeyValuePair<float, int>(Time.time, number);
            }

            return number;
        }
        
        public List<Cell> GetParasitismNeighbours(Cell cell)
        {
            var cellData = m_ParasitismNeighbours[cell];
            var time = cellData.Key;
            var neighbours = cellData.Value;

            if (Time.time - time > EvolutionHyperParameters.Instance.m_ParasitismTimeRecalc)
            {
                neighbours = FindParasitismNeighbours(cell);
                m_ParasitismNeighbours[cell] = new KeyValuePair<float, List<Cell>>(Time.time, neighbours);
            }

            return neighbours;
        }

        protected int CalcLightingNeighboursCount(Cell cell)
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
        
        protected List<Cell> FindParasitismNeighbours(Cell cell)
        {
            List<Cell> neighbours = new List<Cell>();

            foreach (var _cell in m_ActiveCells)
            {
                if (_cell != cell)
                {
                    var dist = (_cell.transform.position - cell.transform.position).magnitude;
                    if (dist < EvolutionHyperParameters.Instance.m_ParasitismDistance)
                    {
                        neighbours.Add(_cell);
                    }
                }
            }

            return neighbours;
        }
    }
}