using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    [SerializeField] public Player[] players;
    [HideInInspector] public Board board;
    public Player currentPlayer { get; private set; } // Added proper currentPlayer reference
    public bool gameEnded { get; private set; } // Better naming than isGameActive

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            board = FindObjectOfType<Board>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializePlayers();
        StartGame();
    }

    private void InitializePlayers()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] == null) continue;
            players[i].playerIndex = i;
            players[i].board = board ?? FindObjectOfType<Board>();
        }
    }

    public void StartGame()
    {
        gameEnded = false;
        currentPlayer = players[0];
        currentPlayer?.BeginTurn();
    }

    public void SwitchPlayer()
    {
        currentPlayer?.EndTurn();
        currentPlayer = (currentPlayer == players[0]) ? players[1] : players[0];
        currentPlayer?.BeginTurn(); //  Must trigger AI's turn
    }

    private Player GetNextPlayer()
    {
        int nextIndex = (System.Array.IndexOf(players, currentPlayer) + 1);
        if (nextIndex >= players.Length) nextIndex = 0;
        return players[nextIndex];
    }

    public void CheckGameState()
    {
        if (gameEnded) return;

        int result = board.CheckWinner();

        if (result != 0) // We have a winner
        {
            Player winner = result == 1 ? players[0] : players[1];
            EndGame(winner);
        }
        else if (board.IsBoardFull()) // Draw
        {
            EndGame(null);
        }
        else // Continue game
        {
            SwitchPlayer();
        }
    }

    public void EndGame(Player winner)
    {
        gameEnded = true;
        foreach (Player player in players)
        {
            player?.EndTurn();
        }
        Debug.Log(winner != null ? $"{winner.name} wins!" : "It's a draw!");
    }
}