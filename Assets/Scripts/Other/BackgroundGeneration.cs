using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundGeneration : MonoBehaviour
{
    public GameObject cube;
    public int radius = 10;

    private void Awake()
    {
        CreateBackground(radius);
    }

    void CreateBackground(int radius = 10)
    {
        Instantiate(cube, transform.position, Quaternion.identity).transform.SetParent(transform);

        for(int i = 0; i < radius; i++)
        {
            for (int j = 0; j < radius; j++)
            {
                Instantiate(cube, transform.position + new Vector3(i * 12, 0, j * 12), Quaternion.identity).transform.SetParent(transform);
                Instantiate(cube, transform.position + new Vector3(-i * 12, 0, j * 12), Quaternion.identity).transform.SetParent(transform);
                Instantiate(cube, transform.position + new Vector3(-i * 12, 0, -j * 12), Quaternion.identity).transform.SetParent(transform);
                Instantiate(cube, transform.position + new Vector3(i * 12, 0, -j * 12), Quaternion.identity).transform.SetParent(transform);
            }
        }
    }
}
