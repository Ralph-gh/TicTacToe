using UnityEngine;

public class AIPlayer : Player
{
    [SerializeField] private float moveDelay = 0.5f;

    public override void BeginTurn()
    {
        Invoke("MakeMove", moveDelay);
    }

    private void MakeMove()
    {
        int bestMove = FindBestMove();
        board.MarkCell(bestMove, playerIndex);
        CheckGameState();
    }

    private int FindBestMove()
    {
        int[] emptyCells = board.GetEmptyCells();
        int bestMove = -1;
        int bestScore = int.MinValue;

        foreach (int cell in emptyCells)
        {
            board.MarkCell(cell, playerIndex);
            int score = MiniMax(board, 0, false);
            board.UndoMark(cell); // You'll need to implement UndoMark in Board.cs

            if (score > bestScore)
            {
                bestScore = score;
                bestMove = cell;
            }
        }

        return bestMove;
    }

    private int MiniMax(Board currentBoard, int depth, bool isMaximizing)
    {
        int result = currentBoard.CheckWinner();
        if (result != 0)
        {
            if (result == playerIndex + 1) return 10 - depth;
            else if (result == ((playerIndex + 1) % 2) + 1) return depth - 10;
            else return 0;
        }

        int[] emptyCells = currentBoard.GetEmptyCells();
        if (emptyCells.Length == 0) return 0;

        if (isMaximizing)
        {
            int bestScore = int.MinValue;
            foreach (int cell in emptyCells)
            {
                currentBoard.MarkCell(cell, playerIndex);
                int score = MiniMax(currentBoard, depth + 1, false);
                currentBoard.UndoMark(cell);
                bestScore = Mathf.Max(score, bestScore);
            }
            return bestScore;
        }
        else
        {
            int bestScore = int.MaxValue;
            foreach (int cell in emptyCells)
            {
                currentBoard.MarkCell(cell, (playerIndex + 1) % 2);
                int score = MiniMax(currentBoard, depth + 1, true);
                currentBoard.UndoMark(cell);
                bestScore = Mathf.Min(score, bestScore);
            }
            return bestScore;
        }
    }
}