using UnityEngine;

namespace Evolution.Scripts.Genes
{
    public class MovementGene : Gene
    {
        public override void OnCellBirth(Cell cell)
        {   
            Jump(cell);
        }

        public override void OnCellDied(Cell cell)
        {
        }

        public override void OnCellFrame(Cell cell)
        {
   
        }

        public override void OnCellDivided(Cell cell)
        {
            Jump(cell);
        }
        
        protected void Jump(Cell cell)
        {
            cell.transform.Rotate(Vector3.forward, Random.Range(0, 359));
            var energyReducingK = EvolutionHyperParameters.Instance.m_CellBirthEnergy / cell.m_Energy;
            var jumpForce = EvolutionHyperParameters.Instance.m_CellBirthForce * energyReducingK;
            cell.m_RigidBody.AddForce(cell.transform.right * jumpForce);
        }
    }
}