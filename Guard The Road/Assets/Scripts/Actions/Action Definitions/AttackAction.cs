using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : ActionDefinition, IWeaponObserver, IAnimationObserver
{
    [SerializeField] private GameObject _attackRangeSphere;
 
    [SerializeField] private string _attackParameter = "swing";
    [SerializeField] private string _attackSpeedParameter = "attackSpeed";
    [SerializeField] private float _attackSpeed = 1f;
    [SerializeField] private string _targetTag;

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

    private float _attackRangeSquared;

    void Start()
    {
        _attackRangeSquared = Mathf.Pow(_attackRangeSphere.transform.localScale.x, 2f);
    }

    public override void PostAction()
    {
        if(Slot.isTriggered){
            // if the player is holding down the button for this slot, then enqueue it again
            TriggerAction();
        }
    }


    public override IEnumerator Action()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(_targetTag);
        GameObject closestTarget = null;
        float closestMagnitude = Mathf.Infinity;
        float squareMagnitude;
        IAttackable lockedOnTarget;


        _animation.SetFloat(_attackSpeedParameter, _attackSpeed);
        _animation.TriggerAnimation(_attackParameter);

        yield return new WaitForSeconds(0.5f / _attackSpeed);


        foreach (GameObject target in targets)
        {
            squareMagnitude = (target.transform.position - _attackRangeSphere.transform.position).sqrMagnitude;
            if(squareMagnitude < closestMagnitude){
                // this is the closest target so far
                closestTarget = target;
                closestMagnitude = squareMagnitude;
            }
        }

        if(closestMagnitude <= _attackRangeSquared)
        {
            lockedOnTarget = closestTarget.GetComponentInChildren<IAttackable>();

            lockedOnTarget.Hit(Weapon.hitDamage, Vector3.zero);
        }


        yield return new WaitForSeconds(0.5f / _attackSpeed);
    }



}
