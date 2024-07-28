using Common.Scripts;
using Common.Scripts.CellParams;
using UnityEngine;

namespace Evolution.Scripts.Genes
{
    public class BirthCellGene : CellGene
    {
        public override void OnCellBirth(Cell cell)
        {   
            cell.m_FloatParams[CellFloatParams.BirthTime] = Time.time;
            cell.m_FloatParams[CellFloatParams.Energy] = EvolutionHyperParameters.Instance.m_CellBirthEnergy;
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