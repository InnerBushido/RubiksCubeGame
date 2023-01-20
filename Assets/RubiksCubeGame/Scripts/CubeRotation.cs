using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRotation : MonoBehaviour
{
    public List<CubeDetector> m_Rows;
    public List<CubeDetector> m_Columns;
    public List<CubeDetector> m_Columns2;

    public List<CubeColorDetector> m_ColorDetectors;

    // Temporary Gameobject created for Rotation
    GameObject m_CurrentRotator;

    // Keep track of childed cubes for unchilding
    List<GameObject> m_RotatingCubes = new List<GameObject>();

    // Lock Player from rotating while rotation is active
    private bool rotating = false;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            RotateAxis(m_Rows[0]);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            RotateAxis(m_Rows[0], false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            RotateAxis(m_Rows[1]);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            RotateAxis(m_Rows[1], false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            RotateAxis(m_Rows[2]);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            RotateAxis(m_Rows[2], false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            RotateAxis(m_Columns[0]);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            RotateAxis(m_Columns[0], false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            RotateAxis(m_Columns[1]);
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            RotateAxis(m_Columns[1], false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            RotateAxis(m_Columns[2]);
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            RotateAxis(m_Columns[2], false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            RotateAxis(m_Columns2[0]);
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            RotateAxis(m_Columns2[0], false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            RotateAxis(m_Columns2[1]);
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            RotateAxis(m_Columns2[1], false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            RotateAxis(m_Columns2[2]);
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            RotateAxis(m_Columns2[2], false);
        }
    }

    public void RotateAxis(CubeDetector axis, bool rotateClockwise = true)
    {
        if (rotating)
            return;

        rotating = true;

        m_CurrentRotator = new GameObject(axis.name);
        m_CurrentRotator.transform.position = axis.transform.position;
        m_CurrentRotator.transform.rotation = axis.transform.rotation;
        m_CurrentRotator.transform.parent = transform;

        m_RotatingCubes.Clear();

        foreach (GameObject cube in axis.DetectedCubes)
        {
            m_RotatingCubes.Add(cube);
            cube.transform.parent = m_CurrentRotator.transform;
        }

        StartCoroutine(RotateAxis(rotateClockwise, axis.m_AxisOfRotation));

    }

    IEnumerator RotateAxis(bool rotateClockwise, Vector3 rotationAxis)
    {
        //Debug.Log("Started Rotating");

        Quaternion startRotation = m_CurrentRotator.transform.localRotation;
        float endRot = 90;
        float duration = 0.5f;
        float t = 0;

        // Flip Rotation Direction
        if(!rotateClockwise)
        {
            endRot *= -1;
        }

        while (t < 1f)
        {
            t = Mathf.Min(1f, t + Time.deltaTime / duration);
            Vector3 newEulerOffset = rotationAxis * (endRot * t);

            m_CurrentRotator.transform.localRotation = startRotation * Quaternion.Euler(newEulerOffset);

            yield return null;
        }

        FinishedRotating();
    }

    void FinishedRotating()
    {
        //Debug.Log("Finished Rotating");

        foreach(GameObject cube in m_RotatingCubes)
        {
            cube.transform.parent = transform;
        }
        Destroy(m_CurrentRotator);

        DetectNewCubes();
        EnableColorDetections();

        rotating = false;
    }

    // After Rotation, update the cubes in each section for future rotation
    void DetectNewCubes()
    {
        foreach(CubeDetector detector in m_Rows)
        {
            detector.DetectCubes();
        }

        foreach (CubeDetector detector in m_Columns)
        {
            detector.DetectCubes();
        }

        foreach (CubeDetector detector in m_Columns2)
        {
            detector.DetectCubes();
        }
    }

    // After Rotation, update the Colors Detected on each side
    void EnableColorDetections(bool enable = true)
    {
        foreach (CubeColorDetector detector in m_ColorDetectors)
        {
            detector.DetectColors();
        }
    }

}
