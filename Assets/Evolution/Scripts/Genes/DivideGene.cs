using UnityEngine;

namespace Evolution.Scripts.Genes
{
    public class DivideGene : Gene
    {
        public override void OnCellBirth(Cell cell)
        {   

        }

        public override void OnCellDied(Cell cell)
        {
        }

        public override void OnCellFrame(Cell cell)
        {
            if (cell.m_Energy >= EvolutionHyperParameters.Instance.m_CellDevidingEnergy)
            {
                CellFactory.Instance.SpawnChild(cell);
                cell.m_Energy -= EvolutionHyperParameters.Instance.m_CellBirthEnergy;
                cell.OnDivided?.Invoke(cell);
            }
        }

        public override void OnCellDivided(Cell cell)
        {

        }
        
    }
}