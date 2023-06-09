using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Motion Observer")]
public class MotionObserver : ScriptableObject
{
    public float CourseSpeed = 0f;
    
    public Vector3 Position;
    public Vector3 Course;
    public Vector3 Heading = Vector3.forward;
    public float Speed = 0f;
  

    public Vector3 Velocity{
        get=> Speed * Heading;
    }

}
