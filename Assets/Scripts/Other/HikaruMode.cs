using UnityEngine;

public class HikaruMode : MonoBehaviour
{
    bool white = false, black = false;

    private int _lastCheck = -1;
    private readonly int _hikaruTurn = 6, _carlsenTurn = 12;

    private Piece wQueen, bQueen, wKing, bKing;

    void Start()
    {
        Variables();
    }

    void Update()
    {
        // don't check every frame, but every turn
        if (_lastCheck == GameManager.instance.turnNumber)
            return;
        _lastCheck = GameManager.instance.turnNumber;

        // check for Hikaru mode
        if(_lastCheck <= _hikaruTurn)
            CheckHikaru();
        if(_lastCheck <= _carlsenTurn)
            CheckCarlsen();
    }

    private void Variables()
    {
        // kings
        bKing = GameManager.instance.Board(GameManager.instance.blackKingPosition);
        wKing = GameManager.instance.Board(GameManager.instance.whiteKingPosition);

        // queens
        foreach (Piece piece in GameManager.instance.pieces)
        {
            if (piece.Type == "queen")
            {
                if(piece.white)
                    wQueen = piece;
                else
                    bQueen = piece;
            }
        }

        if(wQueen == null ||  bQueen == null)
        {
            Debug.Log($"ERROR HIKARU MODE Queen not found " +
                $"{(wQueen == null ? 'w' : ' ')} {(bQueen == null ? 'b' : ' ')}");
        }
        if (bKing == null || wKing == null)
        {
            Debug.Log("ERROR HIKARU MODE King not found");
        }
    }

    private void CheckHikaru()
    {
        if (GameManager.instance.turnNumber <= 6)
        {
            if (GameManager.instance.whiteKingPosition.y == 3 && !white)
            {
                white = true;
                ImHikaru(true);
                GameManager.instance.RefreshChecksBoard(true);
            }
            if (GameManager.instance.blackKingPosition.y == 6 && !black)
            {
                black = true;
                ImHikaru(false);
                GameManager.instance.RefreshChecksBoard(false);
            }
        }
    }

    private void CheckCarlsen()
    {
        Vector2Int wKingPos = new Vector2Int(4, 1);
        Vector2Int bKingPos = new Vector2Int(4, 8);

        Vector2Int wQueenPos = new Vector2Int(5, 1);
        Vector2Int bQueenPos = new Vector2Int(5, 8);

        // white
        if (wKing.position == wKingPos && wQueen.position == wQueenPos)
        {
            ImCarlsen(true);
        }

        if(bKing.position == bKingPos && bQueen.position == bQueenPos)
        {
            ImCarlsen(false);
        }
    }

    private void ImHikaru(bool white)
    {
        foreach (Piece piece in GameManager.instance.pieces)
        {
            if(piece.white == white && piece.Type != "king")
            {
                piece.ChangeType("knight");
            }
        }

        GameManager.instance.RefreshOnBoardPieces();

        wQueen = null;
        bQueen = null;
    }

    private void ImCarlsen(bool white)
    {
        Debug.Log((white ? "White" : "Black") + " is Carlsen");
        Destroy(bQueen.gameObject);
        Destroy(wQueen.gameObject);
    }
}
