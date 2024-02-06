using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }

    [SerializeField] private GameObject chessPiece;

    private GameObject[,] positions = new GameObject[8, 8];
    private List<GameObject> playerBlack = new List<GameObject>();
    private List<GameObject> playerWhite = new List<GameObject>();

    private Player currentPlayer;

    private bool gameOver = false;

    // List of the possible pieces
    [SerializeField] private List<ChessPieceSO> pieceList;
    // private Dictionary<ChessPiece, ChessPieceSO> pieceDictionary;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        Instance = this;

        foreach (ChessPieceSO piece in pieceList)
        {
            for (int i = 0; i < piece.StartPositions.Count; i++)
            {
                if (piece.OwnerPlayer == Player.White) // This is slow need to be improved
                {
                    playerWhite.Add(Create(piece, piece.StartPositions[i].X, piece.StartPositions[i].Y));
                }
                else
                {
                    playerBlack.Add(Create(piece, piece.StartPositions[i].X, piece.StartPositions[i].Y));
                }
            }
        }

        for (int i = 0; i < playerWhite.Count; i++)
        {
            SetPosition(playerBlack[i]);
            SetPosition(playerWhite[i]);
        }
    }

    private GameObject Create(ChessPieceSO piece, int x, int y)
    {
        GameObject retObj = Instantiate(chessPiece, new Vector3(0, 0, -1), Quaternion.identity);
        ChessPiece cm = retObj.GetComponent<ChessPiece>();

        cm.name = piece.PieceType.ToString();
        cm.XBoardPos = x;
        cm.YBoardPos = y;
        cm.Activate(piece);

        return retObj;
    }

    public void SetPosition(GameObject obj)
    {
        ChessPiece cm = obj.GetComponent<ChessPiece>();

        positions[cm.XBoardPos, cm.YBoardPos] = obj;
    }

    public void SetPositionEmpty(int x, int y)
    {
        positions[x, y] = null;
    }

    public GameObject GetPosition(int x, int y)
    {
        return positions[x, y];
    }

    // Make sure the position we whant to use are actually on the board
    public bool PositionOnBoard(int x, int y)
    {
        if (x < 0 || y < 0 || x >= positions.GetLength(0) || y >= positions.GetLength(1))
        {
            return false;
        }
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
