using TMPro;
using UnityEngine;

public class GameResultUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] public GameObject resultPanel;
    [SerializeField] public TMP_Text resultText;

    [Header("Messages")]
    [SerializeField] private string playerWinText = "You Win!";
    [SerializeField] private string playerLoseText = "You Lose!";
    [SerializeField] private string drawText = "It's a Draw!";

    private void OnEnable()
    {
        // Verify setup when enabled
        if (resultPanel == null || resultText == null)
        {
            Debug.LogError("GameResultUI missing references!", this);
            return;
        }

        GameManager.OnGameEnded += ShowResult;
        GameManager.OnGameReset += HideResult;

        // Start with hidden panel
        resultPanel.SetActive(false);
    }

    private void OnDisable()
    {
        GameManager.OnGameEnded -= ShowResult;
        GameManager.OnGameReset -= HideResult;
    }

    private void ShowResult(Player winner)
    {
        if (resultPanel == null || resultText == null)
        {
            Debug.LogError("Missing UI references in GameResultUI", this);
            return;
        }

        Debug.Log($"Showing result for winner: {winner?.name ?? "draw"}");

        resultText.text = winner switch
        {
            HumanPlayer _ => playerWinText,
            AIPlayer _ => playerLoseText,
            null => drawText,
            _ => "Game Over"
        };

        resultPanel.SetActive(true);
    }

    public void HideResult()
    {
        if (resultPanel != null)
        {
            resultPanel.SetActive(false);
        }
    }
}