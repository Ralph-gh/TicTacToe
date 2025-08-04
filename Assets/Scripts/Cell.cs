using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [HideInInspector] public Board board;
    [HideInInspector] public int index;
    public bool IsEmpty { get; private set; } = true;

    private Button button;
    private Transform markContainer;

    private void Awake()
    {
        button = GetComponent<Button>();
        markContainer = transform.Find("MarkContainer") ?? transform;
    }

    // Replacement for Initialize method
    public void Setup(Board linkedBoard, int cellIndex)
    {
        board = linkedBoard;
        index = cellIndex;
        button.onClick.AddListener(OnCellClicked);
    }

    private void OnCellClicked()
    {
        if (IsEmpty && board != null)
        {
            // Get reference to HumanPlayer and call MakeMove properly
            HumanPlayer humanPlayer = GameManager.Instance.players[0] as HumanPlayer;
            if (humanPlayer != null)
            {
                humanPlayer.MakeMove(index);
            }
        }
    }

    public void Mark(int playerIndex)
    {
        IsEmpty = false;
        // Visual marking handled by Board
    }

    public void ClearMark()
    {
        IsEmpty = true;
        foreach (Transform child in markContainer)
        {
            Destroy(child.gameObject);
        }
    }

    public void SetInteractable(bool interactable)
    {
        if (button != null)
        {
            button.interactable = interactable;
        }
    }
}