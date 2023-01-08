using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField]
    RotationAxis m_CurrentRotation = RotationAxis.none;

    [SerializeField]
    ushort m_CurrentRow = 0;

    [SerializeField]
    ushort m_CurrentColumn = 0;

    private void Start()
    {
        
    }
}

public enum RotationAxis
{
    none,
    horizontal,
    vertical
}