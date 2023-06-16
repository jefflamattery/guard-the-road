using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Action Slot")]
public class ActionSlot : ScriptableObject
{
    private List<ActionDefinition> _listeners;

    public bool isTriggered;
    
    void Awake()
    {
        _listeners = new List<ActionDefinition>();
    }

    public void Assign(ActionDefinition action)
    {
        _listeners.Add(action);
    }

    public void Remove(ActionDefinition action)
    {
        _listeners.Remove(action);
    }

    public void Trigger(){
        isTriggered = true;
        foreach(ActionDefinition listener in _listeners)
        {
            listener.TriggerAction();
        }
    }
}
