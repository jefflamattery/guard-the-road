using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Motion Observer")]
public class MotionObserver : Observer
{
    public float courseSpeed = 0f;
    public float turningRate = 10f;
    
    public Vector3 position;
    public Vector3 course;
    public Vector3 heading = Vector3.forward;
    public float speed = 0f;
    public Vector3 Velocity{
        get=> speed * heading;
    }

    private bool _canMove;
    public bool CanMove{
        get=>_canMove;
    }
    private RigidbodyController _listener;
    public RigidbodyController Listener{
        set=>_listener = value;
    }

    public void EnableMovement()
    {
       _canMove = true;
       _listener.EnableMovement();
    }

    public void DisableMovement()
    {
        _canMove = false;
        _listener.DisableMovement();
    }
    

    public override Observer Clone()
    {
        MotionObserver clone = ScriptableObject.CreateInstance<MotionObserver>();

        clone.courseSpeed = courseSpeed;
        clone.position = position;
        clone.course = course;
        clone.heading = heading;
        clone.speed = speed;

        return clone;
    }

    public override void Inject(GameObject agent)
    {
        IMotionObserver[] slots = agent.GetComponentsInChildren<IMotionObserver>();

        foreach(IMotionObserver slot in slots)
        {
            slot.Motion = this;
        }
    }

}
