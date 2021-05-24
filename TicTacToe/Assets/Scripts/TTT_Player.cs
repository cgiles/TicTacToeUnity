using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTT_Player : MonoBehaviour
{
    public bool isPlayer0;
    
    public int playerId;
    public bool isPlaying;
    [HideInInspector]
    public TTT_GameManager manager;

    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Input.GetMouseButtonDown(0))
            {
                if(Physics.Raycast(ray,out hit))
                {
                    if (hit.collider.GetComponent<TTT_CaseCon>())
                    {
                        if (hit.collider.GetComponent<TTT_CaseCon>().value == 0)
                        {
                            manager.SetCaseValue(playerId, hit.collider.GetComponent<TTT_CaseCon>().position);
                        }
                    }
                }
            }
        }
    }
    public virtual void Init()
    {
         Init(isPlayer0);
    }
    public virtual void Init(bool isP0)
    {
        isPlayer0 = isP0;
        playerId = isPlayer0 ? 1 : 2;
        cam = Camera.main;
        manager = TTT_GameManager.instance;
    }
}
