using System;
using System.Collections.Generic;
using Evolution.Scripts;

namespace Common.Scripts
{
    [Serializable]
    public class CellGenome
    {
        public List<CellGene> m_Genes = new List<CellGene>();

        public void Activate(Cell cell)
        {
            foreach (var g in m_Genes)
            {
                cell.OnBirth += g.OnCellBirth;
                cell.OnFrame += g.OnCellFrame;
                cell.OnFixedFrame += g.OnCellFixedFrame;
                cell.OnRareFrame += g.OnCellRareFrame;
                cell.OnDivided += g.OnCellDivided;
                cell.OnDied += g.OnCellDied;
            }
        }

        public void Deactivate(Cell cell)
        {
            foreach (var g in m_Genes)
            {
                cell.OnBirth -= g.OnCellBirth;
                cell.OnFrame -= g.OnCellFrame;
                cell.OnFixedFrame -= g.OnCellFixedFrame;
                cell.OnRareFrame -= g.OnCellRareFrame;
                cell.OnDivided -= g.OnCellDivided;
                cell.OnDied -= g.OnCellDied;
            }
        }

        public CellGenome Clone()
        {
            var dna = new CellGenome();
            foreach (var g in m_Genes)
            {
                dna.m_Genes.Add(g.Clone() as CellGene);
            }

            return dna;
        }

        public void Mutate()
        {
            foreach (var g in m_Genes)
            {
                g.Mutate();
            }
        }
        
    }
}