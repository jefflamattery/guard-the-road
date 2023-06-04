using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldInteraction : MonoBehaviour, IMotionObserver, IFieldObserver
{
    [SerializeField] private FieldObserver _field;

    [SerializeField] private MotionObserver _motion;
    
    public MotionObserver Motion{
        get=>_motion;
        set=>_motion = value;
    }
    
    public FieldObserver Field{
        get=>_field;
        set=>_field = value;
    }

    void FixedUpdate()
    {
        // set the position of the observer based on the position of the motion
        _field.Position = _motion.Position;
    }

}
