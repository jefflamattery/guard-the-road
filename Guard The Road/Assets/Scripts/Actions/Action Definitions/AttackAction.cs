using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : ActionDefinition, IWeaponObserver, IAnimationObserver, IMotionObserver, IAgentObserver
{
    [SerializeField] private AgentObserver _agent;
    public AgentObserver Agent{
        get=>_agent;
        set=>_agent = value;
    }

    [SerializeField] private AnimationObserver _animation;
    public AnimationObserver Animation{
        get=>_animation;
        set=>_animation = value;
    }

    [SerializeField] private WeaponObserver _weapon;
    public WeaponObserver Weapon{
        get=>_weapon;
        set=>_weapon = value;
    }

    [SerializeField] private MotionObserver _motion;
    public MotionObserver Motion{
        get=>_motion;
        set=>_motion = value;
    }
    [SerializeField] private string _attackParameter = "swing";
    [SerializeField] private string _attackSpeedParameter = "attackSpeed";
    [SerializeField] private float _attackDistance = 1.5f;



    public override void PostAction()
    {
        
    }


    public override IEnumerator Act()
    {
        AgentReport report;

        _motion.DisableMovement();

        _animation.SetFloat(_attackSpeedParameter, _weapon.attackSpeed);
        _animation.TriggerAnimation(_attackParameter);

        yield return new WaitForSeconds(0.5f / _weapon.attackSpeed);

        report = _agent.FindAgents(_attackDistance);
        

        yield return new WaitForSeconds(0.5f / _weapon.attackSpeed);

        _motion.EnableMovement();


    }



}
