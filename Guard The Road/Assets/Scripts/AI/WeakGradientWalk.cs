using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakGradientWalk : MonoBehaviour, IFieldObserver, IMotionObserver
{
    [SerializeField] private FieldObserver _field;
    [SerializeField] private MotionObserver _motion;
    [SerializeField] private float _meanWalkTime;
    [SerializeField] private float _minimumWalkTime;
    [SerializeField] private float _fieldThreshold;

    [SerializeField] private float _wanderSpeed;
    [SerializeField] private float _chaseSpeed;

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


    IEnumerator Walk()
    {
        Vector2 randomStep;
        float randomWalkTime = 0f;

        while(true)
        {
            
            // update the course using the field, and update the field using the position
            _field.Position = _motion.Position;
            _motion.Course = _field.VectorField;
            _motion.CourseSpeed = _chaseSpeed;

            // if the vector field's magnitude does not meet the threshold, then random walk until the threshold is met (until the trail has been found again)
            while(Mathf.Abs(_field.ScalarField) < _fieldThreshold)
            {
                // the field position will need to be updated within any while loop
                _field.Position = _motion.Position;

                if(randomWalkTime <= 0f){
                    // time to choose a new direction to randomly walk in
                    randomStep = Random.insideUnitCircle;
                    _motion.Course = new Vector3(randomStep.x, 0f, randomStep.y);
                    randomWalkTime = Random.Range(_minimumWalkTime, 2f * _meanWalkTime - _minimumWalkTime);

                    // set the course speed to this agent's wandering speed
                    _motion.CourseSpeed = _wanderSpeed;

                } else {
                    // count down toward 0
                    randomWalkTime -= Time.fixedDeltaTime;
                }

                yield return new WaitForFixedUpdate();
            }

            yield return new WaitForFixedUpdate();

        }
    }
}
