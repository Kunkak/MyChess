using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Board2D : MonoBehaviour
{
    public static Board2D instance;
    public GameObject[,] squares;
    public bool[,] possibleSquares;
    public GameObject square;

    private Color
        white = Color.white,
        black = new Color(0.3301887f, 0.1533019f, 0, 1),
        lightGreen = new Color(0, 0.5f, 0, 1),
        darkGreen = new Color(0.03147675f, 0.2735849f, 0, 1),
        blue = new Color(0.3653582f, 0.260146f, 0.6981132f, 1),
        red = new Color(1, 0, 0, 1);

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        squares = new GameObject[9, 9];
        SetSquares();
    }

    public void ResetColors()
    {
        for (int i = 1; i < 9; i++)
        {
            for (int j = 1; j < 9; j++)
            {
                if((i + j) % 2 == 1)
                    squares[i,j].GetComponent<SpriteRenderer>().color = white;
                else
                    squares[i, j].GetComponent<SpriteRenderer>().color = black;
            }
        }
        possibleSquares = Piece.EmptyBoard();
    }


    public void GreenSquares(bool[,] array, Vector2Int from)
    {
        // save the board with possible squares
        possibleSquares = array;

        for (int i = 1; i < 9; i++)
        {
            for (int j = 1; j < 9; j++)
            {
                if (possibleSquares[i, j])
                {
                    if ((i + j) % 2 == 1)
                        squares[i, j].GetComponent<SpriteRenderer>().color = lightGreen;
                    else
                        squares[i, j].GetComponent<SpriteRenderer>().color = darkGreen;
                }
                else
                {
                    if ((i + j) % 2 == 1)
                        squares[i, j].GetComponent<SpriteRenderer>().color = white;
                    else
                        squares[i, j].GetComponent<SpriteRenderer>().color = black;
                }
            }
        }
        squares[from.x, from.y].GetComponent<SpriteRenderer>().color = blue;
    }

    public void RedSquares(bool[,] array)
    {
        for (int i = 1; i < 9; i++)
        {
            for (int j = 1; j < 9; j++)
            {
                if (array[i, j])
                {
                    squares[i, j].GetComponent<SpriteRenderer>().color = red;
                }
                else
                {
                    if ((i + j) % 2 == 0)
                        squares[i, j].GetComponent<SpriteRenderer>().color = white;
                    else
                        squares[i, j].GetComponent<SpriteRenderer>().color = black;
                }
            }
        }
    }

    private void SetSquares()
    {
        for (int i = 1; i < 9; i++) 
        {
            for (int j = 1; j < 9; j++)
            {
                squares[i, j] = Instantiate(square, new Vector3(i, j, 0), Quaternion.identity, transform);
            }
        }
        ResetColors();
    } 

}
