using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionDefinition : MonoBehaviour, IActionObserver
{
    [SerializeField] private ActionObserver _action;
    
    public ActionObserver Action{
        get=>_action;
        set=>_action = value;
    }
    [SerializeField] private bool _canInterrupt = false;
    public bool CanInterrupt{
        get=>_canInterrupt;
        set=>_canInterrupt = value;
    }

    private bool _isActing;
    public bool IsActing{
        get=>_isActing;
    }

    private Coroutine _actionHandle;

    public void StartAction()
    {
        _actionHandle = StartCoroutine(ActionPlayer());
    }

    public void Interrupt()
    {
        if(_actionHandle != null){ StopCoroutine(_actionHandle); }

        PostAction();
        _isActing = false;
    }


    IEnumerator ActionPlayer()
    {
        _isActing = true;
        yield return StartCoroutine(Act());
        PostAction();
        _isActing = false;
        _action.Release();
    }

    public virtual IEnumerator Act()
    {
        return default;
    }

    public virtual void PostAction(){}

}
