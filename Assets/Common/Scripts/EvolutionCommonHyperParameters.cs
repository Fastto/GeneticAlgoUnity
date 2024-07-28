using Common.Scripts;
using UnityEngine;

namespace Evolution.Scripts
{
    public class EvolutionCommonHyperParameters : Singleton<EvolutionCommonHyperParameters>
    {
        [Header("Mutation Parameters")] 
        public float m_MutationPossibilityRate = 0.01f; 
        public float m_MutationRangeRate = 0.1f; 
        public float m_RareFramePeriod = .25f; 
    }
}