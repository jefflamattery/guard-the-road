using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWalk : MonoBehaviour, IMotionObserver
{
    [SerializeField] private MotionObserver _motion;
    [SerializeField] private MotionObserver _target;
    [SerializeField] private float _maximumAngleDeviation;
    [SerializeField] private float _meanWalkTime;
    [SerializeField] private float _meanSpeed;

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
        
        while(true)
        {
            // create a course that is a random deviation from a direct heading to the target
            _motion.Course = Quaternion.Euler(0f, Random.Range(-_maximumAngleDeviation, _maximumAngleDeviation), 0f) * _target.Position - _motion.Position;
            _motion.CourseSpeed = Random.Range(_meanSpeed / 2f, _meanSpeed * 1.5f);

            yield return new WaitForSeconds(Random.Range(0f, 2f * _meanWalkTime));

        }
    }
}
