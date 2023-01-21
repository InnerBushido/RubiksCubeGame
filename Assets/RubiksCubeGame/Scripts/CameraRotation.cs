using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public float m_TurnSpeed = 5.0f;
    public Transform m_CameraPivotUp;
    public Vector2 m_ZoomingThreshold = new Vector2(-8, -12);

    private RotationAxis m_CurrentRotationAxis = RotationAxis.none;

    private bool m_CanRotate = false;
    private float m_MinThreshold = 0.2f;

    private int m_ZoomMaxSteps = 10;
    private ushort m_ZoomCurrentStep = 0;
    private float m_ZoomStepDistance = 0;

    private void Start()
    {
        m_ZoomStepDistance = (m_ZoomingThreshold.y - m_ZoomingThreshold.x) / m_ZoomMaxSteps;
    }

    private enum RotationAxis
    {
        none,
        XAxis,
        YAxis
    }

    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButton(0))
        {
            RotateAlongAxis();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            // Reset axis dragging on drag release
            m_CanRotate = false;
            m_CurrentRotationAxis = RotationAxis.none;
        }

        HandleZoomingInput();

    }

    private void HandleZoomingInput()
    {
        float mouseWheel = Input.GetAxis("Mouse ScrollWheel");

        if (mouseWheel < 0)
        {
            // Zoom Out
            if (m_ZoomCurrentStep < m_ZoomMaxSteps)
            {
                m_ZoomCurrentStep++;
            }
        }
        else if (mouseWheel > 0)
        {
            // Zoom In
            if (m_ZoomCurrentStep > 0)
            {
                m_ZoomCurrentStep--;
            }
        }

        Camera.main.transform.localPosition = new Vector3(Camera.main.transform.localPosition.x, Camera.main.transform.localPosition.y, m_ZoomingThreshold.x + (m_ZoomCurrentStep * m_ZoomStepDistance));
    }

    private void RotateAlongAxis()
    {
        float mouseInputX;
        float delta;

        if (m_CurrentRotationAxis == RotationAxis.none)
        {
            CheckForAxisOfRotation();
        }

        if (m_CurrentRotationAxis == RotationAxis.XAxis)
        {
            mouseInputX = Input.GetAxis("Mouse X");

            // Rotate along X-Axis
            delta = mouseInputX * m_TurnSpeed;
            transform.Rotate(transform.up, delta);
        }
        else if(m_CurrentRotationAxis == RotationAxis.YAxis)
        {
            RotateOnYAxis();
        }
    }

    private void CheckForAxisOfRotation()
    {
        float mouseInputX = Input.GetAxis("Mouse X");
        float mouseInputY = Input.GetAxis("Mouse Y");

        if (mouseInputX > m_MinThreshold || mouseInputX < -m_MinThreshold)
        {
            m_CurrentRotationAxis = RotationAxis.XAxis;
            m_CanRotate = true;
        }
        else if(mouseInputY > m_MinThreshold || mouseInputY < -m_MinThreshold)
        {
            m_CurrentRotationAxis = RotationAxis.YAxis;
            m_CanRotate = true;
        }
    }

    private void RotateOnYAxis()
    {
        float delta = Input.GetAxis("Mouse Y") * m_TurnSpeed;
        float valueNonClamped = m_CameraPivotUp.localEulerAngles.x + delta;
        float clampedRotation;

        if (valueNonClamped > 180)
        {
            valueNonClamped -= 360;
        }

        clampedRotation = Mathf.Clamp(valueNonClamped, -80f, 80f);
        m_CameraPivotUp.localRotation = Quaternion.Euler(clampedRotation, m_CameraPivotUp.localEulerAngles.y, m_CameraPivotUp.localEulerAngles.z);
    }

}
