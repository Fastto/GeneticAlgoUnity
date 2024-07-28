using Common.Scripts;
using UnityEngine;

namespace Evolution.Scripts.Genes
{
    public class EmptyCellGene : CellGene
    {
        public override void OnCellBirth(Cell cell)
        {   

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