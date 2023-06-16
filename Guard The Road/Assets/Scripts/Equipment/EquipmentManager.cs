using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour, IWeaponObserver
{
    public const int MAIN_HAND = 0;
    public const int OFF_HAND = 1;
    [SerializeField] private GameObject _mainHand;
    [SerializeField] private GameObject _offHand;

    [SerializeField] private WeaponObserver _weapon;
    public WeaponObserver Weapon{
        get=>_weapon;
        set=>_weapon = value;
    }

    public void EquipWeapon(int equipmentSlot, GameObject weapon){
        
        switch (equipmentSlot)
        {
            case MAIN_HAND:
            weapon.transform.SetParent(_mainHand.transform, true);
            weapon.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            break;

            case OFF_HAND:
            weapon.transform.SetParent(_offHand.transform);
            break;
            default:
            break;
        }   

    }

    void Start()
    {
        GameObject weapon;

        if(Weapon.weaponModel != null){
            weapon  = GameObject.Instantiate(Weapon.weaponModel);
            EquipWeapon(MAIN_HAND,weapon);
        }
    }


}
