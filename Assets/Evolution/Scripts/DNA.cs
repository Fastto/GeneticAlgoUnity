using System;
using System.Collections.Generic;
using Evolution.Scripts.Genes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Evolution.Scripts
{
    [Serializable]
    public class DNA
    {
        public List<Gene> m_Genes = new List<Gene>();
        
        static public DNA GetRandom()
        {
            DNA dna = new DNA();
            dna.m_Genes.Add(new BirthGene());
            dna.m_Genes.Add(new AppearanceGene());
            dna.m_Genes.Add(new MovementGene());
            dna.m_Genes.Add(new MetabolismGene());
            // dna.m_Genes.Add(new PhotosyntesisGene().SetRandom());
            // dna.m_Genes.Add(new ParasitismGene().SetRandom());
            dna.m_Genes.Add(new ParasitismPhotosyntesisGene().SetValue(1f));
            dna.m_Genes.Add(new DivideGene());
            dna.m_Genes.Add(new DeathGene());

            return dna;
        }

        public void Activate(Cell cell)
        {
            foreach (var g in m_Genes)
            {
                cell.OnBirth += g.OnCellBirth;
                cell.OnFrame += g.OnCellFrame;
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
                cell.OnDivided -= g.OnCellDivided;
                cell.OnDied -= g.OnCellDied;
            }
        }

        public DNA Clone()
        {
            var dna = new DNA();
            foreach (var g in m_Genes)
            {
                dna.m_Genes.Add(g.Clone() as Gene);
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