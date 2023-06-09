using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardListener : MonoBehaviour, IMotionObserver
{
    [SerializeField] private MotionObserver _motion;
    [SerializeField] private float _cursorErrorRadius = 0.2f;
    private bool _isMoving = false;
    private float _initialCourseSpeed;
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
        _motion.Speed = 0f;
    }

    void Awake(){
        _initialCourseSpeed = _motion.CourseSpeed;
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
                _motion.CourseSpeed = 0f;
            } else {
                //course.Normalize();
                Debug.Log("setting course");
                _motion.Course = course;
                _motion.CourseSpeed = _initialCourseSpeed;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    
}
