using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchAction : ActionDefinition
{
    [SerializeField] private AnimationObserver _animation;
    [SerializeField] private string _punchParameter = "punch";
    [SerializeField] private string _attackSpeedParameter = "attackSpeed";
    [SerializeField] private float _attackSpeed = 1f;
    public override IEnumerator Action()
    {
        _animation.SetFloat(_attackSpeedParameter, _attackSpeed);
        _animation.TriggerAnimation(_punchParameter);
        yield return new WaitForSeconds(1f / _attackSpeed);
    }



}
