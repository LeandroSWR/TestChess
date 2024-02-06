using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ChessPiece", menuName = "ChessPiece")]
public class ChessPieceSO : ScriptableObject
{
    [Serializable]
    public struct IntVector
    {
        [SerializeField] private int x;
        public int X => x;
        [SerializeField] private int y;
        public int Y => y;
    }

    [SerializeField] private Player owner;
    public Player OwnerPlayer => owner;

    [SerializeField] private ChessPieceType pieceType;
    public ChessPieceType PieceType => pieceType;

    [SerializeField] private MoveType moveType;
    public MoveType MovementType => moveType;

    [SerializeField] private Sprite sprite;
    public Sprite PieceSprite => sprite;

    [SerializeField] private List<IntVector> possibleStartPositions;
    public List<IntVector> StartPositions => possibleStartPositions;

    [SerializeField] private List<IntVector> possibleLineMoveDir;
    public List<IntVector> PossibleLineMoveDirections => possibleLineMoveDir;
}

public enum MoveType
{
    LineMove,
    SurroundMove,
    LMove,
    PawnMove
}

public enum ChessPieceType
{
    Black_Queen,
    Black_Knight,
    Black_King,
    Black_Bishop,
    Black_Rook,
    Black_Pawn,
    White_Queen,
    White_Knight,
    White_King,
    White_Bishop,
    White_Rook,
    White_Pawn
}