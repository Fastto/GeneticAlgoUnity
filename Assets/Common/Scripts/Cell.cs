using System;
using System.Collections;
using System.Collections.Generic;
using Common.Scripts.CellParams;
using Evolution.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Common.Scripts
{
    public class Cell : MonoBehaviour
    {
        public SpriteRenderer m_Body;
        
        public Dictionary<CellFloatParams, float> m_FloatParams = new Dictionary<CellFloatParams, float>();
        public Dictionary<CellBoolParams, bool> m_BoolParams = new Dictionary<CellBoolParams, bool>();
        
        [HideInInspector] public CellGenome m_CellGenome;
        [HideInInspector] public bool m_IsDead;

        public Action<Cell> OnDied;
        public Action<Cell> OnBirth;
        public Action<Cell> OnFrame;
        public Action<Cell> OnFixedFrame;
        public Action<Cell> OnRareFrame;
        public Action<Cell> OnDivided;
        
        protected Rigidbody2D m_RigidBody;

        private void Start()
        {
            m_FloatParams = CellParams.CellParams.GetFloatParams();
            m_BoolParams = CellParams.CellParams.GetBoolParams();
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
            if (m_CellGenome == null)
            {
                m_CellGenome = CellFactory.Instance.GetRandomGenome();
            }
            
            m_CellGenome.Activate(this);
            OnBirth?.Invoke(this);
            yield return null;
        }
        
        protected IEnumerator Life()
        {
            StartCoroutine(FixedFrameLife());
            StartCoroutine(RareFrameLife());
            while (!m_IsDead)
            {
                OnFrame?.Invoke(this);
                yield return null;
            }
        }

        protected IEnumerator FixedFrameLife()
        {
            while (!m_IsDead)
            {
                OnFixedFrame?.Invoke(this);
                yield return new WaitForFixedUpdate();
            }
        }
        
        protected IEnumerator RareFrameLife()
        {
            while (!m_IsDead)
            {
                OnRareFrame?.Invoke(this);
                yield return new WaitForSeconds(EvolutionCommonHyperParameters.Instance.m_RareFramePeriod);
            }
        }
        
        protected IEnumerator Death()
        {
            OnDied?.Invoke(this);
            m_CellGenome.Deactivate(this);
            gameObject.SetActive(false);
            
            yield return null;
        }

        public void SetCellGenome(CellGenome cellGenome)
        {
            this.m_CellGenome = cellGenome;
        }
        
        public CellGenome GetCellGenome()
        {
            return m_CellGenome;
        }
        
        public Rigidbody2D GetRigidbody2D()
        {
            return m_RigidBody;
        }
        
        public void SetRigidbody2D(Rigidbody2D rigidbody2D)
        {
            m_RigidBody = rigidbody2D;
        }
    }
}