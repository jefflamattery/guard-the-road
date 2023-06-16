using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : ScriptableObject
{
    public virtual Observer Clone()
    {
        return null;
    }

    public virtual void Inject(GameObject agent){}
    
}
