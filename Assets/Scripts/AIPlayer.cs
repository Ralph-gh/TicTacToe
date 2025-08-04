using UnityEngine;
using System.Collections;

public class AIPlayer : Player
{
    [Header("AI Settings")]
    [SerializeField] private float moveDelay = 0.5f;
    [SerializeField] private int maxDepth = 5;

    private bool isProcessingMove = false;
    private Coroutine activeCoroutine;

    public override void BeginTurn()
    {
        if (!isProcessingMove && GameManager.Instance.currentPlayer == this)
        {
            activeCoroutine = StartCoroutine(ExecuteAITurn());
        }
    }

    private IEnumerator ExecuteAITurn()
    {
        isProcessingMove = true;
        yield return new WaitForSeconds(moveDelay);

        // Only proceed if still our turn and game is active
        if (GameManager.Instance.currentPlayer != this || GameManager.Instance.gameEnded)
        {
            SafeAbort();
            yield break;
        }

        // Find best move using your MiniMax logic
        int bestMove = -1;
        int bestScore = int.MinValue;
        int[] validMoves = board.GetEmptyCells();

        foreach (int move in validMoves)
        {
            if (!board.IsCellEmpty(move)) continue;

            board.MarkCell(move, playerIndex);
            int score = 0;
            yield return StartCoroutine(
                EvaluateMove(board, 0, false, (result) => score = result)
            );
            board.UndoMark(move);

            if (score > bestScore || bestMove == -1)
            {
                bestScore = score;
                bestMove = move;
            }

            yield return null; // Prevent freezing
        }

        // Make ONE move if valid
        if (bestMove != -1 && board.IsCellEmpty(bestMove))
        {
            board.MarkCell(bestMove, playerIndex);
            GameManager.Instance.CheckGameState(); // Handles turn switching
        }

        isProcessingMove = false;
    }

    private IEnumerator EvaluateMove(Board board, int depth, bool isMaximizing, System.Action<int> callback)
    {
        if (depth > maxDepth || board == null)
        {
            callback(0);
            yield break;
        }

        int gameResult = board.CheckWinner();
        if (gameResult != 0)
        {
            callback(gameResult == playerIndex + 1 ? 10 - depth : depth - 10);
            yield break;
        }

        int[] moves = board.GetEmptyCells();
        if (moves.Length == 0)
        {
            callback(0);
            yield break;
        }

        int currentScore = isMaximizing ? int.MinValue : int.MaxValue;
        int processedMoves = 0;

        foreach (int move in moves)
        {
            board.MarkCell(move, isMaximizing ? playerIndex : (playerIndex + 1) % 2);
            int score = 0;
            yield return StartCoroutine(
                EvaluateMove(board, depth + 1, !isMaximizing, (result) => score = result)
            );
            board.UndoMark(move);

            currentScore = isMaximizing
                ? Mathf.Max(currentScore, score)
                : Mathf.Min(currentScore, score);

            if (++processedMoves % 2 == 0)
                yield return null;
        }

        callback(currentScore);
    }

    private void SafeAbort()
    {
        if (activeCoroutine != null)
            StopCoroutine(activeCoroutine);

        isProcessingMove = false;
    }

    void OnDisable()
    {
        SafeAbort();
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