using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace SL.Scripts
{
    public class Cell: MonoBehaviour
    {
        [SerializeField] private GameObject CellPrefab;
        [SerializeField] private SpriteRenderer _body;

        [ReadOnly]
        [SerializeField]
        private Genome _genome;
        
        [SerializeField]
        [ReadOnly]
        private float _energy;
        
        private Rigidbody2D _rigidbody;
        private Vector3 _lastPosition;
        
        private CellPhase _phase = CellPhase.Initiation;
        private float _energyFine = 1.1f;
        private float _dieEnergy = 10f;
        
        private float _coldDownTime = 0;

        private float _scaleMultiplier = 1f;
        private float _energyPerUnit = 100f;

        private float _timeToBirtMinValue = .1f; //for coloring, R channel
        private float _birthSizeMinValue = 80f; //for coloring, G channel
        private float _birthForceMinValue = 25f;//for coloring, B channel
        
        private float _timeToBirtMaxValue = .6f; //for coloring, R channel
        private float _birthSizeMaxValue = 120f; //for coloring, G channel
        private float _birthForceMaxValue = 40f; //for coloring, B channel

        private void Awake()
        {
            _rigidbody = GetComponentInParent<Rigidbody2D>();
        }
        
        void Start()
        {
            _lastPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            transform.Rotate(Vector3.forward, Random.Range(0, 359));
        }
        
        void Update()
        {
            float scale = 1f;
            if (_phase != CellPhase.Initiation)
            {
                scale = (Mathf.Sqrt((_energy / _energyPerUnit) / Mathf.PI) * 2f)* _scaleMultiplier;
                _energy -= _energyFine * (_energy / _energyPerUnit);
                if (_energy < _dieEnergy)
                {
                    Destroy(gameObject);
                    return;
                }
                
                transform.localScale = new Vector3(scale, scale, 1);
            }

            if (_phase.Equals(CellPhase.Birth))
            {
                _rigidbody.AddForce(transform.up * _genome.BirthForce);
                _energy -= _genome.BirthForce;

                // _body.color = new Color(
                //     _genome.TimeToBirth / _timeToBirtMaxValue, 
                //     _genome.BirthSize / _birthSizeMaxValue, 
                //     _genome.BirthForce / _birthForceMaxValue);

                float r = (_genome.TimeToBirth - _timeToBirtMinValue) / (_timeToBirtMaxValue - _timeToBirtMinValue);
                float g = (_genome.BirthSize - _birthSizeMinValue) / (_birthSizeMaxValue - _birthSizeMinValue);
                float b = (_genome.BirthForce - _birthForceMinValue) / (_birthForceMaxValue - _birthForceMinValue);
                float[] channels = new[] {r, g, b};
                float max = Mathf.Max(channels);
                
                _body.color = new Color(r/max, g/max, b/max);
                
                _phase = CellPhase.Life;
                return;
            }
            
            if (_phase.Equals(CellPhase.Life))
            {
                _energy += GetLightVolume() * (_rigidbody.velocity.magnitude) * (_energy/_energyPerUnit);
                //if ((_lastPosition - transform.position).magnitude == 0f)
                if (_rigidbody.velocity.magnitude < 1f)
                {
                    _coldDownTime += Time.deltaTime;
                    if (_coldDownTime > _genome.TimeToBirth)
                    {
                        _phase = CellPhase.Dividing;
                        return;
                    }
                }
                
                _lastPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            }

            if (_phase.Equals(CellPhase.Dividing))
            {
                Divide();
                Destroy(gameObject);
            }
        }
        

        private void Divide()
        {
            float devidingEffectivity = (_genome.TimeToBirth * 7) / (_energy/_energyPerUnit);
            devidingEffectivity = devidingEffectivity > 1 ? 1 : devidingEffectivity;
            _energy = _energy * devidingEffectivity;
            
            int ChildrenQuantity = (int)_energy / (int)_genome.BirthSize;

            for (int i = 0; i < ChildrenQuantity; i++)
            {
                Cell cell = Instantiate(CellPrefab, transform.position, Quaternion.identity).GetComponent<Cell>();
                cell.SetGenome(_genome.Clone().Mutate());
                cell.SetEnergy(_energy/ChildrenQuantity);
            }
        }

        public void SetGenome(Genome genome)
        {
            _genome = genome;
            _energy = _genome.BirthSize;
            
            _phase = CellPhase.Birth;
        }

        public Genome GetGenome()
        {
            return _genome;
        }

        public void SetEnergy(float energy)
        {
            _energy = energy;
        }

        private float GetLightVolume()
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            return Environment.Instance.GetColor(screenPos.x, screenPos.y).a;
        }
    }
}