using UnityEngine;

namespace Evolution.Scripts.Genes
{
    public class AppearanceGene : Gene
    {
        public override void OnCellBirth(Cell cell)
        {   
            cell.m_Body.color = new Color(0, 0, 0);
        }

        public override void OnCellDied(Cell cell)
        {
        }

        public override void OnCellFrame(Cell cell)
        {
            var scale = (Mathf.Sqrt((cell.m_Energy / 100f) / Mathf.PI) * 2f)* 1f;
            cell.transform.localScale = new Vector3(scale, scale, 1);
        }

        public override void OnCellDivided(Cell cell)
        {

        }
        
    }
}