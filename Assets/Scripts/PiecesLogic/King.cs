using UnityEngine;

public class King : Piece
{
    void Awake()
    {
        base.OnStart();
        Type = "king";

        if (white)  GameManager.instance.whiteKingPosition = position;
        else        GameManager.instance.blackKingPosition = position;
    }

    protected override bool[,] Moves()
    {
        bool[,] moves = Piece.EmptyBoard();
        var checksBoard = white ? GameManager.instance.blackChecks : GameManager.instance.whiteChecks;

        for (int i = position.x - 1; i <= position.x + 1; i++)
        {
            for (int j = position.y - 1; j <= position.y + 1; j++)
            {
                if (i > 0 && i < 9 &&  j > 0 && j < 9)
                {
                    // empty square or enemy piece there
                    if (GameManager.instance.board[i, j] == null || 
                        GameManager.instance.board[i, j].white == !white)
                    {
                        moves[i, j] = !checksBoard[i, j];
                    }
                }
            }
        }

        // castle
        if (!moved)
        {
            for(int i = 1; i < 9; i++)
            {
                if (GameManager.instance.board[i, position.y] != null &&
                    GameManager.instance.board[i, position.y].Type == "rook" &&
                    GameManager.instance.board[i, position.y].white == white &&
                    !GameManager.instance.board[i, position.y].moved)
                {
                    bool obstacle = false;

                    // rook is on the right side
                    if (i > position.x)
                    {
                        for (int j = i-1; j > position.x; j--)
                        {
                            if(GameManager.instance.board[j, position.y] != null)
                            {
                                obstacle = true;
                                break;
                            }
                        }
                        if(GameManager.instance.board[6, position.y] != null ||
                           GameManager.instance.board[7, position.y] != null)
                            obstacle = true;
                        if (!obstacle)
                        {
                            moves[i, position.y] = true;
                        }
                    }
                    else // rook is on the left side
                    {
                        for (int j = i+1; j < position.x; j++)
                        {
                            if (GameManager.instance.board[j, position.y] != null)
                            {
                                obstacle = true;
                                break;
                            }
                        }
                        if (GameManager.instance.board[3, position.y] != null ||
                            GameManager.instance.board[4, position.y] != null)
                            obstacle = true;
                        if (!obstacle)
                        {
                            moves[i, position.y] = true;
                        }
                    }
                }
            }

        }

        return moves;
    }
    
    public override void FillChecksBoard()
    { 
        var board = white ? GameManager.instance.whiteChecks : GameManager.instance.blackChecks;

        for(int i = position.x - 1; i <= position.x + 1; i++)
        {
            for (int j = position.y - 1; j <= position.y + 1; j++)
            {
                if(i>0 && j>0 && i<9 && j < 9)
                {
                    board[i, j] = true;
                }
            }
        }
    }

    public override void Move(Vector2Int direction)
    {
        moved = true;

        if(Pieces(direction) == null || Pieces(direction).white != white)
            base.Move(direction);
        // castle
        else
        {
            var rook = Pieces(direction);

            // castle to the right
            if (direction.x > position.x)
            {
                // clear squares from the rook and the king
                GameManager.instance.board[position.x, position.y] = null;
                GameManager.instance.board[rook.position.x, rook.position.y] = null;

                // place the rook and the king on the correct places
                GameManager.instance.board[7, direction.y] = this;
                GameManager.instance.board[6, direction.y] = rook;

                // move the king and the rook objects
                position.x = 7;
                rook.position.x = 6;
                transform.position = new Vector3(position.x, position.y, 0);
                rook.transform.position = new Vector3(rook.position.x, rook.position.y, 0);

            }
            // castle to the left
            else
            {
                // clear squares from the rook and the king
                GameManager.instance.board[position.x, position.y] = null;
                GameManager.instance.board[rook.position.x, rook.position.y] = null;

                // place the rook and the king on the correct places
                GameManager.instance.board[3, direction.y] = this;
                GameManager.instance.board[4, direction.y] = rook;

                // move the king and the rook objects
                position.x = 3;
                rook.position.x = 4;
                transform.position = new Vector3(position.x, position.y, 0);
                rook.transform.position = new Vector3(rook.position.x, rook.position.y, 0);
            }
        }

        if (white) GameManager.instance.whiteKingPosition = position;
        else GameManager.instance.blackKingPosition = position;

        GameManager.instance.AfterMove();
    }
}