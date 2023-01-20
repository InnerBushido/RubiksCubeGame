using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CubeDetector : MonoBehaviour
{
    public bool m_OrderByRow = false;

    [SerializeField]
    List<GameObject> m_DetectedCubes = new List<GameObject>();

    bool ExecuteUpdated() => m_UpdateExecuted;
    bool m_UpdateExecuted = false;

    private void Start()
    {
        GetComponent<Collider>().enabled = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(DetectCubes());
        }
    }

    private void FixedUpdate()
    {
        if(!m_UpdateExecuted)
        {
            m_UpdateExecuted = true;
        }
    }

    IEnumerator DetectCubes()
    {
        m_UpdateExecuted = false;
        m_DetectedCubes.Clear();
        GetComponent<Collider>().enabled = true;
        yield return new WaitUntil(ExecuteUpdated);
        GetComponent<Collider>().enabled = false;

        Debug.Log("Ordering Cubes");
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
            Debug.Log("Detected Cube: " + other.name);
        }
    }
}
