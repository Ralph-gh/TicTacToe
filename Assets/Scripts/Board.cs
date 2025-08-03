using UnityEngine;

public class Board : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private GameObject[] marks;

    [Header("Runtime References")]
    [SerializeField] private Cell[] cells = new Cell[9];

    private int[,] boardState = new int[3, 3]; // Added this critical line

    private void Start()
    {
        InitializeBoard();
    }

    private void InitializeBoard()
    {
        // Validate prefab
        if (cellPrefab == null)
        {
            Debug.LogError("Cell prefab not assigned!", this);
            return;
        }

        // Validate marks
        if (marks == null || marks.Length != 2)
        {
            Debug.LogError("Marks array not properly configured!", this);
            return;
        }

        // Create grid
        for (int i = 0; i < 9; i++)
        {
            GameObject cellObj = Instantiate(cellPrefab, transform);
            cellObj.name = $"Cell_{i}";

            // Get and configure cell
            Cell cell = cellObj.GetComponent<Cell>();
            if (cell == null)
            {
                Debug.LogError("Instantiated cell missing Cell script!", cellObj);
                continue;
            }

            // Manually assign board reference
            cell.board = this;
            cell.index = i;
            cells[i] = cell;
        }
    }

    public void MarkCell(int index, int playerIndex)
    {
        if (index < 0 || index >= 9)
        {
            Debug.LogError($"Invalid cell index: {index}");
            return;
        }

        int row = index / 3;
        int col = index % 3;
        boardState[row, col] = playerIndex + 1;

        // Safety checks
        if (cells[index] == null)
        {
            Debug.LogError($"Cell at index {index} is null!");
            return;
        }

        if (playerIndex < 0 || playerIndex >= marks.Length)
        {
            Debug.LogError($"Invalid player index: {playerIndex}");
            return;
        }

        if (marks[playerIndex] == null)
        {
            Debug.LogError($"Mark prefab for player {playerIndex} is null!");
            return;
        }

        // Instantiate the mark
        GameObject mark = Instantiate(marks[playerIndex], cells[index].transform);
        mark.SetActive(true);
        cells[index].SetInteractable(false);
    }

    public void UndoMark(int index)
    {
        if (index < 0 || index >= 9)
        {
            Debug.LogError($"Invalid cell index: {index}");
            return;
        }

        int row = index / 3;
        int col = index % 3;
        boardState[row, col] = 0;

        if (cells[index] == null)
        {
            Debug.LogError($"Cell at index {index} is null!");
            return;
        }

        cells[index].ClearMark();
        cells[index].SetInteractable(true);
    }

    public bool IsCellEmpty(int index)
    {
        if (index < 0 || index >= 9)
        {
            Debug.LogError($"Invalid cell index: {index}");
            return false;
        }

        int row = index / 3;
        int col = index % 3;
        return boardState[row, col] == 0;
    }

    public int CheckWinner()
    {
        // Check rows, columns, and diagonals
        for (int i = 0; i < 3; i++)
        {
            // Rows
            if (boardState[i, 0] != 0 && boardState[i, 0] == boardState[i, 1] && boardState[i, 1] == boardState[i, 2])
                return boardState[i, 0];

            // Columns
            if (boardState[0, i] != 0 && boardState[0, i] == boardState[1, i] && boardState[1, i] == boardState[2, i])
                return boardState[0, i];
        }

        // Diagonals
        if (boardState[0, 0] != 0 && boardState[0, 0] == boardState[1, 1] && boardState[1, 1] == boardState[2, 2])
            return boardState[0, 0];

        if (boardState[0, 2] != 0 && boardState[0, 2] == boardState[1, 1] && boardState[1, 1] == boardState[2, 0])
            return boardState[0, 2];

        // Check for draw
        foreach (int cell in boardState)
        {
            if (cell == 0) return 0; // Game still ongoing
        }

        return -1; // Draw
    }

    public int[] GetEmptyCells()
    {
        System.Collections.Generic.List<int> emptyCells = new System.Collections.Generic.List<int>();
        for (int i = 0; i < 9; i++)
        {
            if (IsCellEmpty(i)) emptyCells.Add(i);
        }
        return emptyCells.ToArray();
    }

    public void ResetBoard()
    {
        boardState = new int[3, 3];
        foreach (Cell cell in cells)
        {
            if (cell != null)
            {
                cell.SetInteractable(true);
                cell.ClearMark();
            }
        }
    }
}