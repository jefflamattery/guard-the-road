using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadListener : MonoBehaviour, IMotionObserver, IActionObserver
{
    [SerializeField] private MotionObserver _motion;
    public MotionObserver Motion{
        get=>_motion;
        set=>_motion = value;
    }
    [SerializeField] private ActionObserver _action;
    public ActionObserver Action{
        get=>_action;
        set=>_action = value;
    }

    [SerializeField] private CameraObserver _camera;
    [SerializeField] private float _cameraZoomRate = -0.05f;
    [SerializeField] private float _cameraRotationRate = -3f;
    [SerializeField] private float _cursorErrorRadius = 50f;
    [SerializeField] private float _mouseZoomTime = 0.2f;
    [SerializeField] private float _mouseRotationScale;

    private float _cameraParameterChange = 0f;
    private float _cameraRotationChange;
    private float _cameraAngle = 0f;

    private Vector3 _moveStick = Vector3.zero;
    private Vector3 _lookStick = Vector3.zero;

    private Coroutine _mouseListen;
    private bool _mouseInLookMode = false;
    private bool _mouseInMoveMode = false;
    private Vector2 _mousePanOrigin;
    
   

    
    public void ChangeMotion(){}

    void OnStartMove(InputValue value)
    {
        _mouseInMoveMode = true;

        if(_mouseListen == null){
            _mouseListen = StartCoroutine(MouseAsStick());
        }
    }

    void OnStopMove(InputValue value)
    {
        _mouseInMoveMode = false;
        StopCoroutine(_mouseListen);
        _mouseListen = null;
        _moveStick = Vector3.zero;
    }

    void OnStartPan(InputValue value)
    {
        _mouseInLookMode = true;
        _mousePanOrigin = Mouse.current.position.ReadValue();

        if(_mouseListen == null){
            _mouseListen = StartCoroutine(MouseAsStick());
        }
    }

    void OnStopPan(InputValue value)
    {
        _mouseInLookMode = false;
        _cameraRotationChange = 0f;
    }

    void OnZoom(InputValue value)
    {
        Vector2 scroll = value.Get<Vector2>();
        scroll.Normalize();

        StartCoroutine(MouseZoom(scroll, _mouseZoomTime));
        
    }

    IEnumerator MouseZoom(Vector2 scrollDirection, float zoomTime)
    {
        float cameraZoomChange = _cameraZoomRate * Vector2.Dot(scrollDirection, Vector2.up);

        _cameraParameterChange += cameraZoomChange;

        yield return new WaitForSeconds(zoomTime);

        _cameraParameterChange -= cameraZoomChange;
    }

    IEnumerator MouseAsStick()
    {
        Vector2 mousePosition;
        Vector3 characterPosition = Vector3.zero;
        Vector3 course;

        while(true){

            mousePosition = Mouse.current.position.ReadValue();
            characterPosition = Camera.main.WorldToScreenPoint(_motion.position);
            course = new Vector3(mousePosition.x - characterPosition.x, 0f, mousePosition.y - characterPosition.y);

            // if the course magnitude is less than cursor error, then this is effectively a stop command
            if(course.magnitude > _cursorErrorRadius){
                course.Normalize();

                if(_mouseInLookMode){
                    RotateCameraTrack(Quaternion.AngleAxis(_mouseRotationScale * Vector2.Dot(mousePosition - _mousePanOrigin, Vector2.right), Vector3.up));
                }
                
                if(_mouseInMoveMode){
                    _moveStick = course;
                }

            } else {
                if(_mouseInMoveMode){
                     _moveStick = Vector3.zero;
                }
            }

            yield return new WaitForFixedUpdate();
        }
    }

    void OnStickMove(InputValue value)
    {
        Vector2 stickHeading = value.Get<Vector2>();
       
       _moveStick.x = stickHeading.x;
       _moveStick.z = stickHeading.y;
    }

    void OnStickCamera(InputValue value)
    {
        Vector2 stickHeading = value.Get<Vector2>();

        if(stickHeading.y > 0f){
            _cameraParameterChange = _cameraZoomRate;
        } else if(stickHeading.y < 0f){
            _cameraParameterChange = -_cameraZoomRate;
        } else {
            _cameraParameterChange = 0f;
        }

        _cameraRotationChange = -_cameraRotationRate * stickHeading.x;

    }

    void RotateCameraTrack(Quaternion rotation)
    {
        // rotate the track
        _camera.trackMinimum = rotation * _camera.trackMinimum;
        _camera.trackMaximum = rotation * _camera.trackMaximum;

        // rotate the camera forward vector
        _cameraAngle += _cameraRotationChange;
    }

    void FixedUpdate()
    {
        // move the camera along the track
        _camera.TrackParameter += _cameraParameterChange;

        if(_cameraRotationChange != 0f){
            RotateCameraTrack(Quaternion.AngleAxis(_cameraRotationChange, Vector3.up));
        }
    
        // update the course based on what direction the moveStick is in, and what angle the camera has been rotated to
        _motion.course = _camera.orientation * _moveStick;
        _motion.course.y = 0f;
    }


}
