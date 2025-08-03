using UnityEngine;

public abstract class Player : MonoBehaviour
{
    [HideInInspector] public int playerIndex;
    [HideInInspector] public Board board;

    public abstract void BeginTurn();

    public virtual void EndTurn() { }

    protected void CheckGameState()
    {
        int result = board.CheckWinner();

        if (result != 0)
        {
            Player winner = result == -1 ? null :
                GameManager.Instance.players[result - 1];
            GameManager.Instance.EndGame(winner);
        }
        else
        {
            GameManager.Instance.SwitchPlayer();
        }
    }
}