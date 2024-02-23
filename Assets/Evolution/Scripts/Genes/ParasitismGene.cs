using UnityEngine;

namespace Evolution.Scripts.Genes
{
    public class ParasitismGene : Gene
    {
        public override void OnCellBirth(Cell cell)
        {
            var color = cell.m_Body.color;
            color.r = m_Value;
            cell.m_Body.color = color;
        }

        public override void OnCellDied(Cell cell)
        {
        }

        public override void OnCellFrame(Cell cell)
        {
            cell.m_Energy += GetReward(cell);
            cell.m_Energy -= GetPenalty(cell);
        }
        
        protected float GetReward(Cell cell)
        {
            var stolenEnergy = 0f;

            var neighbours = CellFactory.Instance.GetParasitismNeighbours(cell);
            foreach (var n in neighbours)
            {
                if (n.gameObject.activeSelf)
                {
                    // var delta = m_Energy * EvolutionHyperParameters.Instance.m_ParasitismRate * Time.deltaTime;
                    var delta = EvolutionHyperParameters.Instance.m_ParasitismRate * Time.deltaTime * m_Value;
                    stolenEnergy += delta;
                    n.m_Energy -= delta;
                }
            }

            return stolenEnergy;
        }

        protected float GetPenalty(Cell cell)
        {
            return 0f;
        }
    }
}