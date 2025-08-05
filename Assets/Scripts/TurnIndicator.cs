using TMPro;
using UnityEngine;

public class TurnIndicator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI turnText;

    void Update()
    {
        if (GameManager.Instance == null) return;

        if (GameManager.Instance.currentPlayer is HumanPlayer)
            turnText.text = "Your turn (X)";
        else
            turnText.text = "AI's turn (O)";
    }
}