using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDummy : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger hit!!");
    }
}
