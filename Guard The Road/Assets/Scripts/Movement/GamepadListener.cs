using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadListener : MonoBehaviour, IMotionObserver
{
    [SerializeField] private MotionObserver _motion;

    
    public MotionObserver Motion{
        get=>_motion;
        set=>_motion = value;
    }

    void OnStickMove(InputValue value)
    {
        Vector2 stickHeading = value.Get<Vector2>();
       _motion.Course = new Vector3(stickHeading.x, 0f, stickHeading.y);
    }


}
