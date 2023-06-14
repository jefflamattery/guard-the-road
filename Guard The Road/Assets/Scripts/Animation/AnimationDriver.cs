using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDriver : MonoBehaviour, IAnimationObserver
{
    [SerializeField] private Animator _animator;
    [SerializeField] private AnimationObserver _animation;
    public AnimationObserver Animation{
        get=>_animation;
        set=>_animation = value;
    }

    void Start()
    {
        _animation.Assign(this);
    }

    public void TriggerAnimation(string name)
    {
        _animator.SetTrigger(name);
        
    }

    public void SetState(string name, bool value)
    {
        _animator.SetBool(name, value);
    }

    public void SetFloat(string name, float value)
    {
        _animator.SetFloat(name, value);
    }



    
    
}
