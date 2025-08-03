using UnityEngine;

public class Cell : MonoBehaviour
{
    private Board board;
    private int index;

    private void Awake()
    {
        board = GetComponentInParent<Board>();
        index = transform.GetSiblingIndex();
    }

    private void OnMouseDown()
    {
        if (board.IsCellEmpty(index))
        {
            board.MarkCell(index, 0); // 0 for human player
            GameManager.Instance.SwitchPlayer();
        }
    }

    public void SetInteractable(bool state)
    {
        // You could change appearance here if needed
    }

    public void ClearMark()
    {
        // Destroy any child objects (X or O marks)
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}