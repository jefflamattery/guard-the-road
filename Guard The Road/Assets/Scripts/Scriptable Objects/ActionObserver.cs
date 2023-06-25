using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Action Observer")]
public class ActionObserver : Observer
{
    private ActionDefinition _active;
    private ActionDefinition _queued;
    private ActionDefinition _default;
    public ActionDefinition Default{
        set=>_default = value;
    }

    private bool _isQueued = false;

    void Awake()
    {
        _active = null;
        _queued = null;
        _isQueued = false;
    }

    public void Enqueue(ActionDefinition action)
    {
        if(action == null){return;}

        if(_active == null){
            StartAction(action);
        } else if(!_active.IsActing){
            StartAction(action);
        } else if(_active.CanInterrupt && !action.IsActing){
            _active.Interrupt();
            StartAction(action);
        } else {
            _queued = action;
            _isQueued = true;
        }
    }

    public void Interrupt(ActionDefinition action)
    {
        if(action == null){return;}

        if(_active != null){
            if(_active.IsActing){
                _active.Interrupt();
            }
        }

        _isQueued = false;
        StartAction(action);
    }

    public void Release()
    {
        if(_isQueued){
            StartAction(_queued);
            _isQueued = false;
        } else if(_default != null){
            StartAction(_default);
        }
    }

    void StartAction(ActionDefinition action)
    {
        _active = action;
        _active.StartAction();
    }

     public override Observer Clone()
    {
        ActionObserver clone = ScriptableObject.CreateInstance<ActionObserver>();
        return clone;
    }

    public override void Inject(GameObject agent)
    {
        
        IActionObserver[] slots = agent.GetComponentsInChildren<IActionObserver>();

        foreach(IActionObserver slot in slots)
        {
            slot.Action = this;
        }
        
    }
}
