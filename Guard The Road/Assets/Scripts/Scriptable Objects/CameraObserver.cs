using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Camera Observer")]
public class CameraObserver : ScriptableObject
{
    public Quaternion orientation;
    public Vector3 trackMinimum;
    public Vector3 trackMaximum;
    public Vector2 trackParameterLimits;
    private float _trackParameter;
    public float TrackParameter{
        get=>_trackParameter;
        set{
            if(value < trackParameterLimits.x){
                _trackParameter = trackParameterLimits.x;
            } else if(value > trackParameterLimits.y){
                _trackParameter = trackParameterLimits.y;
            } else {
                _trackParameter = value;
            }
        }
    }

    
    
}
