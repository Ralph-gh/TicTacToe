using UnityEngine;

public class AIPlayer : Player
{
    [SerializeField] private float moveDelay = 0.5f;
    private bool isMakingMove = false; // Add this flag
    public override void BeginTurn()
    {
        if (isMakingMove) return; // Prevent multiple calls

        Debug.Log("AI Player turn started");
        isMakingMove = true;
        Invoke("MakeMove", moveDelay);
    }

   /* private void MakeMove()
    {
        Debug.Log("AI making move");
        if (board == null)
        {
            Debug.LogError("Board reference missing in AI player");
            isMakingMove = false;
            return;
        }
        int bestMove = FindBestMove();
        if (bestMove == -1)
        {
            Debug.LogError("AI couldn't find a valid move");
            isMakingMove = false;
            return;
        }
        board.MarkCell(bestMove, playerIndex);
        isMakingMove = false; // Reset flag
        CheckGameState();
    }*/



    private void MakeMove()
    {
        int[] emptyCells = board.GetEmptyCells();
        if (emptyCells.Length == 0) return;

        int randomIndex = Random.Range(0, emptyCells.Length);
        int move = emptyCells[randomIndex];

        board.MarkCell(move, playerIndex);
        CheckGameState();
        isMakingMove = false;
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

            board.UndoMark(cell);

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
        int winner = currentBoard.CheckWinner();

        // Terminal state
        if (winner != 0)
        {
            if (winner == playerIndex + 1) return 10 - depth; // AI win
            else if (winner == ((playerIndex + 1) % 2) + 1) return depth - 10; // Opponent win
            else return 0; // Draw
        }

        // Depth limit (e.g. 6)
        if (depth >= 6)
            return 0;

        int[] emptyCells = currentBoard.GetEmptyCells();

        if (isMaximizing)
        {
            int bestScore = int.MinValue;

            foreach (int cell in emptyCells)
            {
                currentBoard.MarkCell(cell, playerIndex);
                int score = MiniMax(currentBoard, depth + 1, false);
                currentBoard.UndoMark(cell);
                bestScore = Mathf.Max(bestScore, score);
                Debug.Log($"AI evaluates cell {cell} with score {score}");
            }

            return bestScore;
        }
        else
        {
            int bestScore = int.MaxValue;

            int opponentIndex = (playerIndex + 1) % 2;

            foreach (int cell in emptyCells)
            {
                currentBoard.MarkCell(cell, opponentIndex);
                int score = MiniMax(currentBoard, depth + 1, true);
                currentBoard.UndoMark(cell);
                bestScore = Mathf.Min(bestScore, score);
                Debug.Log($"AI evaluates cell {cell} with score {score}");
            }

            return bestScore;
        }
    }


    /*private int MiniMax(Board currentBoard, int depth, bool isMaximizing)
    {
        if (depth > 9) return 0; // Safety check

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
    }*/
}