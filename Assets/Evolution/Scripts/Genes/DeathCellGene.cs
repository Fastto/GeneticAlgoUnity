using Common.Scripts;
using Common.Scripts.CellParams;
using UnityEngine;

namespace Evolution.Scripts.Genes
{
    public class DeathCellGene : CellGene
    {
        public override void OnCellBirth(Cell cell)
        {   

        }

        public override void OnCellDied(Cell cell)
        {
        }

        public override void OnCellFrame(Cell cell)
        {
            if(Time.time - cell.m_FloatParams[CellFloatParams.BirthTime] > EvolutionHyperParameters.Instance.m_CellLifeTime)
            {
                cell.m_IsDead = true;
            }

            if (cell.m_FloatParams[CellFloatParams.Energy] < EvolutionHyperParameters.Instance.m_CellDeathEnergy)
            {
                cell.m_IsDead = true;
            }
        }

        public override void OnCellDivided(Cell cell)
        {

        }
        
    }
}