using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Field Observer")]
public class FieldObserver : Observer
{
    public Vector3 position;
    public float charge;
    public float scalarField;
    public Vector3 vectorField;
    public NavNode lastNode;



    public override Observer Clone()
    {
        FieldObserver clone = ScriptableObject.CreateInstance<FieldObserver>();

        clone.position = position;
        clone.charge = charge;
        clone.scalarField = 0f;
        clone.vectorField = Vector3.zero;
        clone.lastNode = null;

        return clone;
    }


    public override void Inject(GameObject agent)
    {
        IFieldObserver[] slots = agent.GetComponentsInChildren<IFieldObserver>();

        foreach(IFieldObserver slot in slots)
        {
            slot.Field = this;
        }
    }
    
}
