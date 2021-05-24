using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTT_AI : TTT_Player
{
    public int depth = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
            manager.SetCaseValue(playerId,GetBestMove());
    }
    public override void Init()
    {
        base.Init();
    }
    void DumbAi()
    {
        int randomCaseId = Random.Range(0, TTT_GameManager.availableCase.Count);
        Vector2Int caseToPlay = TTT_GameManager.availableCase[randomCaseId];
        manager.SetCaseValue(playerId, caseToPlay);
    }
    Vector2Int GetBestMove()
    {
        Vector2Int bestMove = new Vector2Int();
        float bestScore = -Mathf.Infinity;
        TTT_Round round = new TTT_Round(manager.board, playerId, TTT_GameManager.availableCase);
       
        foreach(Vector2Int v in round.availableCases)
        {
            round.setCase(v);
            TTT_Round aRound = new TTT_Round(round);
            float score = MinMax(aRound, depth, -Mathf.Infinity, Mathf.Infinity, false);
            round.UndoBoard();
            Debug.Log(score);
            if (score > bestScore)
            {
                bestMove = v;
                bestScore = score;
            }

        }
        return bestMove;

                
    }
    float MinMax(TTT_Round aRound,int depth,float alpha,float beta,bool thisIsPlaying)
    {
        if (depth == 0 || aRound.IsGameOver()>=0)
        {
            if (aRound.IsGameOver() > 0)
            {
                if (thisIsPlaying) return -1 - depth;
                else return 1 + depth;
            }
            else return 0;
        }
        else
        {
            if (thisIsPlaying)
            {
                float bestScore = -Mathf.Infinity;
                foreach(Vector2Int move in aRound.availableCases)
                {
                    aRound.setCase(move);
                    TTT_Round nRound = new TTT_Round(aRound);
                    bestScore = Mathf.Max(bestScore, MinMax(nRound, depth - 1, alpha, beta, !thisIsPlaying));
                    aRound.UndoBoard();
                    alpha = Mathf.Max(alpha, bestScore);
                    if (alpha >= beta)
                    {
                        break;
                        
                    }
                }
                return bestScore;
            }
            else
            {
                float bestScore = Mathf.Infinity;
                foreach (Vector2Int move in aRound.availableCases)
                {
                    aRound.setCase(move);
                    TTT_Round nRound = new TTT_Round(aRound);
                    bestScore = Mathf.Min(bestScore, MinMax(nRound, depth - 1, alpha, beta, !thisIsPlaying));
                    aRound.UndoBoard();
                    beta = Mathf.Min(bestScore, beta);
                    if (alpha >= beta)
                    {
                        break;

                    }
                }
                return bestScore;
            }

        }
        return 0;
    }

}
