using UnityEngine;

public class HumanPlayer : Player
{
    public override void BeginTurn()
    {
        // Human interaction is handled through Cell clicks
    }

    public void MakeMove(int cellIndex)
    {
        if (GameManager.Instance.currentPlayer != this) return;
        board.MarkCell(cellIndex, playerIndex); // Places X
        GameManager.Instance.CheckGameState();  // Critical! Switches to AI
    }
}