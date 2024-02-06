using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPiece : MonoBehaviour
{
    // References
    [SerializeField] private GameObject movePlate;

    // Positions
    public int XBoardPos { get; set; } = -1;
    public int YBoardPos { get; set; } = -1;

    // Keep track of "balck/white" player
    public Player Owner { get; private set; } = Player.White;

    private ChessPieceSO pieceData;

    public void Activate(ChessPieceSO thisPiece)
    {
        SetCoords();

        pieceData = thisPiece;
        Owner = pieceData.OwnerPlayer;
        GetComponent<SpriteRenderer>().sprite = thisPiece.PieceSprite;
    }

    public void SetCoords()
    {        
        transform.position = WorldPiecePosition(XBoardPos, YBoardPos, -1f);
    }

    private void OnMouseUp()
    {
        if (Game.Instance.CurrentPlayer != Owner) return;

        DestroyMovePlates();

        InitializeMovePlates();
    }

    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]);
        }
    }

    public void InitializeMovePlates()
    {
       switch (pieceData.MovementType)
       {
            case MoveType.LineMove:
                for (int i = 0; i < pieceData.PossibleLineMoveDirections.Count; i++)
                {
                    LineMovePlate(pieceData.PossibleLineMoveDirections[i].X, pieceData.PossibleLineMoveDirections[i].Y);
                }
                break;
            case MoveType.LMove:
                LMovePlate();
                break;
            case MoveType.SurroundMove:
                SurroundMovePlate();
                break;
            case MoveType.PawnMove: 
                PawnMovePlate(XBoardPos, Owner == Player.White ? (YBoardPos + 1) : (YBoardPos - 1));
                break;
       }
    }

    private void LineMovePlate(int xIncrement, int yIncrement)
    {
        int x = XBoardPos + xIncrement;
        int y = YBoardPos + yIncrement;

        while (Game.Instance.PositionOnBoard(x, y) && Game.Instance.GetPosition(x, y) == null)
        {
            SpawnMovePlate(x, y);
            x += xIncrement;
            y += yIncrement;
        }

        if (Game.Instance.PositionOnBoard(x, y) && Game.Instance.GetPosition(x, y).GetComponent<ChessPiece>().Owner != Owner)
        {
            SpawnMovePlate(x, y, true);
        }
    }

    private void LMovePlate()
    {
        PointMovePlate(XBoardPos + 1, YBoardPos + 2);
        PointMovePlate(XBoardPos - 1, YBoardPos + 2);
        PointMovePlate(XBoardPos + 2, YBoardPos + 1);
        PointMovePlate(XBoardPos + 2, YBoardPos - 1);
        PointMovePlate(XBoardPos + 1, YBoardPos - 2);
        PointMovePlate(XBoardPos - 1, YBoardPos - 2);
        PointMovePlate(XBoardPos - 2, YBoardPos + 1);
        PointMovePlate(XBoardPos - 2, YBoardPos - 1);
    }

    private void SurroundMovePlate()
    {
        PointMovePlate(XBoardPos, YBoardPos + 1);
        PointMovePlate(XBoardPos, YBoardPos - 1);
        PointMovePlate(XBoardPos - 1, YBoardPos - 1);
        PointMovePlate(XBoardPos - 1, YBoardPos);
        PointMovePlate(XBoardPos - 1, YBoardPos + 1);
        PointMovePlate(XBoardPos + 1, YBoardPos - 1);
        PointMovePlate(XBoardPos + 1, YBoardPos);
        PointMovePlate(XBoardPos + 1, YBoardPos + 1);
    }

    private void PointMovePlate(int x, int y)
    {
        if (!Game.Instance.PositionOnBoard(x, y)) return;

        GameObject chessPiece = Game.Instance.GetPosition(x, y);

        if (chessPiece == null)
        {
            SpawnMovePlate(x, y);
        }
        else if (chessPiece.GetComponent<ChessPiece>().Owner != Owner)
        {
            SpawnMovePlate(x, y, true);
        }
    }

    private void PawnMovePlate(int x, int y)
    {
        if (!Game.Instance.PositionOnBoard(x, y)) return;

        GameObject chessPiece = Game.Instance.GetPosition(x, y);

        if (chessPiece == null)
        {
            SpawnMovePlate(x, y);
        }

        chessPiece = Game.Instance.GetPosition(x + 1, y);
        if (chessPiece != null && chessPiece.GetComponent<ChessPiece>().Owner != Owner)
        {
            SpawnMovePlate(x + 1, y, true);
        }

        chessPiece = Game.Instance.GetPosition(x - 1, y);
        if (chessPiece != null && chessPiece.GetComponent<ChessPiece>().Owner != Owner)
        {
            SpawnMovePlate(x - 1, y, true);
        }
    }

    private void SpawnMovePlate(int inputX, int inputY, bool isAttack = false)
    {
        MovePlate mp = Instantiate(movePlate, WorldPiecePosition(inputX, inputY, -3), Quaternion.identity).GetComponent<MovePlate>();
        mp.Initialize(this, inputX, inputY, isAttack);
    }

    private Vector3 WorldPiecePosition(float x, float y, float z)
    {
        x *= 0.66f;
        y *= 0.66f;

        x += -2.31f;
        y += -2.31f;

        return new Vector3(x, y, -1);
    }
}

public enum Player
{
    White,
    Black
}
