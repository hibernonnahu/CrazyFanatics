using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinningManager : MonoBehaviour {
    [SerializeField]
    List<Vector2> enemysToKill = new List<Vector2>();
    public static WinningManager Instance;
    // Use this for initialization
    void Start () {
        Instance = this;
	}

    public void AddEnemyToKill(List<Vector2> list)
    {
        enemysToKill = list;
    }
    public void CheckEnemyKill(int id)
    {
        if(enemysToKill.Count!=0&&id!=-1)
       
        {
            for (int i = enemysToKill.Count - 1; i >= 0; i--)
            {
                if (id==enemysToKill[i].x)
                {
                    enemysToKill[i] += Vector2.down;
                    if (enemysToKill[i].y==0)
                    {
                        enemysToKill.RemoveAt(i);
                        if (enemysToKill.Count == 0)
                        {
                            GameManager.Instance.OnWin();
                        }
                        break;
                    }
                    
                }
               

            }
        }
        
        
    }

}
