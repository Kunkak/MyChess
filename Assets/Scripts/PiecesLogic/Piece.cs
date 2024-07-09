using Unity.VisualScripting;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    public bool white;
    public Vector2Int position;
    public bool moved;
    protected int _turnNumber = -1;
    public bool[,] possibleMoves;

    public Piece3D piece3D;

    protected string type { get; set; }

    protected void OnStart()
    {
        position = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        white = position[1] <= 4;
        moved = false;
    }

    private void Update()
    {
        if(_turnNumber < GameManager.instance.turnNumber)
        {
            FillChecksBoard();
            possibleMoves = PossibleMoves();
        }
        
    }
    private void LateUpdate()
    {
        if (_turnNumber < GameManager.instance.turnNumber)
        {
            _turnNumber = GameManager.instance.turnNumber;
            possibleMoves = PossibleMoves();
        }
    }

    public void OnDestroy()
    {
        GameManager.instance.pieces.Remove(this);
    }

    public string Type
    {
        get { return type; }
        set 
        {
            string[] values = { "pawn", "rook", "knight", "bishop", "queen", "king" };
            foreach(string v in values)
            {
                if(value == v)
                {
                    type = value;
                    return;
                }
            }
            type = "error";
        }
    }

    public static bool[,] EmptyBoard()
    {
        bool[,] board = new bool[9, 9];
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                board[i, j] = false;
            }
        }
        return board;
    }

    public virtual void Move(Vector2Int direction)
    {
        moved = true;

        // takes
        if (GameManager.instance.board[direction.x, direction.y] != null)
        {
            Destroy(GameManager.instance.board[direction.x, direction.y].gameObject);

            GameManager.instance.board[direction.x, direction.y] = GameManager.instance.board[position[0], position[1]];
            GameManager.instance.board[position[0], position[1]] = null;
            position[0] = direction.x;
            position[1] = direction.y;

            transform.position = new Vector3(direction.x, direction.y, 0);
        }
        // move to the free square
        else
        {
            GameManager.instance.board[direction.x, direction.y] = GameManager.instance.board[position.x, position.y];
            GameManager.instance.board[position.x, position.y] = null;
            position = direction;

            transform.position = new Vector3(direction.x, direction.y, 0);
        }

        if(type != "king")
            GameManager.instance.AfterMove();
    }

    public bool[,] PossibleMoves()
    {
        bool[,] possibleMoves = Moves();

        if (type != "king")
        {
            for (int i = 1; i < 9; i++)
            {
                for (int j = 1; j < 9; j++)
                {
                    if (possibleMoves[i, j] && GameManager.instance.ChecksAfterMove(position, new Vector2Int(i, j)))
                    {
                        possibleMoves[i, j] = false;
                    }
                }
            }
        }

        return possibleMoves;
    }

    public void ChangeType(string to)
    {
        Object newPiece;
        Vector3 position3 = new Vector3(position.x, position.y, 0);

        string color = white ? "White" : "Black";
        newPiece = to switch
        {
            "queen" => Resources.Load($"Prefabs/2D/{color} pieces/{color[0]}Queen"),
            "rook" => Resources.Load($"Prefabs/2D/{color} pieces/{color[0]}Rook"),
            "bishop" => Resources.Load($"Prefabs/2D/{color} pieces/{color[0]}Bishop"),
            "knight" => Resources.Load($"Prefabs/2D/{color} pieces/{color[0]}Knight"),
            "pawn" => Resources.Load($"Prefabs/2D/{color} pieces/{color[0]}Pawn"),
            _ => Resources.Load($"Prefabs/2D/{color} pieces/{color[0]}Queen"),
        };

        Destroy(gameObject);
        type = to;
        GameManager.instance.board[position.x, position.y] = 
            Instantiate(newPiece, position3, Quaternion.identity).GetComponent<Piece>();
        GameManager.instance.board[position.x, position.y].white = white;
    }

    protected abstract bool[,] Moves();

    public abstract void FillChecksBoard();

    protected Piece Pieces(Vector2Int direction) => 
        GameManager.instance.board[direction.x, direction.y];

    protected virtual string Path3D()
    {
        var w = white ? "White" : "Black";
        return $"Prefabs/3D/Pieces/Chess {type} {w}";
    }

    protected virtual void Adjust(ref Vector3 pos, ref Quaternion quat) { }

    public void Create3DPiece()
    {
        var piece = Resources.Load(Path3D());
        Vector3 position3D = new Vector3(position.x, 0, position.y) * Board3D.instance.scale + Board3D.instance.offset;
        Quaternion quaternion = Board3D.instance.transform.rotation;
        Adjust(ref position3D, ref quaternion);
        GameObject representation3D = Instantiate(piece, position3D, quaternion).GameObject();
        piece3D = representation3D.AddComponent<Piece3D>();

        piece3D.DeactivateRigidbody();
    }


}
