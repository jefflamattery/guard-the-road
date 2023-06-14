using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    private Coroutine _active;
    private ActionDefinition _default;
    private ActionDefinition _queued;

    private bool _isActing = false;
    private bool _isQueued = false;


    public void Enqueue(ActionDefinition definition)
    {
        _queued = definition;
        _isQueued = true;
        Resume();
    }

    public void SetDefault(ActionDefinition definition)
    {
        Pause();
        _default = definition;
        Resume();
    }

    private IEnumerator DiscreteAction(ActionDefinition definition)
    {
        Pause();
        _isActing = true;
        yield return StartCoroutine(definition.Action());
        _isActing = false;
        Resume();
    }

    private void Pause()
    {
        // continuous (default) actions must be paused from within their class (to take care of things like setting velocity to 0)
        if(!_isActing && _default != null){
            _default.Interrupt();
        }

        if(_active != null){
            StopCoroutine(_active);
        }
    }

    private void Resume()
    {
        // action can only be resumed if it isn't currently running
        if(!_isActing){
            if(_isQueued){
                _isQueued = false;
                _active = StartCoroutine(DiscreteAction(_queued));
            } else if(_default != null) {
                _active = StartCoroutine(_default.Action());
            }
        }
    }

    void Awake()
    {
        // find all Actions attached to this gameObject and give them a reference to this Action Manager
        ActionDefinition[] actions = GetComponentsInChildren<ActionDefinition>();

        foreach(ActionDefinition action in actions)
        {
            action.Manager = this;
        }
    }




}
