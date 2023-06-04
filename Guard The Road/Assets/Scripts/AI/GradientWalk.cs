using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradientWalk : MonoBehaviour, IFieldObserver, IMotionObserver
{
    [SerializeField] private FieldObserver _field;
    [SerializeField] private MotionObserver _motion;
    [SerializeField] private float _meanWalkTime;
    [SerializeField] private float _minimumWalkTime;

    public FieldObserver Field{
        get=>_field;
        set=>_field = value;
    }

    public MotionObserver Motion{
        get=>_motion;
        set=>_motion = value;
    }


    void Start()
    {
        StartCoroutine(Walk());
    }

    Vector3 RandomDeviation(Vector3 course, float meanAngleDeviation)
    {
        // TODO
        return course;
    }

    IEnumerator Walk()
    {
        while(true)
        {

            // update the course using the field, and update the field using the position
            _field.Position = _motion.Position;
            _motion.Course = _field.VectorField;

            //yield return new WaitForSeconds(Random.Range(_minimumWalkTime, 2f * (_meanWalkTime + _minimumWalkTime) ));
            yield return new WaitForFixedUpdate();

        }
    }
}
