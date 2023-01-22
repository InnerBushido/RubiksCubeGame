using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CubeDetector : MonoBehaviour
{
    public bool m_OrderByRow = false;

    public Vector3 m_AxisOfRotation = Vector3.up;

    [SerializeField]
    List<GameObject> m_DetectedCubes = new List<GameObject>();

    [SerializeField]
    List<GameObject> m_DetectedCubeColors = new List<GameObject>();

    public List<GameObject> DetectedCubes
    {
        get { return m_DetectedCubes; }
        set { m_DetectedCubes = value; }
    }

    public List<GameObject> DetectedCubeColors
    {
        get { return m_DetectedCubeColors; }
        set { m_DetectedCubeColors = value; }
    }

    // Needed to make sure cubes are detected in FixedUpdate
    bool ExecuteUpdated() => m_UpdateExecuted;
    bool m_UpdateExecuted = false;

    private void Start()
    {
        GetComponent<Collider>().enabled = false;

        DetectCubes();
    }

    private void FixedUpdate()
    {
        if(!m_UpdateExecuted)
        {
            m_UpdateExecuted = true;
        }
    }

    public void DetectCubes()
    {
        StartCoroutine(DetectCubesCoroutine());
    }

    IEnumerator DetectCubesCoroutine()
    {
        m_UpdateExecuted = false;
        m_DetectedCubes.Clear();
        m_DetectedCubeColors.Clear();
        GetComponent<Collider>().enabled = true;
        yield return new WaitUntil(ExecuteUpdated);
        GetComponent<Collider>().enabled = false;

        if(m_OrderByRow)
        {
            m_DetectedCubes = m_DetectedCubes
                        .OrderBy(x => x.transform.localPosition.z)
                        .ThenBy(x => x.transform.localPosition.x).ToList();
        }
        else
        {
            m_DetectedCubes = m_DetectedCubes
                        .OrderByDescending(x => x.transform.localPosition.y)
                        .ThenBy(x => x.transform.localPosition.x)
                        .ThenBy(x => x.transform.localPosition.z).ToList();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other?.tag == "Cube")
        {
            m_DetectedCubes.Add(other.gameObject);
        }
        else if (other?.tag == "CubeColor")
        {
            m_DetectedCubeColors.Add(other.gameObject);
        }
    }
}
