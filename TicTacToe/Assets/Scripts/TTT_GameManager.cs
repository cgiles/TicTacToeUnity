using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTT_GameManager : MonoBehaviour
{
    public TTT_CaseCon tttCase;
    public GameObject gridElement;
    [HideInInspector]
    public int[,] board = new int[3, 3];
    public TTT_CaseCon[,] cases = new TTT_CaseCon[3, 3];
    static public TTT_GameManager instance;
    [Range(3, 10)]
    public int boardSize=3;
    float startBoardOffset;
    public TTT_Player[] players=new TTT_Player[2];
    public int currentPlayer=0;
  public static   List<Vector2Int> availableCase = new List<Vector2Int>();
    // Start is called before the first frame update
    public GameMode gameMode;
   public enum GameMode
    {
        PVsP,
        PVsAi,
        AisVP,
        AiVsAi

    }
    void Start()
    {
        Camera.main.transform.position = -Vector3.forward * boardSize;
        instance = this;
        startBoardOffset = boardSize / 2f;
        Debug.Log(startBoardOffset);
        GenerateBoard();
        InitBoards();
        InitPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GenerateBoard()
    {
        
        board = new int[boardSize,boardSize];
        cases = new TTT_CaseCon[boardSize, boardSize];
        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                Instantiate(gridElement, new Vector3(x - startBoardOffset+0.5f, y -startBoardOffset+0.5f, 0), Quaternion.identity, transform);
                cases[x, y] = Instantiate(tttCase, new Vector3(x - startBoardOffset+0.5f, y - startBoardOffset+0.5f, 0), Quaternion.identity, transform);
                cases[x, y].position = new Vector2Int(x, y);
            }
        }
    }
    public void InitBoards()
    {
        availableCase.Clear();
        for(int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                board[x, y] = 0;
                cases[x, y].Reset();
                availableCase.Add(new Vector2Int(x, y));
            }
        }   
    }
     public void SetCaseValue(int playerId,Vector2Int caseId)
    {
        board[caseId.x, caseId.y] = playerId;
        cases[caseId.x, caseId.y].SetValue(playerId);
        availableCase.Remove(caseId);
        Debug.Log(availableCase.Count);
        SwitchPlayer();
        if (CheckGameOver() >= 0)
        {
            if(CheckGameOver()>0)
            {
                Debug.Log("Winner is P" + CheckGameOver().ToString());
                GameOver();
            }
            else
            {
                Debug.Log("Game is a Tie");
                GameOver();
            }
        }
    }
    public void InitPlayer()
    {
        switch (gameMode)
        {
            case GameMode.PVsP :
                players[0] = gameObject.AddComponent<TTT_Player>();
                players[1] = gameObject.AddComponent<TTT_Player>();
                break;
            case GameMode.PVsAi:
                players[0] = gameObject.AddComponent<TTT_Player>();
                players[1] = gameObject.AddComponent<TTT_AI>();
                break;
            case GameMode.AisVP:
                players[0] = gameObject.AddComponent<TTT_AI>();
                players[1] = gameObject.AddComponent<TTT_Player>();
                break;
            case GameMode.AiVsAi:
                players[0] = gameObject.AddComponent<TTT_AI>();
                players[1] = gameObject.AddComponent<TTT_AI>();
                break;
             default:
                gameMode = GameMode.PVsP;
                InitPlayer();
                return;
                
        }
        
        players[0].isPlayer0 = true;
        players[0].isPlaying=true;
        foreach (TTT_Player p in players) p.Init();
        currentPlayer = 0;
    }

    public void SwitchPlayer()
    {
        players[currentPlayer].isPlaying = false;
        currentPlayer = (currentPlayer + 1) % 2;
        players[currentPlayer].isPlaying = true;
    }
    public int CheckGameOver()
    {
        return CheckGameOver(board);
    }
    public int CheckGameOver(int[,] aBoard)
    {
       
        int pV = 0;
        bool isEqual = false;
        for (int x = 0; x < boardSize; x++)
        {
            pV = aBoard[x, 0];
           
            
            for(int y = 0; y < boardSize; y++)
            {
                if (board[x, y] == pV && pV != 0)
                {
                    isEqual = true;
                }else
                {
                    isEqual = false;
                    break;
                }
            }
            if (isEqual)
            {
               
                return pV;
            }
        }
        for(int y = 0; y < boardSize; y++)
        {
            pV = board[0, y];
            for(int x = 0; x < boardSize; x++)
            {
                if (board[x, y] == pV&&pV!=0)
                {
                    isEqual = true;
                }
                else
                {
                    isEqual = false;
                    break;
                }
            }
            if (isEqual)
            {
                return pV;
            }
        }
        pV = aBoard[0, 0];
        
        for (int x = 0, y = 0; x < boardSize; x++, y++)
        {
            

            if (aBoard[x, y] == pV && pV != 0)
            {
                isEqual = true;

            }else
            {
                isEqual = false;
                break;
            }
        }
        if (isEqual)
        {
            
            return pV;

        }
        pV = aBoard[0, boardSize-1];
        isEqual = false;
        for (int x = 0, y = boardSize-1; x < boardSize; x++, y--)
        {


            if (aBoard[x, y] == pV && pV != 0)
            {
                isEqual = true;

            }
            else
            {
                isEqual = false;
                break;
            }
        }
        if (isEqual)
        {
            
            return pV;

        }
        bool isFull = true;
        foreach (int i in aBoard)
        {
            if (i == 0)
            {
                isFull = false;
                break;
            }
            
        }
        if (isFull)
        {

           
            return 0;
        }
       
        
        return -1;
    }
    public void GameOver()
    {
        players[currentPlayer].isPlaying = false;
        StartCoroutine(_GameOver());
    }
    IEnumerator _GameOver()
    {
        yield return new WaitForSeconds(5f);
        InitBoards();
        InitPlayer();
    }
}
