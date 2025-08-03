using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private Cell[] cells;
    [SerializeField] private GameObject[] marks; // X and O prefabs
    private int[,] boardState = new int[3, 3];

    public void MarkCell(int index, int playerIndex)
    {
        int row = index / 3;
        int col = index % 3;
        boardState[row, col] = playerIndex + 1;

        Instantiate(marks[playerIndex], cells[index].transform);
        cells[index].SetInteractable(false);
    }

    public void UndoMark(int index)
    {
        int row = index / 3;
        int col = index % 3;
        boardState[row, col] = 0;
        cells[index].ClearMark();
        cells[index].SetInteractable(true);
    }

    public bool IsCellEmpty(int index)
    {
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
            cell.SetInteractable(true);
            cell.ClearMark();
        }
    }
}