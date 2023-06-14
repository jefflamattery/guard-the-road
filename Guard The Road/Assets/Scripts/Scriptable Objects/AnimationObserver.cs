using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable Object/Animation Observer")]
public class AnimationObserver : ScriptableObject
{
    private AnimationDriver _driver;
    
    public void Assign(AnimationDriver driver)
    {
       _driver = driver;
    }

    public void TriggerAnimation(string name)
    {
        if(_driver != null)
        {
            _driver.TriggerAnimation(name);
        }
        
    }

    public void SetState(string name, bool value)
    {
        if(_driver != null)
        {
            _driver.SetState(name, value);
        }
    }

    public void SetFloat(string name, float value)
    {
        if(_driver != null)
        {
            _driver.SetFloat(name, value);
        }
    }


}
