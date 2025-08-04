using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Cell : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Board board; // Now serialized for manual assignment
    public int index;

    private Button button;

    private void Awake()
    {
        // Get required components
        button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError("Cell is missing Button component!", this);
            return;
        }

        // Try to find board if not assigned
        if (board == null)
        {
            board = GetComponentInParent<Board>();
            if (board == null)
            {
                Debug.LogError("Board reference not found in parent!", this);
                return;
            }
        }

        // Safe initialization
        button.onClick.RemoveAllListeners(); // Clear existing listeners
        button.onClick.AddListener(OnCellClicked);
    }

    private void OnCellClicked()
    {

        /// Early exit checks
        if (GameManager.Instance == null || board == null)
        {
            Debug.LogError("Missing references in Cell click");
            return;
        }

        // Only allow human player to click during their turn
        if (!(GameManager.Instance.players[GameManager.Instance.currentPlayerIndex] is HumanPlayer))
        {
            Debug.Log("Not human player's turn");
            return;
        }

        if (board.IsCellEmpty(index))
        {
            board.MarkCell(index, GameManager.Instance.currentPlayerIndex);
            GameManager.Instance.SwitchPlayer();
        }
    }

    public void SetInteractable(bool state)
    {
        if (button != null) button.interactable = state;
    }

    public void ClearMark()
    {
        foreach (Transform child in transform)
        {
            if (child != transform) // Don't destroy self
                Destroy(child.gameObject);
        }
    }
}