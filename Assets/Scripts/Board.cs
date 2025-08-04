using UnityEngine;

public class Board : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private GameObject[] marks; // Index 0 = X, 1 = O

    [Header("Runtime References")]
    [SerializeField] private Cell[] cells = new Cell[9];

    private int[,] boardState = new int[3, 3]; // 0 = empty, 1 = X, 2 = O

    private void Start()
    {
        InitializeBoard();
    }
    
    public int GetCellState(int index) => boardState[index / 3, index % 3];
    

    private void InitializeBoard()
    {
        if (cellPrefab == null || marks == null || marks.Length != 2)
        {
            Debug.LogError("Missing required prefabs!", this);
            return;
        }

        for (int i = 0; i < 9; i++)
        {
            GameObject cellObj = Instantiate(cellPrefab, transform);
            cellObj.name = $"Cell_{i}";

            Cell cell = cellObj.GetComponent<Cell>();
            if (cell == null)
            {
                Debug.LogError("Cell prefab missing Cell script!", cellObj);
                continue;
            }

            cell.Setup(this, i); // Using Setup instead of Initialize
            cells[i] = cell;
        }
    }

    public void MarkCell(int index, int playerIndex)
    {
        if (!IsValidIndex(index) || !IsValidPlayer(playerIndex) || !IsCellEmpty(index))
            return;
        Debug.Log($"Cell {index} marked for player {playerIndex}");
        // Update board state (playerIndex 0 becomes 1 (X), 1 becomes 2 (O))
        int row = index / 3;
        int col = index % 3;
        boardState[row, col] = playerIndex + 1;

        // Visual representation
        if (cells[index] != null)
        {
            cells[index].SetInteractable(false);
            InstantiateMark(cells[index].transform, playerIndex);
        }
    }

    private void InstantiateMark(Transform parent, int playerIndex)
    {
        if (marks[playerIndex] == null)
        {
            Debug.LogError($"Mark prefab for player {playerIndex} is null!");
            return;
        }

        GameObject mark = Instantiate(marks[playerIndex], parent);
        mark.transform.localPosition = Vector3.zero;
        mark.transform.localRotation = Quaternion.identity;
        mark.SetActive(true);
    }

    public void UndoMark(int index)
    {
        if (!IsValidIndex(index)) return;

        int row = index / 3;
        int col = index % 3;
        boardState[row, col] = 0;

        if (cells[index] != null)
        {
            cells[index].ClearMark();
            cells[index].SetInteractable(true);
        }
    }

    public bool IsCellEmpty(int index)
    {
        if (!IsValidIndex(index)) return false;

        int row = index / 3;
        int col = index % 3;
        return boardState[row, col] == 0;
    }

    public int CheckWinner()
    {
        // Check rows and columns
        for (int i = 0; i < 3; i++)
        {
            // Rows
            if (boardState[i, 0] != 0 &&
                boardState[i, 0] == boardState[i, 1] &&
                boardState[i, 1] == boardState[i, 2])
                return boardState[i, 0];

            // Columns
            if (boardState[0, i] != 0 &&
                boardState[0, i] == boardState[1, i] &&
                boardState[1, i] == boardState[2, i])
                return boardState[0, i];
        }

        // Diagonals
        if (boardState[0, 0] != 0 &&
            boardState[0, 0] == boardState[1, 1] &&
            boardState[1, 1] == boardState[2, 2])
            return boardState[0, 0];

        if (boardState[0, 2] != 0 &&
            boardState[0, 2] == boardState[1, 1] &&
            boardState[1, 1] == boardState[2, 0])
            return boardState[0, 2];

        return 0; // No winner yet
    }

    public int[] GetEmptyCells()
    {
        System.Collections.Generic.List<int> emptyCells = new System.Collections.Generic.List<int>();
        for (int i = 0; i < 9; i++)
        {
            if (IsCellEmpty(i))
                emptyCells.Add(i);
        }
        return emptyCells.ToArray();
    }

    public bool IsBoardFull()
    {
        for (int i = 0; i < 9; i++)
        {
            if (IsCellEmpty(i))
                return false;
        }
        return true;
    }

    public void ResetBoard()
    {
        boardState = new int[3, 3];
        foreach (Cell cell in cells)
        {
            if (cell != null)
            {
                cell.ClearMark();
                cell.SetInteractable(true);
            }
        }
    }

    private bool IsValidIndex(int index) => index >= 0 && index < 9;
    private bool IsValidPlayer(int playerIndex) => playerIndex >= 0 && playerIndex < 2;
}