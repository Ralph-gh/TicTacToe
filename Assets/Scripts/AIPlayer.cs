using UnityEngine;
using System.Collections;

public class AIPlayer : Player
{
    [SerializeField] private float moveDelay = 0.5f;
    private bool isMakingMove = false;
    private const int maxDepth = 9; // Safety limit

    public override void BeginTurn()
    {
        if (isMakingMove) return;

        Debug.Log("AI Player turn started");
        StartCoroutine(ExecuteMove());
    }

    private IEnumerator ExecuteMove()
    {
        isMakingMove = true;
        yield return new WaitForSeconds(moveDelay); // Optional delay

        if (board == null)
        {
            Debug.LogError("Board reference missing");
            isMakingMove = false;
            yield break;
        }

        int bestMove = FindBestMove();
        if (bestMove == -1)
        {
            Debug.LogError("No valid moves found");
            isMakingMove = false;
            yield break;
        }

        board.MarkCell(bestMove, playerIndex);
        isMakingMove = false;
        CheckGameState();
    }

    private int FindBestMove()
    {
        int[] emptyCells = board.GetEmptyCells();
        if (emptyCells.Length == 0) return -1;

        int bestMove = emptyCells[0];
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
        // Base case: depth limit
        if (depth > maxDepth) return 0;

        int result = currentBoard.CheckWinner();
        if (result != 0)
        {
            if (result == playerIndex + 1) return 10 - depth;
            if (result == ((playerIndex + 1) % 2) + 1) return depth - 10;
            return 0; // Draw
        }

        int[] emptyCells = currentBoard.GetEmptyCells();
        if (emptyCells.Length == 0) return 0; // Draw

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
                int opponentIndex = (playerIndex + 1) % 2;
                currentBoard.MarkCell(cell, opponentIndex);
                int score = MiniMax(currentBoard, depth + 1, true);
                currentBoard.UndoMark(cell);
                bestScore = Mathf.Min(score, bestScore);
            }
            return bestScore;
        }
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