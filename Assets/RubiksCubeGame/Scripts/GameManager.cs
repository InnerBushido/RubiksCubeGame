using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : GenericSingletonClass<GameManager>
{
    public static ushort m_CubeMatrixAmount = 3;

    [SerializeField]
    GameObject m_RubiksCubePrefab;

    [SerializeField]
    RubiksCube m_RubiksCube;

    public static readonly ushort MIN_CUBE_AMT = 2;

    private void Start()
    {
        GameObject rubiksCube = null;

        if(m_RubiksCubePrefab != null)
        {
            rubiksCube = Instantiate(m_RubiksCubePrefab);
        }

        if (rubiksCube != null && rubiksCube.GetComponent<RubiksCube>() != null)
        {
            m_RubiksCube = rubiksCube.GetComponent<RubiksCube>();
        }
        else
        {
            Debug.LogError("MISSING RUBIKS CUBE PREFAB.");
        }
    }

}
