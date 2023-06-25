using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable Object/Vitals Observer")]
public class VitalsObserver : Observer
{
    public float hitpoints;
    public float maximumHitpoints;


    public override Observer Clone()
    {
        VitalsObserver clone = ScriptableObject.CreateInstance<VitalsObserver>();

        clone.hitpoints = hitpoints;
        clone.maximumHitpoints = maximumHitpoints;

        return clone;
    }

    public override void Inject(GameObject agent)
    {
        IVitalsObserver[] slots = agent.GetComponentsInChildren<IVitalsObserver>();

        foreach(IVitalsObserver slot in slots)
        {
            slot.Vitals = this;
        }
    }
    
}
