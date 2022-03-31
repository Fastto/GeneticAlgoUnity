using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cell : MonoBehaviour
{
    [SerializeField] private GameObject CellPrefab;

    private Rigidbody2D _rigidbody;
    private Vector3 _lastPosition;
    private float _stayTime = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        _lastPosition = transform.position;
        _rigidbody = GetComponentInParent<Rigidbody2D>();
        
        transform.Rotate(Vector3.forward, Random.Range(0,359));
        _rigidbody.AddForce(transform.up * 30);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if ((_lastPosition - transform.position).magnitude == 0)
        {
            _stayTime += Time.deltaTime;

            if (_stayTime > 1)
            {
                Devide();
                Destroy(gameObject);
            }
        }
        _lastPosition = transform.position;
    }

    private void Devide()
    {
        for (int i = 0; i < 5; i++)
        {
            Instantiate(CellPrefab, transform.position, Quaternion.identity);
        }
    }
}
