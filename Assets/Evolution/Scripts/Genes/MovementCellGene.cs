using System;
using Common.Scripts;
using Common.Scripts.CellParams;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Evolution.Scripts.Genes
{
    public class MovementCellGene : CellGene
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
            if (cell.m_FloatParams[CellFloatParams.Energy] <= 0f)
                throw new Exception("Energy can not be 0 for jump");
            
            cell.transform.Rotate(Vector3.forward, Random.Range(0, 359));
            var energyReducingK = EvolutionHyperParameters.Instance.m_CellBirthEnergy / cell.m_FloatParams[CellFloatParams.Energy];
            // var jumpForce = EvolutionHyperParameters.Instance.m_CellBirthForce * energyReducingK;
            var jumpForce = m_Force * energyReducingK;
            cell.GetRigidbody2D().AddForce(cell.transform.right * jumpForce);
        }
    }
}