using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : ActionDefinition
{
    [SerializeField] private MotionObserver _motion;
    [SerializeField] private AnimationObserver _animation;
    [SerializeField] private float _turningRate;
    [SerializeField] private string _runningParameter;
    [SerializeField] private string _speedParameter;

    public override void Awake()
    {
        // this is a continuous action and is not triggered by an action slot
        // start this action right away
        Manager.SetDefault(this);
    }

    public override void Interrupt()
    {
        _motion.Speed = 0f;
        _animation.SetState(_runningParameter, false);
    }

    public override void TriggerAction()
    {
        Manager.SetDefault(this);
    }

    public override IEnumerator Action()
    {
        Vector3 heading, course;


        while(true)
        {
            heading = _motion.Heading;
            course = _motion.Course;

            // convert a course into a velocity
            if(course.magnitude > 0f){

                course.Normalize();
                
                // the angle that the motor should turn depends on which direction the heading would need to rotate to line up with the course.
                // that angle is the orthogonality of heading and course, multiplied by some small constant angle turningRate
                _motion.Heading = Quaternion.Euler(0f, _turningRate * Vector3.Dot(Vector3.Cross(heading, course), Vector3.up), 0f) * heading;
                _motion.Speed = _motion.CourseSpeed;

                _animation.SetState(_runningParameter, true);
                _animation.SetFloat(_speedParameter, _motion.Speed);

            } else {
                _motion.Speed = 0f;
                _animation.SetState(_runningParameter, false);

            }

            yield return new WaitForFixedUpdate();
        }
    }



    
}
