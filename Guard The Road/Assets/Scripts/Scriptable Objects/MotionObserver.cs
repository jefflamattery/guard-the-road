using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Motion Observer")]
public class MotionObserver : Observer
{
    public float CourseSpeed = 0f;
    
    public Vector3 Position;
    public Vector3 Course;
    public Vector3 Heading = Vector3.forward;
    public float Speed = 0f;
  

    public Vector3 Velocity{
        get=> Speed * Heading;
    }

    public override Observer Clone()
    {
        MotionObserver clone = ScriptableObject.CreateInstance<MotionObserver>();

        clone.CourseSpeed = CourseSpeed;
        clone.Position = Position;
        clone.Course = Course;
        clone.Heading = Heading;
        clone.Speed = Speed;

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
