using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Motion Observer")]
public class MotionObserver : ScriptableObject
{
    
    private Vector3 _position = Vector3.zero;
    private Vector3 _course = Vector3.zero;
    private Vector3 _heading = Vector3.forward;
    private float _speed = 0f;

    public Vector3 Position{
        get=>_position;
        set=>_position = value;
    }

    public Vector3 Course{
        get=>_course;
        set=>_course = value;
    }

    public Vector3 Heading{
        get=>_heading;
        set=>_heading = value;
    }

    public float Speed{
        get=>_speed;
        set=>_speed = value;
    }

    public Vector3 Velocity{
        get=> _speed * _heading;
    }






}
