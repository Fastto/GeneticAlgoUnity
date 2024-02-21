using UnityEngine;

namespace Evolution.Scripts
{
    public class EvolutionHyperParameters : Singleton<EvolutionHyperParameters>
    {
        public GameObject m_CellPrefab;

        [Header("Initial Spawn Params")] 
        public float m_SpawningWifth;
        public float m_SpawningHeight;
        public int m_SpawningNumber;
        
        [Header("Mutation Parameters")] 
        public float m_MutationPossibilityRate; //.05
        public float m_MutationRangeRate; //.5
        
        [Space] [Header("Lighting")] 
        public float m_LightingAbsorptionPerSecond;
        public float m_LightingGeneEnergyUtilisationRate;

        [Space] [Header("PARASITISM")] 
        public float m_ParasitismRate;
        public float m_ParasitismDistance;
        public float m_ParasitismTimeRecalc;
        
        [Space] [Header("Cell Parameters")] 
        public float m_CellBirthEnergy; // 100f
        public float m_CellDevidingEnergy; // 300f
        public float m_CellBirthForce; //10f
        public float m_CellDeathEnergy; //10f
        public float m_CellLifeTime; // 30f
        public float m_CellSizeFine; // 30f

        [Space] 
        public float m_NeigbourMaxDistance;
        public float m_NeigbourTimeRecalc;
        public float m_NeighbourLightingFine;
    }
}