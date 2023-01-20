using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public float turnSpeed = 1.0f;

    public Transform cameraPivotUp;


    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            float delta = Input.GetAxis("Mouse X") * turnSpeed;
            transform.Rotate(transform.up, delta);
        }

        if (Input.GetMouseButton(1))
        {
            float delta = Input.GetAxis("Mouse Y") * turnSpeed;
            //cameraPivotUp.Rotate(transform.right, delta);

            float valueNonClamped = cameraPivotUp.localEulerAngles.x + delta;
            //Debug.Log(valueNonClamped);

            if(valueNonClamped > 180)
            {
                valueNonClamped -= 360;
            }

            float clampedRotation = Mathf.Clamp(valueNonClamped, -80f, 80f);
            cameraPivotUp.localRotation = Quaternion.Euler(clampedRotation, cameraPivotUp.localEulerAngles.y, cameraPivotUp.localEulerAngles.z);

        }
    }

}
