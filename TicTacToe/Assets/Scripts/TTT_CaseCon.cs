using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTT_CaseCon : MonoBehaviour
{
    public GameObject Cross, Circle;
    public Vector2Int position;
    Transform child;
    public int value = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
  public  void SetValue(int val)
    {
        value = val;
        if (value == 1)
        {
            GameObject playerObject = Instantiate(Cross, transform);
            child = playerObject.transform;

        }else if (value == 2)
        {
            GameObject playerObject = Instantiate(Circle, transform);
            child = playerObject.transform;
        }
    }
    public void Reset()
    {
        value = 0;
        if (child!=null) Destroy(child.gameObject);
    }
}
