using System;
using Evolution.Scripts;
using Random = UnityEngine.Random;

namespace Common.Scripts
{
    [Serializable]
    public class CellGene : ICloneable
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
        
        public virtual void OnCellFixedFrame(Cell cell)
        {
            
        }  
        
        public virtual void OnCellRareFrame(Cell cell)
        {
            
        }
        
        public virtual void OnCellDivided(Cell cell)
        {
            
        }

        public CellGene SetRandom()
        {
            m_Value = Random.value;
            return this;
        }
        
        public virtual CellGene SetValue(float val)
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
            if (Random.value < EvolutionCommonHyperParameters.Instance.m_MutationPossibilityRate)
            {
                var delta = Random.Range(-EvolutionCommonHyperParameters.Instance.m_MutationRangeRate / 2f,
                    EvolutionCommonHyperParameters.Instance.m_MutationRangeRate / 2f);
                SetValue(m_Value + delta);
            }
        }

        public float GetRareFrameTime()
        {
            return EvolutionCommonHyperParameters.Instance.m_RareFramePeriod;
        }
    }
}