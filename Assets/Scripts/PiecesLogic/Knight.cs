using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    void Awake()
    {
        base.OnStart();
        type = "knight";
    }

    protected override bool[,] Moves()
    {
        bool[,] moves = Piece.EmptyBoard();

        // 2 rows up
        if (position.y < 7)
        {
            // to the right
            if (position.x < 8) 
            {
                if (GameManager.instance.CheckPiece(position.x + 1, position.y + 2) != white) 
                {
                    moves[position.x + 1, position.y + 2] = true;
                }
            }
            // to the left
            if (position.x > 1)
            {
                if (GameManager.instance.CheckPiece(position.x - 1, position.y + 2) != white)
                {
                    moves[position.x - 1, position.y + 2] = true;
                }
            }
        }

        // 2 rows down
        if (position.y > 2)
        {
            // to the right
            if (position.x < 8)
            {
                if (GameManager.instance.CheckPiece(position.x + 1, position.y - 2) != white)
                {
                    moves[position.x + 1, position.y - 2] = true;
                }
            }
            // to the left
            if (position.x > 1)
            {
                if (GameManager.instance.CheckPiece(position.x - 1, position.y - 2) != white)
                {
                    moves[position.x - 1, position.y - 2] = true;
                }
            }
        }

        // 1 row up
        if (position.y < 8)
        {
            // to the right
            if (position.x < 7)
            {
                if (GameManager.instance.CheckPiece(position.x + 2, position.y + 1) != white)
                {
                    moves[position.x + 2, position.y + 1] = true;
                }
            }
            // to the left
            if (position.x > 2)
            {
                if (GameManager.instance.CheckPiece(position.x - 2, position.y + 1) != white)
                {
                    moves[position.x - 2, position.y + 1] = true;
                }
            }
        }

        // 1 row down
        if (position.y > 1)
        {
            // to the right
            if (position.x < 7)
            {
                if (GameManager.instance.CheckPiece(position.x + 2, position.y - 1) != white)
                {
                    moves[position.x + 2, position.y - 1] = true;
                }
            }
            // to the left
            if (position.x > 2)
            {
                if (GameManager.instance.CheckPiece(position.x - 2, position.y - 1) != white)
                {
                    moves[position.x - 2, position.y - 1] = true;
                }
            }
        }

        return moves;
    }

    public override void FillChecksBoard()
    {
        var board = white ? GameManager.instance.whiteChecks : GameManager.instance.blackChecks;

        // 2 rows up
        if (position.y < 7)
        {
            // to the right
            if (position.x < 8)
            {
                board[position.x + 1, position.y + 2] = true;
            }
            // to the left
            if (position.x > 1)
            {
                board[position.x - 1, position.y + 2] = true;
            }
        }

        // 2 rows down
        if (position.y > 2)
        {
            // to the right
            if (position.x < 8)
            {
                board[position.x + 1, position.y - 2] = true;
            }
            // to the left
            if (position.x > 1)
            {
                board[position.x - 1, position.y - 2] = true;
            }
        }

        // 1 row up
        if (position.y < 8)
        {
            // to the right
            if (position.x < 7)
            {
                board[position.x + 2, position.y + 1] = true;
            }
            // to the left
            if (position.x > 2)
            {
                board[position.x - 2, position.y + 1] = true;
            }
        }

        // 1 row down
        if (position.y > 1)
        {
            // to the right
            if (position.x < 7)
            {
                board[position.x + 2, position.y - 1] = true;
            }
            // to the left
            if (position.x > 2)
            {
                board[position.x - 2, position.y - 1] = true;
            }
        }
    }

    protected override void Adjust(ref Vector3 pos, ref Quaternion quat)
    {
        float w = white ? -1f : 1f;

        quat *= new Quaternion(0, 0, 90, w * 90);
    }
}
