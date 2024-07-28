using Common.Scripts;
using Common.Scripts.CellParams;
using UnityEngine;

namespace Evolution.Scripts.Genes
{
    public class DivideCellGene : CellGene
    {
        public override void OnCellBirth(Cell cell)
        {   

        }

        public override void OnCellDied(Cell cell)
        {
        }

        public override void OnCellFixedFrame(Cell cell)
        {
            if (cell.m_FloatParams[CellFloatParams.Energy] >= EvolutionHyperParameters.Instance.m_CellDevidingEnergy)
            {
                CellFactory.Instance.SpawnChild(cell);
                cell.m_FloatParams[CellFloatParams.Energy] -= EvolutionHyperParameters.Instance.m_CellBirthEnergy;
                cell.OnDivided?.Invoke(cell);
            }
        }

        public override void OnCellDivided(Cell cell)
        {

        }
        
    }
}