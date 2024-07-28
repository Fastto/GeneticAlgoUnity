using Common.Scripts;
using Common.Scripts.CellParams;
using UnityEngine;

namespace Evolution.Scripts.Genes
{
    public class ParasitismPhotosynthesisCellGene : BinaryCellGene
    {
        public override void OnCellBirth(Cell cell)
        {
            var color = cell.m_Body.color;
            if (m_Value > .5f)
            {
                color.g = 1;
                cell.m_BoolParams[CellBoolParams.IsParasite] = false;
            }
            else
            {
                color.r = 1;
                cell.m_BoolParams[CellBoolParams.IsParasite] = true;
            }

            cell.m_Body.color = color;
        }

        public override void OnCellDied(Cell cell)
        {
        }

        public override void OnCellRareFrame(Cell cell)
        {
            if (m_Value > .5f)
            {
                cell.m_FloatParams[CellFloatParams.Energy] += GetReward(cell);
                cell.m_FloatParams[CellFloatParams.Energy] -= GetPenalty(cell);
            }
            else
            {
                cell.m_FloatParams[CellFloatParams.Energy] += GetParasitismReward(cell);
            }
        }
        
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
        
        protected float GetParasitismReward(Cell cell)
        {
            var stolenEnergy = 0f;

            var neighbours = CellFactory.Instance.GetParasitismNeighbours(cell);
            foreach (var n in neighbours)
            {
                if (n.gameObject.activeSelf)
                {
                    // var delta = m_Energy * EvolutionHyperParameters.Instance.m_ParasitismRate * Time.deltaTime;
                    var delta = EvolutionHyperParameters.Instance.m_ParasitismRate 
                                * GetRareFrameTime();
                    stolenEnergy += delta;
                    n.m_FloatParams[CellFloatParams.Energy] -= delta;
                }
            }

            return stolenEnergy;
        }
    }
}