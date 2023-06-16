using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Field Observer")]
public class FieldObserver : Observer
{
    public Vector3 Position;
    public float Charge;
    public float ScalarField;
    public Vector3 VectorField;
    public NavNode lastNode;



    public override Observer Clone()
    {
        FieldObserver clone = ScriptableObject.CreateInstance<FieldObserver>();

        clone.Position = Position;
        clone.Charge = Charge;
        clone.ScalarField = ScalarField;
        clone.VectorField = VectorField;
        clone.lastNode = lastNode;

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
