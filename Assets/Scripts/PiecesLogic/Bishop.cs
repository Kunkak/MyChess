using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    void Awake()
    {
        base.OnStart();
        type = "bishop";
    }

    protected override bool[,] Moves()
    {
        bool[,] moves = Piece.EmptyBoard();
        int column, row;

        // check squeres to the top right
        column = position.x + 1; row = position.y + 1;
        while (column < 9 && row < 9)
        {
            if (GameManager.instance.CheckPiece(column, row) == null)
            {
                moves[column, row] = true;
                column++; row++;
            }
            else if (GameManager.instance.CheckPiece(column, row) == !white)
            {
                moves[column, row] = true;
                break;
            }
            else
                break;
        }
        
        // check squeres to the top left
        column = position.x - 1; row = position.y + 1;
        while (column > 0 && row < 9)
        {
            if (GameManager.instance.CheckPiece(column, row) == null)
            {
                moves[column, row] = true;
                column--; row++;
            }
            else if (GameManager.instance.CheckPiece(column, row) == !white)
            {
                moves[column, row] = true;
                break;
            }
            else
                break;
        }
        

        // check squeres to the bottom right
        column = position.x + 1; row = position.y - 1;
        while (column < 9 && row > 0)
        {
            if (GameManager.instance.CheckPiece(column, row) == null)
            {
                moves[column, row] = true;
                column++; row--;
            }
            else if (GameManager.instance.CheckPiece(column, row) == !white)
            {
                moves[column, row] = true;
                break;
            }
            else
                break;
        }

        // check squeres to the bottom left
        column = position.x - 1; row = position.y - 1;
        while (column > 0 && row > 0)
        {
            if (GameManager.instance.CheckPiece(column, row) == null)
            {
                moves[column, row] = true;
                column--; row--;
            }
            else if (GameManager.instance.CheckPiece(column, row) == !white)
            {
                moves[column, row] = true;
                break;
            }
            else
                break;
        }

        return moves;
    }

    public override void FillChecksBoard()
    {
        var board = white ? GameManager.instance.whiteChecks : GameManager.instance.blackChecks;

        int column, row;

        // check squeres to the top right
        column = position.x + 1; row = position.y + 1;
        while (column < 9 && row < 9)
        {
            if (GameManager.instance.CheckPiece(column, row) == null)
            {
                board[column, row] = true;
                column++; row++;
            }
            else if (GameManager.instance.CheckPiece(column, row) == !white && GameManager.instance.board[column, row].Type == "king")
            {
                board[column, row] = true;
                column++; row++;
            }
            else
            {
                board[column, row] = true;
                break;
            }
        }

        // check squeres to the top left
        column = position.x - 1; row = position.y + 1;
        while (column > 0 && row < 9)
        {
            if (GameManager.instance.CheckPiece(column, row) == null)
            {
                board[column, row] = true;
                column--; row++;
            }
            else if (GameManager.instance.CheckPiece(column, row) == !white && GameManager.instance.board[column, row].Type == "king")
            {
                board[column, row] = true;
                column--; row++;
            }
            else
            {
                board[column, row] = true;
                break;
            }
        }

        // check squeres to the bottom right
        column = position.x + 1; row = position.y - 1;
        while (column < 9 && row > 0)
        {
            if (GameManager.instance.CheckPiece(column, row) == null)
            {
                board[column, row] = true;
                column++; row--;
            }
            // through the king
            else if (GameManager.instance.CheckPiece(column, row) == !white && GameManager.instance.board[column, row].Type == "king")
            {
                board[column, row] = true;
                column++; row--;
            }
            else
            {
                board[column, row] = true;
                break;
            }
        }

        // check squeres to the bottom left
        column = position.x - 1; row = position.y - 1;
        while (column > 0 && row > 0)
        {
            if (GameManager.instance.CheckPiece(column, row) == null)
            {
                board[column, row] = true;
                column--; row--;
            }
            // through the king
            else if (GameManager.instance.CheckPiece(column, row) == !white && GameManager.instance.board[column, row].Type == "king")
            {
                board[column, row] = true;
                column--; row--;
            }
            else
            {
                board[column, row] = true;
                break;
            }
        }
    }
}
