using UnityEngine;
using System.Collections;

public class AIPlayer : Player
{
    [Header("AI Settings")]
    [SerializeField] private float moveDelay = 0.5f;
    private bool isProcessingMove = false;
    private int[,] simulationBoard = new int[3, 3]; // Invisible simulation

    public override void BeginTurn()
    {
        if (!isProcessingMove)
            StartCoroutine(ExecuteAITurn());
    }

    private IEnumerator ExecuteAITurn()
    {
        isProcessingMove = true;
        yield return new WaitForSeconds(moveDelay);

        // 1. Copy current board state to simulation
        CopyRealBoardToSimulation();

        // 2. Find best move (invisible simulation)
        int bestMove = FindBestMove(simulationBoard);

        // 3. Make ONLY ONE VISIBLE MOVE
        if (bestMove != -1)
            board.MarkCell(bestMove, playerIndex);

        GameManager.Instance.CheckGameState();
        isProcessingMove = false;
    }

    private void CopyRealBoardToSimulation()
    {
        for (int i = 0; i < 9; i++)
        {
            int row = i / 3;
            int col = i % 3;
            simulationBoard[row, col] = board.IsCellEmpty(i) ? 0 :
                                      (board.GetCellState(i) == 1 ? 1 : 2);
        }
    }

    private int FindBestMove(int[,] simulatedBoard)
    {
        int bestScore = int.MinValue;
        int bestMove = -1;

        for (int i = 0; i < 9; i++)
        {
            if (simulatedBoard[i / 3, i % 3] != 0) continue;

            // Simulate move in memory
            simulatedBoard[i / 3, i % 3] = 2; // AI is always '2' (O)
            int score = MiniMax(simulatedBoard, 0, false);
            simulatedBoard[i / 3, i % 3] = 0; // Revert simulation

            if (score > bestScore)
            {
                bestScore = score;
                bestMove = i;
            }
        }
        return bestMove;
    }

    private int MiniMax(int[,] boardState, int depth, bool isMaximizing)
    {
        int result = CheckSimulatedWinner(boardState);
        if (result != 0) return result == 2 ? 10 - depth : depth - 10;
        if (IsSimulatedBoardFull(boardState)) return 0;

        if (isMaximizing)
        {
            int bestScore = int.MinValue;
            for (int i = 0; i < 9; i++)
            {
                if (boardState[i / 3, i % 3] != 0) continue;

                boardState[i / 3, i % 3] = 2; // AI move
                bestScore = Mathf.Max(bestScore, MiniMax(boardState, depth + 1, false));
                boardState[i / 3, i % 3] = 0;
            }
            return bestScore;
        }
        else
        {
            int bestScore = int.MaxValue;
            for (int i = 0; i < 9; i++)
            {
                if (boardState[i / 3, i % 3] != 0) continue;

                boardState[i / 3, i % 3] = 1; // Human move
                bestScore = Mathf.Min(bestScore, MiniMax(boardState, depth + 1, true));
                boardState[i / 3, i % 3] = 0;
            }
            return bestScore;
        }
    }
    private int CheckSimulatedWinner(int[,] boardState)
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
        private bool IsSimulatedBoardFull(int[,] boardState)
    {
        for (int i = 0; i < 9; i++)
            if (boardState[i / 3, i % 3] == 0) return false;
        return true;
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



/*private void MakeMove()            //!Random move generation for testing purposes only!
{
    int[] emptyCells = board.GetEmptyCells();
    if (emptyCells.Length == 0) return;

    int randomIndex = Random.Range(0, emptyCells.Length);
    int move = emptyCells[randomIndex];

    board.MarkCell(move, playerIndex);
    CheckGameState();
    isMakingMove = false;
}*/