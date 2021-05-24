using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTT_Round 
{
   public int[,] board,pBoard;
    public int boardSize;
    public int currentPlayer;
    public int playerId, opponentId;
    public List<Vector2Int> availableCases = new List<Vector2Int>();
    Vector2Int lastMove = new Vector2Int();
    public TTT_Round(int[,] aBoard,int cPlayer,List<Vector2Int> availCases)
    {
        boardSize = aBoard.GetLength(0);
        currentPlayer = cPlayer;
        playerId = currentPlayer;
        opponentId = (playerId + 1) % 2;
        board = new int[boardSize, boardSize];
        pBoard = new int[boardSize, boardSize];
        System.Array.Copy(aBoard, board, board.Length);
        SaveBoard();
        availableCases = new List<Vector2Int>(availCases);
    }
    public TTT_Round(TTT_Round aRound)
    {
        boardSize = aRound.boardSize;
        playerId = aRound.opponentId;
        opponentId = aRound.playerId;
        board = new int[boardSize, boardSize];
        pBoard = new int[boardSize, boardSize];
        System.Array.Copy(aRound.board, board, aRound.board.Length);
        SaveBoard();
        availableCases = new List<Vector2Int>(aRound.availableCases);
    }
   public void setCase(Vector2Int posCase)
    {
        SaveBoard();
        lastMove = posCase;
        board[posCase.x, posCase.y] = currentPlayer + 1;
    }
    public void SaveBoard()
    {
        System.Array.Copy(board, pBoard, board.Length);
    }
    public void UndoBoard()
    {
        System.Array.Copy(pBoard, board, pBoard.Length);
        availableCases.Add(lastMove);
    }
   public List<TTT_Round> GenerateNextRounds()
    {
        List<TTT_Round> rounds=new List<TTT_Round>();
        for(int i = 0; i < availableCases.Count; i++)
        {
            setCase(availableCases[i]);
            TTT_Round aRound = new TTT_Round(this);
            rounds.Add(aRound);
            UndoBoard();
        }
        return rounds;
    }
    public int IsGameOver()
    {
        return TTT_GameManager.instance.CheckGameOver(board);
    }

}
