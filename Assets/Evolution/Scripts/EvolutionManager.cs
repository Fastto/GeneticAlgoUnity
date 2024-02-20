using System;
using System.Collections;
using UnityEngine;

namespace Evolution.Scripts
{
    public class EvolutionManager : Singleton<EvolutionManager>
    {
        [SerializeField] protected GameObject m_CellPrefab;

        private void Start()
        {
            StartCoroutine(EvolutionLoop());
        }

        protected IEnumerator EvolutionLoop()
        {
            yield return null;
            
            CellFactory.Instance.SpawnRandomCells(EvolutionHyperParameters.Instance.m_SpawningNumber);
        }
    }
}