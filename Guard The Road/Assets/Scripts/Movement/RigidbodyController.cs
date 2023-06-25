using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyController : MonoBehaviour, IMotionObserver, IAgentObserver, IAnimationObserver, IFieldObserver
{

    [SerializeField] private FieldObserver _field;
    public FieldObserver Field{
        get=>_field;
        set=>_field = value;
    }
    [SerializeField] private AnimationObserver _animation;
    public AnimationObserver Animation{
        get=>_animation;
        set=>_animation = value;
    }
    [SerializeField] private MotionObserver _motion;
    public MotionObserver Motion{
        get=>_motion;
        set=>_motion = value;
    }

    [SerializeField] private AgentObserver _agent;
    public AgentObserver Agent{
        get=>_agent;
        set=>_agent = value;
    }

    [SerializeField] private Rigidbody _root;
    [SerializeField] private string _moveParameter = "isRunning";
    [SerializeField] private string _speedParameter = "runningSpeed";

    private Coroutine _moveHandle;

    void Start()
    {
        _motion.heading = _root.rotation * Vector3.forward;
        _motion.position = _root.transform.position;
        _motion.Listener = this;

        EnableMovement();
    }

    public void EnableMovement()
    {
       

        if(_moveHandle != null){StopCoroutine(_moveHandle);}

        _moveHandle = StartCoroutine(Move());
    }

    public void DisableMovement()
    {
        if(_moveHandle != null){
            StopCoroutine(_moveHandle);
            _moveHandle = null;
        }
        FreezeMovement();
    }

    void FreezeMovement()
    {
        _animation.SetState(_moveParameter, false);
        _root.constraints = RigidbodyConstraints.FreezeAll;
        _root.velocity = Vector3.zero;
        _root.angularVelocity = Vector3.zero;
    }

    IEnumerator Move()
    {
        Vector3 course, heading;
        float speed;

        while(true)
        {
            course = _motion.course;

            if(course.sqrMagnitude > 0f) {
                _root.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
               
                course.Normalize();
                
                // the angle that the motor should turn depends on which direction the heading would need to rotate to line up with the course.
                // that angle is the orthogonality of heading and course, multiplied by some small constant angle turningRate
               heading = Quaternion.Euler(0f, _motion.turningRate * Vector3.Dot(Vector3.Cross(_motion.heading, course), Vector3.up), 0f) * _motion.heading;
               speed = _motion.courseSpeed;
                
                _animation.SetState(_moveParameter, true);
                _animation.SetFloat(_speedParameter, speed);

                _root.AddForce(speed * heading - _root.velocity, ForceMode.VelocityChange);
                _root.MoveRotation(Quaternion.FromToRotation(Vector3.forward, heading));

                _motion.position = _root.transform.position;
                _agent.position = _root.transform.position;
                _field.position = _root.transform.position;
                _motion.heading = _root.rotation * Vector3.forward;
                _motion.speed = speed;
            } else {
                FreezeMovement();
            }

            yield return new WaitForFixedUpdate();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("rigidbody collision enter");
    }

}
