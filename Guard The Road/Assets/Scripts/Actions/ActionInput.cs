using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActionInput : MonoBehaviour, IActionObserver
{
  
    [SerializeField] private ActionObserver _action;
    public ActionObserver Action{
        get=>_action;
        set=>_action = value;
    }
    [SerializeField] private ActionDefinition _slot1;
    [SerializeField] private ActionDefinition _slot2;
    [SerializeField] private ActionDefinition _slot3;
    [SerializeField] private ActionDefinition _slot4;



    public void OnSlot1()
    {
        _action.Enqueue(_slot1);
    }

    public void OnSlot2(InputValue value)
    {
        _action.Enqueue(_slot2);
    }

    public void OnSlot3(InputValue value)
    {
        _action.Enqueue(_slot3);
    }

    public void OnSlot4(InputValue value)
    {
        _action.Enqueue(_slot4);
    }
}
