using Common.Scripts;
using Common.Scripts.CellParams;
using UnityEngine;

namespace Evolution.Scripts.Genes
{
    public class PhotosynthesisCellGene : CellGene
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

        public override void OnCellRareFrame(Cell cell)
        {
            cell.m_FloatParams[CellFloatParams.Energy] += GetReward(cell);
            cell.m_FloatParams[CellFloatParams.Energy] -= GetPenalty(cell);
        }

        /**
         * Reward = Lighting Force * Absorbation Rate * Neighbours Rate
         */
        protected float GetReward(Cell cell)
        {
            float neighbourFine = 1f - CellFactory.Instance.GetLightingNeighboursCount(cell) *
                EvolutionHyperParameters.Instance.m_NeighbourLightingFine;
            
            return m_Value
                   * GetRareFrameTime()
                   * EvolutionHyperParameters.Instance.m_LightingAbsorptionPerSecond
                   * LightingManager.Instance.GetIntensivityForPoint(cell.transform.position)
                   * neighbourFine;
        }

        protected float GetPenalty(Cell cell)
        {
            return m_Value
                   * GetRareFrameTime()
                   * EvolutionHyperParameters.Instance.m_LightingGeneEnergyUtilisationRate;
        }
    }
}