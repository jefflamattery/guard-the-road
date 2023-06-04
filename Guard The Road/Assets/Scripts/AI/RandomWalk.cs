using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWalk : MonoBehaviour, IMotionObserver
{
    [SerializeField] private MotionObserver _motion;
    [SerializeField] private float _maximumAngleDeviation;
    [SerializeField] private float _meanWalkTime;

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
            // create a course that is a random deviation from the heading
            _motion.Course = Quaternion.Euler(0f, Random.Range(-_maximumAngleDeviation, _maximumAngleDeviation), 0f) * _motion.Heading;

            yield return new WaitForSeconds(Random.Range(0f, 2f * _meanWalkTime));

        }
    }
}
