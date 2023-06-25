using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
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

    
    public void ChangeMotion(){}
    
    void Start()
    {
        StartCoroutine(Walk());
    }

    IEnumerator Walk()
    {
        
        while(true)
        {
            // create a course that is a random deviation from a direct heading to the target
            _motion.course = Quaternion.Euler(0f, Random.Range(-_maximumAngleDeviation, _maximumAngleDeviation), 0f) * _target.position - _motion.position;
            _motion.courseSpeed = Random.Range(_meanSpeed / 2f, _meanSpeed * 1.5f);

            _motion.ChangeMotion();

            yield return new WaitForSeconds(Random.Range(0f, 2f * _meanWalkTime));

        }
    }
}
*/