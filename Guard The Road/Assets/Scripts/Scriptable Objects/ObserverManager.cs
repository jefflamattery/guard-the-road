using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObserverManager : MonoBehaviour, IMotionObserver, IFieldObserver, IAnimationObserver, IWeaponObserver, IVitalsObserver
{
    [SerializeField] private MotionObserver _motion;
    public MotionObserver Motion{
        get=>_motion;
        set=>_motion = value;
    }

    [SerializeField] private FieldObserver _field;
    public FieldObserver Field{
        get=>_field;
        set=>_field = value;
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

    [SerializeField] private VitalsObserver _vitals;
    public VitalsObserver Vitals{
        get=>_vitals;
        set=>_vitals = value;
    }

    public void CopyFrom(ObserverManager master)
    {
        _motion = (MotionObserver) master.Motion.Clone();
        _field = (FieldObserver) master.Field.Clone();
        _animation = (AnimationObserver) master.Animation.Clone();
        _weapon = (WeaponObserver) master.Weapon.Clone();
        _vitals = (VitalsObserver) master.Vitals.Clone();
    }

    public void BuildObservers()
    {
        _motion = ScriptableObject.CreateInstance<MotionObserver>();
        _field = ScriptableObject.CreateInstance<FieldObserver>();
        _animation = ScriptableObject.CreateInstance<AnimationObserver>();
        _weapon = ScriptableObject.CreateInstance<WeaponObserver>();
        _vitals = ScriptableObject.CreateInstance<VitalsObserver>();
    }

    public void InjectObservers()
    {
        _motion.Inject(this.gameObject);
        _field.Inject(this.gameObject);
        _animation.Inject(this.gameObject);
        _weapon.Inject(this.gameObject);
        _vitals.Inject(this.gameObject);
    }


}
