using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Board board;
    [SerializeField] public Player[] players;
    private int currentPlayerIndex;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        currentPlayerIndex = 0;
        players[currentPlayerIndex].BeginTurn();
    }

    public void SwitchPlayer()
    {
        players[currentPlayerIndex].EndTurn();
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;
        players[currentPlayerIndex].BeginTurn();
    }

    public void EndGame(Player winner)
    {
        foreach (Player player in players)
        {
            player.EndTurn();
        }

        Debug.Log(winner != null ? $"{winner.name} wins!" : "It's a draw!");
    }
}