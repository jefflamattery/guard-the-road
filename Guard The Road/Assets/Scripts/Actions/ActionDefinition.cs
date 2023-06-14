using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionDefinition : MonoBehaviour
{
    [SerializeField] private ActionSlotObserver _slot;
    public ActionSlotObserver Slot{
        set{
            _slot = value;
            _slot.Assign(this);
        }
        get=>_slot;
    }
    private ActionManager _manager;
    public ActionManager Manager{
        set=>_manager = value;
        get=>_manager;
    }

    public virtual void Awake()
    {
        if(_slot != null){
            _slot.Assign(this);
        }
    }

    public virtual IEnumerator Action()
    {
        return null;
    }

    public virtual void PreAction(){}
    public virtual void PostAction(){}

    public virtual void TriggerAction()
    {
        Manager.Enqueue(this);
    }

    public virtual void Interrupt(){}

}