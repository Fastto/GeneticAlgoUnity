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
       // private Vector3 _lastPosition;
        
        private CellPhase _phase = CellPhase.Initiation;
        private float _energyFine = .4f;
        private float _dieEnergy = 10f;
        
        private float _coldDownTime = 0;

        private float _scaleMultiplier = 1f;
        private float _energyPerUnit = 100;

        private float _timeToBirtMinValue = .1f; //for coloring, R channel
        private float _birthSizeMinValue = 80f; //for coloring, G channel
        private float _birthForceMinValue = 25f;//for coloring, B channel
        
        private float _timeToBirtMaxValue = .6f; //for coloring, R channel
        private float _birthSizeMaxValue = 120f; //for coloring, G channel
        private float _birthForceMaxValue = 40f; //for coloring, B channel

        //private Vector3 prevPos;

        public Action<Cell> OnDie;
        
        private void Awake()
        {
            _rigidbody = GetComponentInParent<Rigidbody2D>();
        }

        private List<Cell> children = new List<Cell>();
        
        void Start()
        {
           // _lastPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
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
                    Kill();
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

                // float r = (_genome.TimeToBirth - _timeToBirtMinValue) / (_timeToBirtMaxValue - _timeToBirtMinValue);
                // float g = (_genome.BirthSize - _birthSizeMinValue) / (_birthSizeMaxValue - _birthSizeMinValue);
                // float b = (_genome.BirthForce - _birthForceMinValue) / (_birthForceMaxValue - _birthForceMinValue);
                //
                
                float r = (_genome.TimeToBirth - Environment.Instance.TimeToBirtMinValue) / (Environment.Instance.TimeToBirtMaxValue - Environment.Instance.TimeToBirtMinValue);
                float g = (_genome.BirthSize - Environment.Instance.BirthSizeMinValue) / (Environment.Instance.BirthSizeMaxValue - Environment.Instance.BirthSizeMinValue);
                float b = (_genome.BirthForce - Environment.Instance.BirthForceMinValue) / (Environment.Instance.BirthForceMaxValue - Environment.Instance.BirthForceMinValue);

                
                // float[] channels = new[] {r, g, b};
                // float max = Mathf.Max(channels);
                //
                // _body.color = new Color(r/max, g/max, b/max);
                _body.color = new Color(r, g, b);
                
                _phase = CellPhase.Life;

               // prevPos = transform.position;
                return;
            }
            
            if (_phase.Equals(CellPhase.Life))
            {
               // _energy += GetLightVolume() * (_rigidbody.velocity.magnitude);// * (_energy/_energyPerUnit);
                //_energy += GetLightVolume() * (prevPos - transform.position).magnitude * 100;// * (_energy/_energyPerUnit);
                _energy += GetLightVolume() * (_energy/_energyPerUnit);
                //_energy += .1f;
                //if ((_lastPosition - transform.position).magnitude == 0f)
                // if ((prevPos - transform.position).magnitude <= 0f)
                // //if (_rigidbody.velocity.magnitude < 1f)
                // {
                //     _coldDownTime += Time.deltaTime;
                //     if (_coldDownTime > _genome.TimeToBirth)
                //     {
                //         _phase = CellPhase.Dividing;
                //         return;
                //     }
                // }

                if ((int) _energy / (int) _genome.BirthSize > 2)
                {
                    Divide();
                    //return;
                }

                if (children.Count > 0)
                {
                    List<Cell> toKill = new List<Cell>();
                    Vector3 pos = transform.position;
                    children.ForEach(item =>
                    {
                        Vector3 cPos = item.transform.position;
                        float mag = (cPos - pos).magnitude;

                        if (mag > 2)
                        {
                            toKill.Add(item);
                        }

                        if (mag < 0.9 || mag > 1.1)
                        {
                            Vector3 dirToParent = (pos - cPos).normalized;
                            float dMag = 1 - mag;
                            dirToParent *= dMag > 0 ? -1 : 1;
                            
                            Vector3 newCPos = Vector3.Lerp(cPos, cPos + dirToParent, .25f);
                            Vector3 newPos = Vector3.Lerp(pos, pos - dirToParent, .25f / children.Count);
                            item.transform.position = newCPos;
                            
                            Debug.Log("Mag " + mag + " pos " + pos + " cpos " + cPos);
                            Debug.Log("DirToP " + dirToParent + " dmag " + dMag);
                            Debug.Log("newCPos " + newCPos);

                            pos = newPos;
                        }
                    });

                    transform.position = pos;

                    foreach (var item in toKill)
                    {
                        item.Kill();
                    }
                }
                
               // _lastPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

                //prevPos = transform.position;
            }

            // if (_phase.Equals(CellPhase.Dividing))
            // {
            //     Divide();
            //     _phase = CellPhase.Life;
            //     //Destroy(gameObject);
            // }
        }

        public void Kill()
        {
            OnDie?.Invoke(this);
            Destroy(gameObject);
        }

        private void Divide()
        {
            float devidingEffectivity = (_genome.TimeToBirth * 7) / (_energy/_energyPerUnit);
            devidingEffectivity = devidingEffectivity > 1 ? 1 : devidingEffectivity;
            //_energy = _energy * devidingEffectivity;
            
            int ChildrenQuantity = (int)_energy / (int)_genome.BirthSize;

            if (ChildrenQuantity > 1)
            {
                for (int i = 0; i < ChildrenQuantity; i++)
                {
                    if ((int)_energy / (int)_genome.BirthSize <= 1)
                        break;
                    
                    Cell cell = Instantiate(CellPrefab, transform.position, Quaternion.identity).GetComponent<Cell>();
                    cell.SetGenome(_genome.Clone().Mutate());
                    cell.SetEnergy(_energy / ChildrenQuantity);

                    _energy -= _energy / ChildrenQuantity;
                    cell.OnDie += OnDieHandler;
                    
                    children.Add(cell);
                    

                    // gameObject.AddComponent(typeof(SpringJoint2D));
                    // SpringJoint2D joint = gameObject.GetComponent<SpringJoint2D>();
                    // joint.distance = 2;
                    // //joint.enableCollision = true;
                    // //joint.dampingRatio = 1;
                    // joint.autoConfigureDistance = false;
                    // joint.connectedBody = cell._rigidbody;
                }
            }
        }

        private void OnDieHandler(Cell cell)
        {
            if (children.Contains(cell))
            {
                children.Remove(cell);
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