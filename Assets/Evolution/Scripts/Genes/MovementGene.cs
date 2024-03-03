using UnityEngine;

namespace Evolution.Scripts.Genes
{
    public class MovementGene : Gene
    {
        private float m_MinForce = 10f;
        private float m_MaxForce = 100f;

        private float m_Force;
        
        public override void OnCellBirth(Cell cell)
        {
            m_Force = m_MinForce + (m_MaxForce - m_MinForce) * m_Value;
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
            // var jumpForce = EvolutionHyperParameters.Instance.m_CellBirthForce * energyReducingK;
            var jumpForce = m_Force * energyReducingK;
            cell.m_RigidBody.AddForce(cell.transform.right * jumpForce);
        }
    }
}