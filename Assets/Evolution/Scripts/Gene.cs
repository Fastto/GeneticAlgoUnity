using System;
using Random = UnityEngine.Random;

namespace Evolution.Scripts
{
    [Serializable]
    public class Gene : ICloneable
    {
        public float m_Value = 0f;
        
        public virtual void OnCellBirth(Cell cell)
        {
            
        }
        
        public virtual void OnCellDied(Cell cell)
        {
            
        }
        
        public virtual void OnCellFrame(Cell cell)
        {
            
        }
        
        public virtual void OnCellDivided(Cell cell)
        {
            
        }

        public Gene SetRandom()
        {
            m_Value = Random.value;
            return this;
        }
        
        public virtual Gene SetValue(float val)
        {
            m_Value = val;
            if (m_Value < 0) m_Value = 0f;
            if (m_Value > 1) m_Value = 1f;
            return this;
        }
        
        public object Clone()
        {
            return MemberwiseClone();
        }
        
        public virtual void Mutate()
        {
            if (Random.value < EvolutionHyperParameters.Instance.m_MutationPossibilityRate)
            {
                var delta = Random.Range(-EvolutionHyperParameters.Instance.m_MutationRangeRate / 2f,
                    EvolutionHyperParameters.Instance.m_MutationRangeRate / 2f);
                SetValue(m_Value + delta);
            }
        }
    }
}