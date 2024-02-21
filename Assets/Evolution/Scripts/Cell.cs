using System;
using System.Collections;
using System.ComponentModel;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Evolution.Scripts
{
    public class Cell : MonoBehaviour
    {
        protected Rigidbody2D m_RigidBody;
        protected float m_BirthTime;

        protected float m_Energy;
        
        [SerializeField] private SpriteRenderer m_Body;
        
        [SerializeField] 
        protected DNA m_DNA;

        public Action<Cell> OnDied;

        protected void Start()
        {
            m_RigidBody = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            StartCoroutine(LifeLoop());
        }

        protected IEnumerator LifeLoop()
        {
            yield return null;
            yield return StartCoroutine(Birth());
            yield return StartCoroutine(Life());
            yield return StartCoroutine(Death());
        }

        protected IEnumerator Birth()
        {
            m_BirthTime = Time.time;
            m_Energy = EvolutionHyperParameters.Instance.m_CellBirthEnergy;

            if (m_DNA == null)
            {
                m_DNA = DNA.GetRandom();
            }

            var gr = m_DNA.m_Genes[Gene.Genes.PHOTOSYNTESIS].m_Value;
            var rd = m_DNA.m_Genes[Gene.Genes.PARASITISM].m_Value;
            var bl = 0f;
            m_Body.color = new Color(rd, gr, bl);
            
            Jump();
            yield return null;
        }
        
        protected IEnumerator Life()
        {
            while (Time.time - m_BirthTime < EvolutionHyperParameters.Instance.m_CellLifeTime 
                   && m_Energy >= EvolutionHyperParameters.Instance.m_CellDeathEnergy)
            {
                m_Energy += GetLightingEnergyIncome();
                m_Energy -= GetLightingEnergyOutcome();

                m_Energy += GetStolenEnergy();

                m_Energy -= GetSizeEnergyFine();
                

                if (m_Energy >= EvolutionHyperParameters.Instance.m_CellDevidingEnergy)
                {
                    Devide();
                }
                
                ApplyEnergy();
                
                yield return null;
            }
        }
        
        protected IEnumerator Death()
        {
            OnDied?.Invoke(this);
            gameObject.SetActive(false);
            
            yield return null;
        }

        protected void Devide()
        {
           CellFactory.Instance.SpawnChild(this);
           m_Energy -= EvolutionHyperParameters.Instance.m_CellBirthEnergy;
           Jump();
        }

        protected float GetLightingEnergyIncome()
        {
            float neighbourFine = 1f - CellFactory.Instance.GetLightingNeighboursCount(this) *
                                  EvolutionHyperParameters.Instance.m_NeighbourLightingFine;
            return Time.deltaTime 
                   * EvolutionHyperParameters.Instance.m_LightingAbsorptionPerSecond 
                   * LightingManager.Instance.GetIntensivityForPoint(transform.position) 
                   * m_DNA.m_Genes[Gene.Genes.PHOTOSYNTESIS].m_Value
                   * neighbourFine;
        }
        
        protected float GetLightingEnergyOutcome()
        {
            return m_DNA.m_Genes[Gene.Genes.PHOTOSYNTESIS].m_Value
                   * EvolutionHyperParameters.Instance.m_LightingGeneEnergyUtilisationRate 
                   * Time.deltaTime;
        }

        protected float GetSizeEnergyFine()
        {
            return Time.deltaTime
                   * m_Energy
                   * EvolutionHyperParameters.Instance.m_CellSizeFine;
        }

        protected float GetStolenEnergy()
        {
            var stolenEnergy = 0f;

            var neighbours = CellFactory.Instance.GetParasitismNeighbours(this);
            foreach (var n in neighbours)
            {
                if (n.gameObject.activeSelf)
                {
                    // var delta = m_Energy * EvolutionHyperParameters.Instance.m_ParasitismRate * Time.deltaTime;
                    var delta = EvolutionHyperParameters.Instance.m_ParasitismRate * Time.deltaTime * m_DNA.m_Genes[Gene.Genes.PARASITISM].m_Value;
                    stolenEnergy += delta;
                    n.Steal(delta);
                }
            }

            return stolenEnergy;
        }

        protected void ApplyEnergy()
        {
            var scale = (Mathf.Sqrt((m_Energy / 100f) / Mathf.PI) * 2f)* 1f;
            transform.localScale = new Vector3(scale, scale, 1);
        }

        public void SetDNA(DNA dna)
        {
            m_DNA = dna.Clone();
        }
        
        public DNA GetDNA()
        {
            return m_DNA.Clone();
        }

        protected void Jump()
        {
            transform.Rotate(Vector3.forward, Random.Range(0, 359));
            var energyReducingK = EvolutionHyperParameters.Instance.m_CellBirthEnergy / m_Energy;
            var jumpForce = EvolutionHyperParameters.Instance.m_CellBirthForce * energyReducingK;
            m_RigidBody.AddForce(transform.right * jumpForce);
        }

        protected void Steal(float amount)
        {
            m_Energy -= amount;
        }
    }
}