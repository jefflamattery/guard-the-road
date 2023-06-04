using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyController : MonoBehaviour, IMotionObserver
{
    [SerializeField] private Rigidbody _root;
    [SerializeField] private MotionObserver _motion;
    
    public MotionObserver Motion{
        get=>_motion;
        set=>_motion = value;
    }

    void FixedUpdate()
    {
        // update the information about the rigid body's actual motion
        _motion.Position = _root.transform.position;

        // move the rigidbody
        _root.velocity = _motion.Velocity;
        _root.angularVelocity = Vector3.zero;

        // ensure proper rotation of the rigidbody
        _root.transform.rotation = Quaternion.FromToRotation(Vector3.forward, _motion.Heading);
    }

}
