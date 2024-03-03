using System;
using Random = UnityEngine.Random;

namespace Evolution.Scripts
{
    [Serializable]
    public class BinaryGene : Gene
    {
        public override Gene SetValue(float val)
        {
            m_Value = val > .5 ? 1f: 0f;
            return this;
        }
        
        public override void Mutate()
        {
            if (Random.value < EvolutionHyperParameters.Instance.m_MutationPossibilityRate)
            {
                var status = m_Value > .5f;
                status = !status;
                
                SetValue(status ? 1f : 0f);
            }
        }
    }
}