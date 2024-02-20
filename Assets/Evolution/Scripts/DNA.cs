using System;
using System.Collections.Generic;
using UnityEngine;

namespace Evolution.Scripts
{
    [Serializable]
    public class DNA
    {
        public Dictionary<Gene.Genes, Gene> m_Genes = new Dictionary<Gene.Genes, Gene>();
        
        static public DNA GetRandom()
        {
            DNA dna = new DNA();
            dna.m_Genes.Add(Gene.Genes.PHOTOSYNTESIS, Gene.GetRandom());

            return dna;
        }
        

        public DNA()
        {
            
        }

        public DNA Clone()
        {
            var dna = new DNA();
            dna.m_Genes.Add(Gene.Genes.PHOTOSYNTESIS, m_Genes[Gene.Genes.PHOTOSYNTESIS].Clone());
            
            return dna;
        }

        public DNA Mutate()
        {
            var dna = new DNA();
            dna.m_Genes.Add(Gene.Genes.PHOTOSYNTESIS, m_Genes[Gene.Genes.PHOTOSYNTESIS].Mutate());
            
            return dna;
        }

        public DNA Cross(DNA value)
        {
            return new DNA();
        }
    }
}