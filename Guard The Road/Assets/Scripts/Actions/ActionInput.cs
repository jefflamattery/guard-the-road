using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionInput : MonoBehaviour
{
    [SerializeField] private List<ActionSlotObserver> _slots;

    public void OnSlot1()
    {
        _slots[0].Trigger();
    }
}
