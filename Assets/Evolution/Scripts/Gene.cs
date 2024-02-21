using System;
using Random = UnityEngine.Random;

namespace Evolution.Scripts
{
    [Serializable]
    public class Gene
    {
        public float m_Value;
        
        static public Gene GetRandom()
        {
            return new Gene(Random.value);
        }
        
        static public Gene GetZero()
        {
            return new Gene(0f);
        }

        public Gene(float value = 0f)
        {
            m_Value = value;
        }
        
        public enum Genes
        {
            VOID,
            PHOTOSYNTESIS,
            PARASITISM
        }
        
        public Gene Clone()
        {
            return new Gene(m_Value);
        }

        public Gene Mutate()
        {
            if (Random.value < EvolutionHyperParameters.Instance.m_MutationPossibilityRate)
            {
                var delta = Random.Range(-EvolutionHyperParameters.Instance.m_MutationRangeRate / 2f,
                    EvolutionHyperParameters.Instance.m_MutationRangeRate / 2f);
                m_Value += delta;
                if (m_Value < 0) m_Value = 0f;
                if (m_Value > 1) m_Value = 1f;
            }
            return new Gene(m_Value);
        }

        public Gene Cross(Gene value)
        {
            var val = Random.value > .5 ? value.m_Value : m_Value;
            return new Gene(val);
        }
    }
}