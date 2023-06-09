using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Field Observer")]
public class FieldObserver : ScriptableObject
{
    public Vector3 Position;
    public float Charge;
    public float ScalarField;
    public Vector3 VectorField;
    public NavNode lastNode;
    
}
