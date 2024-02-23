using System;
using System.Collections;
using System.ComponentModel;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Evolution.Scripts
{
    public class Cell : MonoBehaviour
    {
        [HideInInspector] public Rigidbody2D m_RigidBody;
        public SpriteRenderer m_Body;
        
        [HideInInspector] public float m_BirthTime;
        [HideInInspector] public float m_Energy;
        [HideInInspector] public DNA m_DNA;
        [HideInInspector] public bool m_IsDead;

        public Action<Cell> OnDied;
        public Action<Cell> OnBirth;
        public Action<Cell> OnFrame;
        public Action<Cell> OnDivided;

        protected void Start()
        {
            m_RigidBody = GetComponent<Rigidbody2D>();
        }

        protected void OnEnable()
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
            if (m_DNA == null)
            {
                m_DNA = DNA.GetRandom();
            }
            
            m_DNA.Activate(this);
            OnBirth?.Invoke(this);
            yield return null;
        }
        
        protected IEnumerator Life()
        {
            while (!m_IsDead)
            {
                OnFrame?.Invoke(this);
                yield return null;
            }
        }
        
        protected IEnumerator Death()
        {
            OnDied?.Invoke(this);
            m_DNA.Deactivate(this);
            gameObject.SetActive(false);
            
            yield return null;
        }

        public void SetDNA(DNA dna)
        {
            m_DNA = dna;
        }
        
        public DNA GetDNA()
        {
            return m_DNA;
        }
    }
}