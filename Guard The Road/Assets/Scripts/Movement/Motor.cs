using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motor : MonoBehaviour, IMotionObserver
{
    [SerializeField] private MotionObserver _motion;
    [SerializeField] private float _speed;
    [SerializeField] private float _turningRate;
    
    public MotionObserver Motion{
        get=>_motion;
        set=>_motion = value;
    }

    private Coroutine _action;

    void Start()
    {
        if(_action != null){
            StopCoroutine(_action);
        }

        _action = StartCoroutine(ExecuteCourse());
    }

    void Stop()
    {
        StopCoroutine(_action);
    }

    IEnumerator ExecuteCourse()
    {
        while(true)
        {
            // convert a course into a velocity
            if(_motion.Course.magnitude > 0f){
                
                // the angle that the motor should turn depends on which direction the heading would need to rotate to line up with the course.
                // that angle is the orthogonality of heading and course, multiplied by some small constant angle turningRate
                _motion.Heading = Quaternion.Euler(0f, _turningRate * Vector3.Dot(Vector3.Cross(_motion.Heading, _motion.Course), Vector3.up), 0f) * _motion.Heading;
                _motion.Speed = _speed;
            } else {
                _motion.Speed = 0f;
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
