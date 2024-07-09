using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Board3D : MonoBehaviour
{
    public static Board3D instance;
    public float scale;
    public Vector3 offset;
    public GameObject greenCube;

    public GameObject[,] possibleSquares = new GameObject[9,9];

    public float rotationSpeed = 10;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        SetValues();
    }

    private void Update()
    {
        if(!GameManager.instance.is3DActive) { return; }

        Move();
    }

    private void SetValues()
    {
        scale = transform.localScale.x * 6 / 100;
        offset = transform.position + new Vector3(-4.5f * scale, 0, -4.5f * scale);
    }

    private void Move()
    {
        Vector3 rotation = new Vector3(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"), 0);

        transform.Rotate(rotationSpeed * Time.deltaTime * rotation);
    }

    private void ClearPossibleSquares()
    {
        for (int i = 1; i < 9; i++)
        {
            for (int j = 1; j < 9; j++)
            {
                if (possibleSquares[i, j] != null)
                {
                    Destroy(possibleSquares[i, j]);
                    possibleSquares[i, j] = null;
                }
            }
        }
    }

    public void GreenSquares()
    {
        GreenSquares(GameManager.instance.theChosenOne.possibleMoves);
    }

    public void GreenSquares(bool[,] array)
    {
        ClearPossibleSquares();

        for (int i = 1; i < 9; i++)
        {
            for(int j = 1; j < 9; j++)
            {
                if (array[i, j])
                {
                    Vector3 position = new Vector3(i * scale, -1, j * scale) + offset;

                    possibleSquares[i, j] = Instantiate(greenCube, position, transform.rotation).GameObject();
                    possibleSquares[i, j].transform.SetParent(transform);
                }
            }
        }
    }

    public Vector2Int FindClosestSquare()
    {
        Piece3D piece = GameManager.instance.theChosenOne.piece3D;

        float shortestDistance = float.PositiveInfinity;
        Vector2Int closestPosition = new Vector2Int(0, 0);

        for (int i = 1; i < 9; i++)
        {
            for (int j = 1; j < 9; j++)
            {
                if (possibleSquares[i, j] != null)
                {
                    float distance = Vector3.Distance(piece.transform.position, possibleSquares[i, j].transform.position);
                    if (shortestDistance > distance)
                    {
                        shortestDistance = distance;
                        closestPosition = new Vector2Int(i, j);
                    }
                }
            }
        }

        return closestPosition;
    }

    public void ResetPosition()
    {
        //transform.SetPositionAndRotation(offset, Quaternion.Euler(-90f, 0f, 0f));
        transform.position = offset - new Vector3(-4.5f * scale, 0, -4.5f * scale);
        transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
    }
}
