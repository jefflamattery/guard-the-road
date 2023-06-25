using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Weapon Observer")]
public class WeaponObserver : Observer
{
    public GameObject weaponModel;
    
    public float hitDamage = 1f;
    public float attackSpeed = 1f;
    public Vector3 hitForce = Vector3.zero;
    public int maximumTargets = 1;
    
    
    public override Observer Clone()
    {
        WeaponObserver clone = ScriptableObject.CreateInstance<WeaponObserver>();

        clone.weaponModel = weaponModel;
        clone.hitDamage = hitDamage;
        clone.hitForce = hitForce;
        clone.maximumTargets = maximumTargets;

        return clone;
    }

    public override void Inject(GameObject agent)
    {
        IWeaponObserver[] slots = agent.GetComponentsInChildren<IWeaponObserver>();

        foreach(IWeaponObserver slot in slots)
        {
            slot.Weapon = this;
        }
    }

}

