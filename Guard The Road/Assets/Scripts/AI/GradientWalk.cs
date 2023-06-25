using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class GradientWalk : MonoBehaviour, IFieldObserver, IMotionObserver, IProximityObserver
{
    
    [SerializeField] private ActionManager _manager;
    [SerializeField] private FieldObserver _field;
    [SerializeField] private MotionObserver _motion;
    [SerializeField] private ProximityObserver _proximity;

    [SerializeField] private float _speed = 4f;
    [SerializeField] private float _fieldUpdateTime = 0.1f;
    [SerializeField] private float _minimumWalkDistance = 0.5f;

    public FieldObserver Field{
        get=>_field;
        set=>_field = value;
    }

    public MotionObserver Motion{
        get=>_motion;
        set=>_motion = value;
    }

    public ProximityObserver Proximity{
        get=>_proximity;
        set=>_proximity = value;
    }
    
    public void ChangeMotion(){}


    void Start()
    {
        _speed = Random.Range(0.8f * _speed, 1.2f * _speed);
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

            if(_proximity.target != null)
            {
                if((_proximity.target.position - _motion.Position).magnitude <= _minimumWalkDistance){
                    
                }
            }


            yield return new WaitForSeconds(_fieldUpdateTime);

        }
    }
}
*/