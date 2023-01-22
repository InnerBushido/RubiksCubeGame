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

    int m_CubeLayerMask = 1 << 8;
    int m_ColorsLayerMask = 1 << 9;
    int m_AxisFlipLayerMask = 1 << 10;

    public bool dragging { get; set; } = false;

    Vector3 clickDragStartPosition = new Vector3();
    int draggingMinimumAmount = 75;
    GameObject m_CubeColorSelected;
    GameObject m_CubeSelected;
    [SerializeField]
    bool m_FlipRotationAxis = false;


    private void Update()
    {
        CheckForCubeCollision();

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

    private void CheckForCubeCollision()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Check for Collision with Color on Cube
            if (Physics.Raycast(ray, out hit, 100.0f, m_ColorsLayerMask))
            {
                m_CubeColorSelected = hit.collider.gameObject;

                clickDragStartPosition = Input.mousePosition;
                dragging = true;
            }

            // Check for Collision with Cube
            if (Physics.Raycast(ray, out hit, 100.0f, m_CubeLayerMask))
            {
                m_CubeSelected = hit.collider.gameObject;
            }

            // Check if we need to flip the rotation axis (on back side of cube)
            if (Physics.Raycast(ray, out hit, 100.0f, m_AxisFlipLayerMask))
            {
                // See if we need to flip rotation axis
                if (hit.collider.gameObject.GetComponent<FlipRotationAxis>() != null)
                {
                    m_FlipRotationAxis = true;
                }
                else
                {
                    m_FlipRotationAxis = false;
                }
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            if(dragging)
            {
                Vector3 dragVector = Input.mousePosition - clickDragStartPosition;
                float dotProductVertical = Vector3.Dot(Vector3.up, dragVector.normalized);
                float dotProductHorizontal = Vector3.Dot(Vector3.right, dragVector.normalized);
                CubeDetector detectorToRotate;

                dragging = false;

                if(Mathf.Abs(dotProductVertical) >= Mathf.Abs(dotProductHorizontal))
                {
                    // Move Vertical
                    if(dragVector.magnitude >= draggingMinimumAmount)
                    {
                        detectorToRotate = FindAppropriateDetector(false);

                        if(dotProductVertical >= 0)
                        {
                            RotateAxis(detectorToRotate);
                        }
                        else
                        {
                            RotateAxis(detectorToRotate, false);
                        }
                    }
                }
                else if (Mathf.Abs(dotProductHorizontal) > Mathf.Abs(dotProductVertical))
                {
                    // Move Horizontal
                    if (dragVector.magnitude >= draggingMinimumAmount)
                    {
                        detectorToRotate = FindAppropriateDetector();

                        if (dotProductHorizontal >= 0)
                        {
                            RotateAxis(detectorToRotate);
                        }
                        else
                        {
                            RotateAxis(detectorToRotate, false);
                        }
                    }
                }
                else
                {
                    Debug.LogError("A Drag direction should always occur.");
                }
            }
        }
    }

    private CubeDetector FindAppropriateDetector(bool isHorizontal = true)
    {
        CubeDetector foundDetector = null;

        if(isHorizontal)
        {
            foreach(CubeDetector detector in m_Rows)
            {
                if(detector.DetectedCubeColors.Contains(m_CubeColorSelected))
                {
                    foundDetector = detector;
                    break;
                }
            }

            // Added to solve case where Color does not belong to a Row (top or bottom of cube)
            if(foundDetector == null)
            {
                foreach (CubeDetector detector in m_Rows)
                {
                    if (detector.DetectedCubes.Contains(m_CubeSelected))
                    {
                        foundDetector = detector;
                        break;
                    }
                }
            }
        }
        else
        {
            foreach (CubeDetector detector in m_Columns)
            {
                if (detector.DetectedCubeColors.Contains(m_CubeColorSelected))
                {
                    foundDetector = detector;
                    break;
                }
            }

            foreach (CubeDetector detector in m_Columns2)
            {
                if (detector.DetectedCubeColors.Contains(m_CubeColorSelected))
                {
                    foundDetector = detector;
                    break;
                }
            }
        }

        return foundDetector;
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

        StartCoroutine(RotateAxisEnum(rotateClockwise, axis));

    }

    IEnumerator RotateAxisEnum(bool rotateClockwise, CubeDetector axis)
    {
        //Debug.Log("Started Rotating");

        Quaternion startRotation = m_CurrentRotator.transform.localRotation;
        float endRot = 90;
        float duration = 0.5f;
        float t = 0;

        // Only flip for Columns
        if (m_FlipRotationAxis && !axis.m_OrderByRow)
        {
            rotateClockwise = !rotateClockwise;
        }

        // Flip Rotation Direction
        if (!rotateClockwise)
        {
            endRot *= -1;
        }

        while (t < 1f)
        {
            t = Mathf.Min(1f, t + Time.deltaTime / duration);
            Vector3 newEulerOffset = axis.m_AxisOfRotation * (endRot * t);

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
