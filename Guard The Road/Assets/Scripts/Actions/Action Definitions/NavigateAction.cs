using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigateAction : ActionDefinition, IFieldObserver, IMotionObserver
{
    [SerializeField] private FieldObserver _field;
    public FieldObserver Field{
        get=>_field;
        set=>_field = value;
    }

    [SerializeField] private MotionObserver _motion;
    public MotionObserver Motion{
        get=>_motion;
        set=>_motion = value;
    }

    [SerializeField] private float _minimumFieldStrength;
    [SerializeField] private float _speed;

    public override IEnumerator Act()
    {
        CanInterrupt = true;
        _motion.EnableMovement();
        while(true)
        {

            _motion.courseSpeed = _speed;

            _motion.course = _field.vectorField;

            yield return new WaitForFixedUpdate();
        }
    }
}
