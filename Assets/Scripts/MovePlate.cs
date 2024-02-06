using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    [SerializeField] private Color MoveColor;
    [SerializeField] private Color AttackColor;
    [SerializeField] private Color SelectedPieceColor;

    public ChessPiece MovingPiece { get; set; } = null;

    public int XBoardPos { get; private set; } = -1;
    public int YBoardPos { get; private set; } = -1;

    [SerializeField] private bool attackPlate = false;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update

    public void Initialize(ChessPiece movingPiece, int xPos, int yPos, bool isAttack = false, bool isPlaying = false)
    {
        attackPlate = isAttack;
        MovingPiece = movingPiece;
        XBoardPos = xPos;
        YBoardPos = yPos;

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = isAttack ? AttackColor : isPlaying ? SelectedPieceColor : MoveColor;
    }

    private void OnMouseUp()
    {
        Game.Instance.SwitchTurn();

        if (attackPlate)
        {
            GameObject chessPiece = Game.Instance.GetPosition(XBoardPos, YBoardPos);

            Destroy(chessPiece);
        }

        Game.Instance.SetPositionEmpty(MovingPiece.XBoardPos, MovingPiece.YBoardPos);

        // Update piece position on the board
        MovingPiece.XBoardPos = XBoardPos;
        MovingPiece.YBoardPos = YBoardPos;
        MovingPiece.SetCoords();

        Game.Instance.SetPosition(MovingPiece.gameObject);

        MovingPiece.DestroyMovePlates();
    }
}
