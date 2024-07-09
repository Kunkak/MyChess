using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Piece[,] board = new Piece[9, 9];
    public List<Piece> pieces = new();

    public Piece theChosenOne;
    public bool playing;
    public bool[,] whiteChecks, blackChecks;
    public Vector2Int blackKingPosition, whiteKingPosition;
    public int turnNumber;
    public GameObject[] buttons;
    public GameObject camera2D, camera3D;

    private bool _whiteTurn { get; set; }
    private bool _3DActive { get; set; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        SetPieces();
    }

    void Start()
    {
        playing = true;
        turnNumber = 0;
        FunModes();

        RefreshChecksBoard();
    }
   
    public void AfterMove()
    {
        RefreshChecksBoard();
        turnNumber++;
        _whiteTurn = !whiteTurn;
        StartCoroutine(nameof(CheckResult));

        //ChangeTo3D();
        theChosenOne = null;
    }

    public void OnClick(Vector2Int position)
    {
        // click on a piece when there is no piece clicked
        if(theChosenOne == null && board[position.x, position.y] != null &&
            board[position.x, position.y].white == whiteTurn)
        {
            theChosenOne = board[position.x, position.y];
            Board2D.instance.GreenSquares(theChosenOne.possibleMoves, theChosenOne.position);
        }
        // click on the possible square
        else if(theChosenOne != null && Board2D.instance.possibleSquares[position.x, position.y])
        {
            if (Settings.instance.GameModeKidsDream)
            {
                ChangeTo3D();
                Board3D.instance.GreenSquares();
                Board2D.instance.ResetColors();
            }
            else if (Settings.instance.GameModeNormal)
            {
                theChosenOne.Move(position);
                Board2D.instance.ResetColors();
            }
            
        }
        // click on a piece when there is same color piece clicked
        else if (theChosenOne != null && board[position.x, position.y] != null &&
            board[position.x, position.y].white == theChosenOne.white)
        {
            theChosenOne = board[position.x, position.y];
            Board2D.instance.GreenSquares(theChosenOne.possibleMoves, theChosenOne.position);
        }
        // click on the impossible square
        else if(theChosenOne != null && !Board2D.instance.possibleSquares[position.x, position.y])
        {
            theChosenOne = null;
            Board2D.instance.ResetColors();
        }
    }

    public bool whiteTurn {  get => _whiteTurn; }

    public bool is3DActive { get => _3DActive; }

    private void SetPieces()
    {
        _whiteTurn = true;

        var BPawn = Resources.Load("Prefabs/2D/Black pieces/BPawn");
        var BKnight = Resources.Load("Prefabs/2D/Black pieces/BKnight");
        var BBishop = Resources.Load("Prefabs/2D/Black pieces/BBishop");
        var BRook = Resources.Load("Prefabs/2D/Black pieces/BRook");
        var BKing = Resources.Load("Prefabs/2D/Black pieces/BKing");
        var BQueen = Resources.Load("Prefabs/2D/Black pieces/BQueen");

        var WPawn = Resources.Load("Prefabs/2D/White pieces/WPawn");
        var WKnight = Resources.Load("Prefabs/2D/White pieces/WKnight");
        var WBishop = Resources.Load("Prefabs/2D/White pieces/WBishop");
        var WRook = Resources.Load("Prefabs/2D/White pieces/WRook");
        var WKing = Resources.Load("Prefabs/2D/White pieces/WKing");
        var WQueen = Resources.Load("Prefabs/2D/White pieces/WQueen");


        board[1, 1] = Instantiate(WRook, new Vector3(1, 1, 0), Quaternion.identity).GetComponent<Piece>();
        board[2, 1] = Instantiate(WKnight, new Vector3(2, 1, 0), Quaternion.identity).GetComponent<Piece>();
        board[3, 1] = Instantiate(WBishop, new Vector3(3, 1, 0), Quaternion.identity).GetComponent<Piece>();
        board[4, 1] = Instantiate(WQueen, new Vector3(4, 1, 0), Quaternion.identity).GetComponent<Piece>();
        board[5, 1] = Instantiate(WKing, new Vector3(5, 1, 0), Quaternion.identity).GetComponent<Piece>();
        board[6, 1] = Instantiate(WBishop, new Vector3(6, 1, 0), Quaternion.identity).GetComponent<Piece>();
        board[7, 1] = Instantiate(WKnight, new Vector3(7, 1, 0), Quaternion.identity).GetComponent<Piece>();
        board[8, 1] = Instantiate(WRook, new Vector3(8, 1, 0), Quaternion.identity).GetComponent<Piece>();

        board[1, 8] = Instantiate(BRook, new Vector3(1, 8, 0), Quaternion.identity).GetComponent<Piece>();
        board[2, 8] = Instantiate(BKnight, new Vector3(2, 8, 0), Quaternion.identity).GetComponent<Piece>();
        board[3, 8] = Instantiate(BBishop, new Vector3(3, 8, 0), Quaternion.identity).GetComponent<Piece>();
        board[4, 8] = Instantiate(BQueen, new Vector3(4, 8, 0), Quaternion.identity).GetComponent<Piece>();
        board[5, 8] = Instantiate(BKing, new Vector3(5, 8, 0), Quaternion.identity).GetComponent<Piece>();
        board[6, 8] = Instantiate(BBishop, new Vector3(6, 8, 0), Quaternion.identity).GetComponent<Piece>();
        board[7, 8] = Instantiate(BKnight, new Vector3(7, 8, 0), Quaternion.identity).GetComponent<Piece>();
        board[8, 8] = Instantiate(BRook, new Vector3(8, 8, 0), Quaternion.identity).GetComponent<Piece>();

        board[1, 2] = Instantiate(WPawn, new Vector3(1, 2, 0), Quaternion.identity).GetComponent<Piece>();
        board[2, 2] = Instantiate(WPawn, new Vector3(2, 2, 0), Quaternion.identity).GetComponent<Piece>();
        board[3, 2] = Instantiate(WPawn, new Vector3(3, 2, 0), Quaternion.identity).GetComponent<Piece>();
        board[4, 2] = Instantiate(WPawn, new Vector3(4, 2, 0), Quaternion.identity).GetComponent<Piece>();
        board[5, 2] = Instantiate(WPawn, new Vector3(5, 2, 0), Quaternion.identity).GetComponent<Piece>();
        board[6, 2] = Instantiate(WPawn, new Vector3(6, 2, 0), Quaternion.identity).GetComponent<Piece>();
        board[7, 2] = Instantiate(WPawn, new Vector3(7, 2, 0), Quaternion.identity).GetComponent<Piece>();
        board[8, 2] = Instantiate(WPawn, new Vector3(8, 2, 0), Quaternion.identity).GetComponent<Piece>();

        board[1, 7] = Instantiate(BPawn, new Vector3(1, 7, 0), Quaternion.identity).GetComponent<Piece>();
        board[2, 7] = Instantiate(BPawn, new Vector3(2, 7, 0), Quaternion.identity).GetComponent<Piece>();
        board[3, 7] = Instantiate(BPawn, new Vector3(3, 7, 0), Quaternion.identity).GetComponent<Piece>();
        board[4, 7] = Instantiate(BPawn, new Vector3(4, 7, 0), Quaternion.identity).GetComponent<Piece>();
        board[5, 7] = Instantiate(BPawn, new Vector3(5, 7, 0), Quaternion.identity).GetComponent<Piece>();
        board[6, 7] = Instantiate(BPawn, new Vector3(6, 7, 0), Quaternion.identity).GetComponent<Piece>();
        board[7, 7] = Instantiate(BPawn, new Vector3(7, 7, 0), Quaternion.identity).GetComponent<Piece>();
        board[8, 7] = Instantiate(BPawn, new Vector3(8, 7, 0), Quaternion.identity).GetComponent<Piece>();

        RefreshOnBoardPieces();
    }

    public void RefreshOnBoardPieces()
    {
        pieces = new List<Piece>();

        GameObject piecesParent = GameObject.Find("PiecesParent");
        if (piecesParent == null)
        {
            piecesParent = new GameObject("PiecesParent");
        }    
        
        for (int i = 1; i < 9; i++)
        {
            for (int j = 1; j < 9; j++)
            {
                if (board[i, j] != null)
                {
                    board[i, j].transform.SetParent(piecesParent.transform);
                    pieces.Add(board[i, j]);
                }
            }
        }
    }

    public bool? CheckPiece(int column, int row)
    {
        if (board[column, row] == null)
        {
            return null;
        }
        else
        {
            return board[column, row].white;
        }
    }

    public void RefreshChecksBoard()
    {
        whiteChecks = Piece.EmptyBoard();
        blackChecks = Piece.EmptyBoard();
        //for (int i = 0; i < 9; i++)
        //{
        //    for (int j = 0; j < 9; j++)
        //    {
        //        if (pieces[i, j] != null)
        //        {
        //            pieces[i, j].FillChecksBoard();
        //        }
        //    }
        //}
    }

    public void RefreshChecksBoard(bool white)
    {
        if (white)
            whiteChecks = Piece.EmptyBoard();
        else
            blackChecks = Piece.EmptyBoard();

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (board[i, j] != null && board[i, j].white == white)
                {
                    board[i, j].FillChecksBoard();
                }
            }
        }
    }

    public bool ChecksAfterMove(Vector2Int from, Vector2Int to)
    {
        bool enPassant = false;
        if (board[from.x, from.y] == null) 
            return false;
        // en passant is the only move in chess which takes the piece from an other square
        // than the direction
        if (board[from.x, from.y].Type == "pawn" &&
            from.x != to.x &&
            board[to.x, to.y] == null)
        {
            enPassant = true;
        }

        bool tempWhite = board[from.x, from.y].white;
        Vector2Int kingPos = tempWhite ? whiteKingPosition : blackKingPosition;
        bool[,] blackChecksBoard = blackChecks;
        bool[,] whiteChecksBoard = whiteChecks;
        Piece temp = board[to.x, to.y], temp2 = null;

        // move piece, then refresh checks board
        board[to.x, to.y] = board[from.x, from.y];
        board[from.x, from.y] = null;
        if (enPassant)
        {
            temp2 = board[from.x, to.y];
            board[from.x, to.y] = null;
        }
        RefreshChecksBoard(!board[to.x, to.y].white);

        // check if there is a check after moving
        bool ans = tempWhite ? blackChecks[kingPos.x, kingPos.y] : whiteChecks[kingPos.x, kingPos.y];

        // return boards to state before moving
        blackChecks = blackChecksBoard;
        whiteChecks = whiteChecksBoard;
        if (enPassant)
        {
            board[from.x, to.y] = temp2;
        }
        board[from.x, from.y] = board[to.x, to.y];
        board[to.x, to.y] = temp;

        return ans;
    }

    public bool CheckAfterDisapearing(Vector2Int from)
    {
        if (board[from.x, from.y] == null) return false;

        bool tempWhite = board[from.x, from.y].white;
        Vector2Int kingPos = tempWhite ? whiteKingPosition : blackKingPosition;
        bool[,] blackChecksBoard = blackChecks;
        bool[,] whiteChecksBoard = whiteChecks;
        Piece temp = board[from.x, from.y];

        // vanish piece, then refresh checks board
        board[from.x, from.y] = null;
        RefreshChecksBoard(tempWhite);

        // check if there is a check after vanishing
        bool ans = tempWhite ? blackChecks[kingPos.x, kingPos.y] : whiteChecks[kingPos.x, kingPos.y];

        // return board to state before disapearing
        blackChecks = blackChecksBoard;
        whiteChecks = whiteChecksBoard;
        board[from.x, from.y] = temp;

        return ans;
    }

    public bool IsKingChecked(bool white)
    {
        Vector2Int pos;
        bool[,] checks;
        if (white)
        {
            pos = whiteKingPosition;
            checks = blackChecks;
        }
        else
        {
            pos = blackKingPosition;
            checks = whiteChecks;
        }
        
        return checks[pos.x, pos.y];
    }

    private bool? IsMated(bool white) 
        // true - the king is checkmated
        // false - the king is not checkmated
        // null - there is a stalemate
    {
        Vector2Int pos = white ? whiteKingPosition : blackKingPosition;
        Piece king = board[pos.x, pos.y];
        var moves = king.possibleMoves;

        // find if the king can move
        for (int i = 1; i < 9; i++)
        {
            for(int j = 1; j < 9; j++)
            {
                if (moves[i, j])
                {
                    return false;
                }
                    
            }
        }

        for (int i = 1; i < 9; i++)
        {
            for (int j = 1; j < 9; j++)
            {
                // find every piece and check if it can rescue the king
                if (board[i, j] != null && 
                    board[i, j].white == white &&
                    board[i, j].Type != "king")
                {
                    moves = board[i, j].possibleMoves;
                    for (int k = 1; k < 9; k++)
                    {
                        for (int l = 1; l < 9; l++)
                        {
                            // is there any possibility to cover the king?
                            if (moves[k, l])
                            {
                                return false;
                            }
                        }
                    }
                }
            }
        }

        // if code reaches here, there is the checkmate or the stalemate
        var checksBoard = white ? blackChecks : whiteChecks;
        if (checksBoard[pos.x, pos.y])
            return true;
        return null;
    }

    private IEnumerator CheckResult()
    {
        yield return null;
        GameOver(ResultNumber());
    }

    public void GameOver(int result)
    {
        // 10 - continue
        // 1 - black is checkmated
        // 2 - black run out of time
        // 3 - black is stalemated
        // 0 - draw by agreement
        //-1 - white is checkmated
        //-2 - white run out of time
        //-3 - white is stalemated


        if (result == 10) return;

        switch (result)
        {
            case 0:
                Debug.Log("Draw");
                break;
            case 1:
                Debug.Log("Checkmate, white wins");
                break;
            case 2:
                Debug.Log("Black ran out of time");
                break;
            case 3:
                Debug.Log("Black is stalemated");
                break;
            case -1:
                Debug.Log("Checkmate, black wins");
                break;
            case -2:
                Debug.Log("White ran out of time");
                break;
            case -3:
                Debug.Log("White is stalemated");
                break;
        }

        foreach(var button in buttons)
            button.SetActive(true);
        GameManager.instance.playing = false;
    }

    private int ResultNumber()
    {
        var result = IsMated(whiteTurn);
        if (result == false) return 10;
        int white = whiteTurn ? -1 : 1;
        if (result == null)
            return 3 * white;
        return 1 * white;
    }

    public void ResetBtn()
    {
        string scene;

        if (Settings.instance.GameModeKidsDream)
        {
            scene = "KidsDream";
        }
        else if (Settings.instance.GameModeNormal)
        {
            scene = "Normal";
        }
        else 
        {
            scene = "Menu";
        }

        SceneManager.LoadScene(scene);
    }

    public void Back2MenuBtn()
    {
        SceneManager.LoadScene("Menu");
    }

    public Piece Board(Vector2Int square) => board[square.x, square.y];

    public void SwitchCameraTo2D()
    {
        camera3D.SetActive(false);
        camera2D.SetActive(true);
        camera3D.GetComponent<Camera>().enabled = false;
        camera2D.GetComponent<Camera>().enabled = true;
    }

    public void SwitchCameraTo3D()
    {
        camera3D.SetActive(true);
        camera2D.SetActive(false);
        camera3D.GetComponent<Camera>().enabled = true;
        camera2D.GetComponent<Camera>().enabled = false;
    }

    public void ChangeTo3D()
    {
        Clock.instance.Set3DClock();

        SwitchCameraTo3D();
        
        Board3D.instance.ResetPosition();

        foreach(var p in pieces)
        {
            p.Create3DPiece();
        }

        _3DActive = true;

        theChosenOne.piece3D.ActivateRigidbody();
    }

    public void ChangeTo2D()
    {
        Clock.instance.Set2DClock();

        SwitchCameraTo2D();

        theChosenOne.Move(Board3D.instance.FindClosestSquare());

        foreach (var p in pieces)
        {
            Destroy(p.piece3D.gameObject);
        }

        _3DActive = false;
    }

    public void FunModes()
    {
        if (Settings.instance.funModes)
        {
            GameObject easterEggs = new GameObject("Eeaster Eggs");

            easterEggs.AddComponent<HikaruMode>();
        }
    }
}