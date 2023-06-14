using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private SpawnObserver _gradientAscent;
    [SerializeField] private SpawnObserver _randomWalk;
    [SerializeField] private SpawnObserver _aStar;

    [SerializeField] private List<Toggle> _toggles;
    public static HUDManager instance;

    void Awake(){
        // ensure there is only one instance of this object
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);

        } else {
            Destroy(gameObject);
        }

        foreach(Toggle toggle in _toggles)
        {
            toggle.isOn = false;
        }
    }

    public void ToggleRandomWalk(Toggle value)
    {
        _randomWalk.isSpawned = value.isOn;
    }

    public void ToggleGradientAscent(Toggle value)
    {
        _gradientAscent.isSpawned = value.isOn;
    }

    public void ToggleAStar(Toggle value)
    {

    }

}
