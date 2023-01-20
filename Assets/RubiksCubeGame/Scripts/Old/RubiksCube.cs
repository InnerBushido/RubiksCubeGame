using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RubiksCube : MonoBehaviour
{
    [SerializeField]
    GameObject m_CubeArrayPrefab;

    [SerializeField]
    List<CubeArray> m_Rows = new List<CubeArray>();

    [SerializeField]
    List<CubeArray> m_Columns = new List<CubeArray>();



    [SerializeField]
    CubeArray m_Row1;

    [SerializeField]
    CubeArray m_Row2;

    [SerializeField]
    CubeArray m_Column1;

    [SerializeField]
    CubeArray m_Column2;

    private void Start()
    {
        InitializeCube();
    }

    private void Update()
    {
        UserInput();
    }

    private void InitializeCube()
    {
        ushort amountOfArrays = GameManager.m_CubeMatrixAmount;
        ushort amountOfMiddleArrays = 0;

        if(amountOfArrays <= 2)
        {
            amountOfMiddleArrays = 0;

            // Initialize Horizontal Arrays
            for(int i = 0; i < amountOfArrays; i++)
            {
                // Find Center of rotation for the array of cubes
                float arrayCenter = (amountOfArrays / 2) - 0.5f;
                CubeArray spawnedArray;

                spawnedArray = Instantiate(m_CubeArrayPrefab.GetComponent<CubeArray>(), transform);
                spawnedArray.transform.localPosition = new Vector3(arrayCenter, -i, arrayCenter);
                spawnedArray.gameObject.name = "Row" + i;
                spawnedArray.Initialize(RotationAxis.horizontal);

                m_Rows.Add(spawnedArray);
            }
        }
        else
        {
            amountOfMiddleArrays = (ushort)(amountOfArrays - 2);
        }
    }

    private void UserInput()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            RotateCubeArray(m_Row1);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            RotateCubeArray(m_Row1, false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            RotateCubeArray(m_Row2);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            RotateCubeArray(m_Row2, false);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            RotateCubeArray(m_Column1);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            RotateCubeArray(m_Column1, false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            RotateCubeArray(m_Column2);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            RotateCubeArray(m_Column2, false);
        }
    }

    // Operations required to make a rotation
    private void RotateCubeArray(CubeArray cubes, bool rotateClockwise = true)
    {
        ParentCubes(cubes);
        ShiftCubeArray(cubes, rotateClockwise);
        UpdateCubeArrays(cubes);
        RotateCubes(cubes, rotateClockwise);
    }

    private void ParentCubes(CubeArray cubes)
    {
        foreach(Cube cube in cubes.Cubes)
        {
            cube.transform.SetParent(cubes.transform);
        }
    }

    private void RotateCubes(CubeArray cubes, bool rotateClockwise)
    {
        if(cubes.RotationAxis == RotationAxis.horizontal)
        {
            if(rotateClockwise)
            {
                // Rotate to the Right
                cubes.transform.Rotate(Vector3.down * 90);
            }
            else
            {
                // Rotate to the Left
                cubes.transform.Rotate(Vector3.up * 90);
            }
        }
        else if(cubes.RotationAxis == RotationAxis.vertical)
        {
            if (rotateClockwise)
            {
                // Rotate downward
                cubes.transform.Rotate(Vector3.left * 90);
            }
            else
            {
                // Rotate upward
                cubes.transform.Rotate(Vector3.right * 90);
            }
        }
    }

    // We put the last or first cube on the beginning or end depending on direction rotated
    private void ShiftCubeArray(CubeArray cubes, bool shiftClockwise)
    {
        Cube finalCube;

        //if(shiftClockwise)
        //{
        //    finalCube = cubes.Cubes[GameManager.m_CubeMatrixAmount];
        //    cubes.Cubes.Remove(finalCube);
        //    cubes.Cubes.Insert(0, finalCube);
        //}

        if (shiftClockwise)
        {
            finalCube = cubes.Cubes.Last();
            cubes.Cubes.Remove(finalCube);
            cubes.Cubes.Insert(0, finalCube);
        }
        else
        {
            finalCube = cubes.Cubes.First();
            cubes.Cubes.Remove(finalCube);
            cubes.Cubes.Add(finalCube);
        }
    }

    // We Update all other Arrays (ex. if horizontal rotation, update all the vertical)
    private void UpdateCubeArrays(CubeArray cubes)
    {
        if(cubes.RotationAxis == RotationAxis.horizontal)
        {
            // We need to update all Vertical Cube Arrays
            m_Column1.Cubes.Clear();
            m_Column1.Cubes.Add(m_Row1.Cubes[0]);
            m_Column1.Cubes.Add(m_Row1.Cubes[1]);
            m_Column1.Cubes.Add(m_Row2.Cubes[1]);
            m_Column1.Cubes.Add(m_Row2.Cubes[0]);

            m_Column2.Cubes.Clear();
            m_Column2.Cubes.Add(m_Row1.Cubes[3]);
            m_Column2.Cubes.Add(m_Row1.Cubes[2]);
            m_Column2.Cubes.Add(m_Row2.Cubes[2]);
            m_Column2.Cubes.Add(m_Row2.Cubes[3]);
        }
        else if (cubes.RotationAxis == RotationAxis.vertical)
        {
            // We need to update all Horizontal Cube Arrays
            m_Row1.Cubes.Clear();
            m_Row1.Cubes.Add(m_Column1.Cubes[0]);
            m_Row1.Cubes.Add(m_Column1.Cubes[1]);
            m_Row1.Cubes.Add(m_Column2.Cubes[1]);
            m_Row1.Cubes.Add(m_Column2.Cubes[0]);

            m_Row2.Cubes.Clear();
            m_Row2.Cubes.Add(m_Column1.Cubes[3]);
            m_Row2.Cubes.Add(m_Column1.Cubes[2]);
            m_Row2.Cubes.Add(m_Column2.Cubes[2]);
            m_Row2.Cubes.Add(m_Column2.Cubes[3]);
        }
    }

}
