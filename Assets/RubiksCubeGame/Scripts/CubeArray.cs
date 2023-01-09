using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeArray : MonoBehaviour
{
    [SerializeField]
    GameObject m_CubePrefab;

    [SerializeField]
    private RotationAxis m_AxisOfRotation;

    [SerializeField]
    List<Cube> m_Cubes = new List<Cube>();

    public void Initialize(RotationAxis axis)
    {
        m_AxisOfRotation = axis;

        SpawnCubes();
    }

    public List<Cube> Cubes
    {
        set { m_Cubes = value; }
        get { return m_Cubes; }
    }

    public RotationAxis RotationAxis
    {
        get { return m_AxisOfRotation; }
    }

    private void SpawnCubes()
    {
        ushort amountOfArrays = GameManager.m_CubeMatrixAmount;

        m_Cubes.Clear();

        if (m_AxisOfRotation == RotationAxis.horizontal)
        {
            // Spawn By Column
            for (int col = 0; col < amountOfArrays; col++)
            {
                // Spawn By Row
                for(int row = 0; row < amountOfArrays; row++)
                {
                    Cube spawnedCube;
                    float offsetAmount = ((amountOfArrays - GameManager.MIN_CUBE_AMT) / 2) + 0.5f;

                    spawnedCube = Instantiate(m_CubePrefab.GetComponent<Cube>(), transform);
                    spawnedCube.gameObject.name = "Cube: " + "Column" + col + "Row" + row;
                    spawnedCube.transform.localPosition = new Vector3(col - offsetAmount, 0, (row * -1) + offsetAmount);

                    m_Cubes.Add(spawnedCube);
                }
            }
        }
    }

}
