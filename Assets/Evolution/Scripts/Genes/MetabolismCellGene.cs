using Common.Scripts;
using Common.Scripts.CellParams;
using UnityEngine;

namespace Evolution.Scripts.Genes
{
    public class MetabolismCellGene : CellGene
    {
        public override void OnCellBirth(Cell cell)
        {   

        }

        public override void OnCellDied(Cell cell)
        {
        }

        public override void OnCellRareFrame(Cell cell)
        {
            cell.m_FloatParams[CellFloatParams.Energy] -= GetSizeEnergyFine(cell);
        }

        public override void OnCellDivided(Cell cell)
        {

        }

        protected float GetSizeEnergyFine(Cell cell)
        {
            return GetRareFrameTime()
                   * cell.m_FloatParams[CellFloatParams.Energy]
                   * EvolutionHyperParameters.Instance.m_CellSizeFine;
        }
        
    }
}