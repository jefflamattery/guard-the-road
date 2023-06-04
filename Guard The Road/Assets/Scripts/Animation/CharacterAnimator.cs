using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour, IMotionObserver
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string _runParameter;
    [SerializeField] private string _speedParameter;

    [SerializeField] private MotionObserver _motion;

    public MotionObserver Motion{
        get=>_motion;
        set=>_motion = value;
    }

    private bool _isRunning = false;

    void FixedUpdate()
    {
        if(_motion.Speed == 0f)
        {
            if(_isRunning){
                _animator.SetBool(_runParameter, false);
                _isRunning = false;
            }
        } else {
            if(!_isRunning){
                _animator.SetBool(_runParameter, true);
                _isRunning = true;
            }

            _animator.SetFloat(_speedParameter, _motion.Speed);
        }

        
    }
}
