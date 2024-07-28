using System;
using Evolution.Scripts;
using Random = UnityEngine.Random;

namespace Common.Scripts
{
    [Serializable]
    public class BinaryCellGene : CellGene
    {
        public override CellGene SetValue(float val)
        {
            m_Value = val > .5 ? 1f: 0f;
            return this;
        }
        
        public override void Mutate()
        {
            if (Random.value < EvolutionCommonHyperParameters.Instance.m_MutationPossibilityRate)
            {
                var status = m_Value > .5f;
                status = !status;
                
                SetValue(status ? 1f : 0f);
            }
        }
    }
}