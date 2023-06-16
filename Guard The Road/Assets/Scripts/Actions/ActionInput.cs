using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActionInput : MonoBehaviour
{
    [SerializeField] private List<ActionSlot> _slots;

    public void OnSlot1(InputValue value)
    {
        if(value.isPressed){
            _slots[0].Trigger();
        } else {
            _slots[0].isTriggered = false;
        }
        
    }
}
