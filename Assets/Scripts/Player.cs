using UnityEngine;

public abstract class Player : MonoBehaviour
{
    [HideInInspector] public int playerIndex;
    public Board board;

    public abstract void BeginTurn();

    public virtual void EndTurn() { }

    public void CheckGameState()
    {
        int result = board?.CheckWinner() ?? 0;

        if (result != 0)
        {
            Player winner = null;
            if (result == 1) winner = GameManager.Instance.players[0];
            else if (result == 2) winner = GameManager.Instance.players[1];

            GameManager.Instance?.EndGame(winner);
        }
        else
        {
            GameManager.Instance?.SwitchPlayer();
        }
    }
  
}