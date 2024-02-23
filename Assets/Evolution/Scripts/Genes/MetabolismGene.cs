using UnityEngine;

namespace Evolution.Scripts.Genes
{
    public class MetabolismGene : Gene
    {
        public override void OnCellBirth(Cell cell)
        {   

        }

        public override void OnCellDied(Cell cell)
        {
        }

        public override void OnCellFrame(Cell cell)
        {
            cell.m_Energy -= GetSizeEnergyFine(cell);
        }

        public override void OnCellDivided(Cell cell)
        {

        }

        protected float GetSizeEnergyFine(Cell cell)
        {
            return Time.deltaTime
                   * cell.m_Energy
                   * EvolutionHyperParameters.Instance.m_CellSizeFine;
        }
        
    }
}