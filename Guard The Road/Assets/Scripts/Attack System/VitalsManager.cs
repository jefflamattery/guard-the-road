using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VitalsManager : MonoBehaviour, IVitalsObserver
{
    [SerializeField] private VitalsObserver _vitals;
    public VitalsObserver Vitals{
        get=>_vitals;
        set=>_vitals = value;
    }

    

    
}
