using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] public Player[] players;
    [HideInInspector] public Board board;
    public int currentPlayerIndex;

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
        for (int i = 0; i < players.Length; i++)
        {
            players[i].playerIndex = i;
            players[i].board = board;
        }
        currentPlayerIndex = 0;
        players[currentPlayerIndex].BeginTurn();
    }

    public void SwitchPlayer()
    {
        players[currentPlayerIndex].EndTurn();
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;
        Debug.Log($"Switching to player {currentPlayerIndex}");
        players[currentPlayerIndex].BeginTurn();
    }

    public void EndGame(Player winner)
    {
        foreach (Player player in players)
        {
            player.EndTurn();
        }

        Debug.Log(winner != null ? $"{winner.name} wins!" : "It's a draw!");
        // Add restart logic here if needed
    }
}