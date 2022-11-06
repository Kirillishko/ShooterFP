using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float Speed;

    public Transform Player;
    private Vector2 _rot = new Vector3(0, 0, 0);

    private float MouseX;
    private float MouseY;

    private Vector2 _currentRotation;
    private Vector2 mouseAxis;


    void Start()
    {
        //StartCoroutine(Cam());
        Player = transform.parent.GetComponent<Transform>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        RotateCamera();
        //Player.transform.eulerAngles = new Vector3(0, _rot.y, 0);
    }

    private void RotateCamera()
    {
        MouseX = Input.GetAxisRaw("Mouse X") * Speed;
        MouseY = Input.GetAxisRaw("Mouse Y") * Speed;
        _rot.x -= MouseY;
        _rot.y += MouseX;
        if (_rot.x > 90) _rot.x = 90;
        if (_rot.x < -90) _rot.x = -90;
        //_rot.z = 0;
        transform.eulerAngles = _rot;
        Player.eulerAngles = new Vector3(0, _rot.y, 0);

        //mouseAxis = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        //mouseAxis *= Speed;
        //_currentRotation += mouseAxis;

        //transform.localRotation = Quaternion.AngleAxis(_currentRotation.x, Vector3.up);
        //_currentRotation.y = Mathf.Clamp(_currentRotation.y, -90, 90);

        //transform.localRotation = Quaternion.AngleAxis(_currentRotation.x, Vector3.up);
        //// transform.localRotation = Quaternion.AngleAxis(_currentRotation.x, Vector3.up);
        ////transform.localRotation = Quaternion.AngleAxis(-_currentRotation.y, Vector3.right);
    }

    //IEnumerator Cam()
    //{
    //    while (true)
    //    {
    //        transform.localRotation = Quaternion.AngleAxis(-_currentRotation.y, Vector3.right);
    //    }
    //    return null;
    //}

    public float GetMouseX()
    {
        return MouseX;
    }

    public float GetMouseY()
    {
        return MouseY;
    }

    public void SetCurrentRotation(Vector2 currentRotation)
    {
        _rot += currentRotation;
    }
}
