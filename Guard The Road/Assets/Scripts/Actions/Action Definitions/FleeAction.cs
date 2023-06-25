using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeAction : ActionDefinition
{
    public override IEnumerator Act()
    {
        Debug.Log("Flee!");

        return default;
    }
}
