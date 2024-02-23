using UnityEngine;

namespace Evolution.Scripts.Genes
{
    public class PhotosyntesisGene : Gene
    {
        public override void OnCellBirth(Cell cell)
        {
            var color = cell.m_Body.color;
            color.g = m_Value;
            cell.m_Body.color = color;
        }

        public override void OnCellDied(Cell cell)
        {
        }

        public override void OnCellFrame(Cell cell)
        {
            cell.m_Energy += GetReward(cell);
            cell.m_Energy -= GetPenalty(cell);
        }

        /**
         * Reward = Lighting Force * Absorbation Rate * Neighbours Rate
         */
        protected float GetReward(Cell cell)
        {
            float neighbourFine = 1f - CellFactory.Instance.GetLightingNeighboursCount(cell) *
                EvolutionHyperParameters.Instance.m_NeighbourLightingFine;
            
            return m_Value
                   * Time.deltaTime
                   * EvolutionHyperParameters.Instance.m_LightingAbsorptionPerSecond
                   * LightingManager.Instance.GetIntensivityForPoint(cell.transform.position)
                   * neighbourFine;
        }

        protected float GetPenalty(Cell cell)
        {
            return m_Value
                   * Time.deltaTime
                   * EvolutionHyperParameters.Instance.m_LightingGeneEnergyUtilisationRate;
        }
    }
}