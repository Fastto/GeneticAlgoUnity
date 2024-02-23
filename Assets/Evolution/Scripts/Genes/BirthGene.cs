using UnityEngine;

namespace Evolution.Scripts.Genes
{
    public class BirthGene : Gene
    {
        public override void OnCellBirth(Cell cell)
        {   
            cell.m_BirthTime = Time.time;
            cell.m_Energy = EvolutionHyperParameters.Instance.m_CellBirthEnergy;
            cell.m_IsDead = false;
        }

        public override void OnCellDied(Cell cell)
        {
        }

        public override void OnCellFrame(Cell cell)
        {
   
        }

        public override void OnCellDivided(Cell cell)
        {

        }
        
    }
}