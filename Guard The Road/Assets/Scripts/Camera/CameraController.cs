using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    
    [SerializeField] private CameraObserver _observer;
    [SerializeField] private MotionObserver _subject;
    [SerializeField] private bool _showTrack;

    [SerializeField] Vector3 _initialTrackMinimum;
    [SerializeField] Vector3 _initialTrackMaximum;
    [SerializeField] Vector2 _initialParameterLimits;

    void Awake()
    {
        _observer.trackMaximum = _initialTrackMaximum;
        _observer.trackMinimum = _initialTrackMinimum;
        _observer.trackParameterLimits = _initialParameterLimits;
    }

    void FixedUpdate()
    {
         Vector3 trackPosition =_subject.Position + _observer.trackMinimum + _observer.TrackParameter * (_observer.trackMaximum - _observer.trackMinimum);

        // the camera slides along a linear track as defined by the CameraObserver
        // the track is parameterized by the trackParameter
        transform.position = trackPosition;

        transform.rotation = Quaternion.LookRotation(_subject.Position - transform.position, Vector3.up);

        _observer.orientation = transform.rotation;

        if(_showTrack){
            Debug.DrawLine(_subject.Position + _observer.trackMinimum, _subject.Position + _observer.trackMaximum, Color.green);
        }
        

    }
}
