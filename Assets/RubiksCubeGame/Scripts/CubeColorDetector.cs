using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeColorDetector : MonoBehaviour
{
    [SerializeField]
    List<GameObject> m_DetectedColors = new List<GameObject>();

    // Needed to make sure colors are detected in FixedUpdate
    bool ExecuteUpdated() => m_UpdateExecuted;
    bool m_UpdateExecuted = false;

    private void Start()
    {
        GetComponent<Collider>().enabled = false;

        DetectColors();
    }

    private void FixedUpdate()
    {
        if (!m_UpdateExecuted)
        {
            m_UpdateExecuted = true;
        }
    }

    public void DetectColors()
    {
        StartCoroutine(DetectColorsCoroutine());
    }

    IEnumerator DetectColorsCoroutine()
    {
        m_UpdateExecuted = false;
        m_DetectedColors.Clear();
        GetComponent<Collider>().enabled = true;
        yield return new WaitUntil(ExecuteUpdated);
        GetComponent<Collider>().enabled = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other != null && other.tag == "CubeColor")
        {
            m_DetectedColors.Add(other.gameObject);
            //Debug.Log("Detected Cube: " + other.name);
        }
    }
}
