using UnityEngine;

public class GameReset : MonoBehaviour
{
    public void ResetGame()
    {
        // Reset the board
        Board board = FindObjectOfType<Board>();
        if (board != null)
        {
            board.ResetBoard();
        }

        // Reset game state
        GameManager gameManager = GameManager.Instance;
        if (gameManager != null)
        {
            gameManager.StartGame(); // Or create a new ResetGame() method in GameManager
        }

        Debug.Log("Game has been reset");
    }
}