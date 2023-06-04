using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardListener : MonoBehaviour, IMotionObserver
{
    [SerializeField] private MotionObserver _motion;
    [SerializeField] private float _cursorErrorRadius = 0.2f;
    private bool _isMoving = false;
    public MotionObserver Motion{
        get=>_motion;
        set=>_motion = value;
    }

    void OnStartMove()
    {
        _isMoving = true;
        StartCoroutine(UpdateHeading());

    }

    void OnStopMove()
    {
        _isMoving = false;
        _motion.Course = Vector3.zero;
    }


    IEnumerator UpdateHeading()
    {
        Vector2 mousePosition;
        Vector3 characterPosition = Vector3.zero;
        Vector3 course;

        while(_isMoving){

            mousePosition = Mouse.current.position.ReadValue();
            characterPosition = Camera.main.WorldToScreenPoint(_motion.Position);
            course = new Vector3(mousePosition.x - characterPosition.x, 0f, mousePosition.y - characterPosition.y);
            
            // if the course magnitude is less than cursor error, then this is effectively a stop command
            if(course.magnitude <= _cursorErrorRadius){
                _motion.Course = Vector3.zero;
            } else {
                course.Normalize();
                _motion.Course = course;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    
}
