using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeArray : MonoBehaviour
{

    [SerializeField]
    private RotationAxis m_AxisOfRotation;

    [SerializeField]
    List<Cube> m_Cubes = new List<Cube>();

    public List<Cube> Cubes
    {
        set { m_Cubes = value; }
        get { return m_Cubes; }
    }

    public RotationAxis RotationAxis
    {
        get { return m_AxisOfRotation; }
    }

}
