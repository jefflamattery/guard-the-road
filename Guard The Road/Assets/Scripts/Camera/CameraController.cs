using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Rigidbody _root;

    private Vector3 _initialDisplacement;

    void Start()
    {
        _initialDisplacement = transform.position - _root.transform.position;
    }
    void FixedUpdate()
    {
        transform.position = _initialDisplacement + _root.transform.position;
    }
}
