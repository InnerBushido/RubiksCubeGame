using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeColorDetector : MonoBehaviour
{
    [SerializeField]
    List<GameObject> m_DetectedCubes = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if(other != null && other.tag == "CubeColor")
        {
            m_DetectedCubes.Add(other.gameObject);
            //Debug.Log("Detected Cube: " + other.name);
        }
    }
}
