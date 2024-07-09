using System;
using UnityEngine;

public class Pawn : Piece
{
    public int enPassant = -1;
    void Awake()
    {
        base.OnStart();
        type = "pawn";
    }

    protected override bool[,] Moves()
    {
        bool[,] moves = Piece.EmptyBoard();
        
        if (white) // white pawn
        {
            // first move
            if (position.y == 2)
            {
                // third row free?
                if (GameManager.instance.CheckPiece(position.x, 3) == null)
                {
                    moves[position.x, 3] = true;
                    // fourth row free?
                    if (GameManager.instance.CheckPiece(position.x, 4) == null)
                        moves[position.x, 4] = true;                 
                }
            }
            else // not first move
            {
                // next row free?
                if (GameManager.instance.CheckPiece(position.x, position.y + 1) == null)
                {
                    moves[position.x, position.y + 1] = true;
                }
            }

            // not first column
            if (position.x > 1)
            {
                // attack to the left
                if (GameManager.instance.CheckPiece(position.x - 1, position.y + 1) == !white)
                {
                    moves[position.x - 1, position.y + 1] = true;
                }
                else if (position.y == 5 &&
                    GameManager.instance.board[position.x - 1, position.y] != null &&
                    GameManager.instance.board[position.x - 1, position.y].Type == "pawn" &&
                    GameManager.instance.board[position.x - 1, position.y].GetComponent<Pawn>().enPassant >= GameManager.instance.turnNumber - 1)
                {
                    moves[position.x - 1, position.y + 1] = true;
                }
            }
            // not last column
            if (position.x < 8)
            {
                // attack to the right
                if (GameManager.instance.CheckPiece(position.x + 1, position.y + 1) == !white)
                {
                    moves[position.x + 1, position.y + 1] = true;
                }
                else if (position.y == 5 &&
                    GameManager.instance.board[position.x + 1, position.y] != null &&
                    GameManager.instance.board[position.x + 1, position.y].Type == "pawn" &&
                    GameManager.instance.board[position.x + 1, position.y].GetComponent<Pawn>().enPassant >= GameManager.instance.turnNumber - 1)
                {
                    moves[position.x + 1, position.y + 1] = true;
                }
            }
        }
        else // black pawn
        {
            // first move
            if (position.y == 7)
            {
                // sixth row free?
                if (GameManager.instance.CheckPiece(position.x, 6) == null)
                {
                    moves[position.x, 6] = true;
                    // fifth row free?
                    if (GameManager.instance.CheckPiece(position.x, 5) == null)
                        moves[position.x, 5] = true;
                }
            }
            else // not first move
            {
                // next row free?
                if (GameManager.instance.CheckPiece(position.x, position.y - 1) == null)
                {
                    moves[position.x, position.y - 1] = true;
                }
                    
            }

            // not first column
            if (position.x > 1)
            {
                // attack to the left
                if (GameManager.instance.CheckPiece(position.x - 1, position.y - 1) == !white)
                {
                    moves[position.x - 1, position.y - 1] = true;
                }
                else if (position.y == 4 &&
                    GameManager.instance.board[position.x - 1, position.y] != null &&
                    GameManager.instance.board[position.x - 1, position.y].Type == "pawn" &&
                    GameManager.instance.board[position.x - 1, position.y].GetComponent<Pawn>().enPassant >= GameManager.instance.turnNumber - 1)
                {
                    moves[position.x - 1, position.y - 1] = true;
                }
            }
            // not last column
            if (position.x < 8)
            {
                // attack to the right
                if (GameManager.instance.CheckPiece(position.x + 1, position.y - 1) == !white)
                {
                    moves[position.x + 1, position.y - 1] = true;
                }
                else if (position.y == 4 &&
                    GameManager.instance.board[position.x + 1, position.y] != null &&
                    GameManager.instance.board[position.x + 1, position.y].Type == "pawn" &&
                    GameManager.instance.board[position.x + 1, position.y].GetComponent<Pawn>().enPassant >= GameManager.instance.turnNumber - 1)
                {
                    moves[position.x + 1, position.y - 1] = true;
                }
            }
        }

        return moves;
    }

    public override void FillChecksBoard()
    {
        var board = white ? GameManager.instance.whiteChecks : GameManager.instance.blackChecks;

        if (position.y == 1 || position.y == 8)
        {
            ChangeType("queen");
            return;
        }
            

        if (white)
        {
            if (position.x > 1)
            {
                board[position.x - 1, position.y + 1] = true;
            }
            if (position.x < 8)
            {
                board[position.x + 1, position.y + 1] = true;
            }
        }
        else
        {
            if (position.x > 1)
            {
                board[position.x - 1, position.y - 1] = true;
            }
            if (position.x < 8)
            {
                board[position.x + 1, position.y - 1] = true;
            }
        }
    }

    public override void Move(Vector2Int direction)
    {
        if (Math.Abs(direction.y - position.y) > 1)
            enPassant = GameManager.instance.turnNumber;

        if(position.x == direction.x || Pieces(direction) != null)
        {
            base.Move(direction);
            return;
        }
        // en passant
        else
        {
            Destroy(GameManager.instance.board[direction.x, position.y].gameObject);
            GameManager.instance.board[direction.x, position.y] = null;

            GameManager.instance.board[direction.x, direction.y] = this;
            GameManager.instance.board[position.x, position.y] = null;
            position = direction;

            transform.position = new Vector3(position.x, position.y, 0f);

            GameManager.instance.AfterMove();
        }
    }

    protected override void Adjust(ref Vector3 pos, ref Quaternion quat)
    {
        pos += new Vector3(0, 1, 0);
    }
}
