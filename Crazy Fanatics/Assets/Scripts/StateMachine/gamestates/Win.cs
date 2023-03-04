using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Win : GameState
{
    private float closingTime= 4f;
    private float counter;
    TextMeshProUGUI mainText;
    Vector4 color = new Vector4(0, 0, 0, 1);
    public Win(GameManager g,StateMachine sm):base(sm,g)
	{
        GameObject go = GameObject.FindGameObjectWithTag("maintext");
        if(go!=null)
        {
            mainText = go.GetComponentInChildren<TextMeshProUGUI>();
        }
        
	}

	public override void Awake ()
	{
        //Debug.Log("idle awake");
        counter = closingTime;
        //if (RamCharacterAttributes.GetInstance().currentLevel >= 0)
        {
            RamCharacterAttributes.GetInstance().allys.Clear();
        }

        for (int i = 0; i < GameManager.Instance.allyList.Count; i++)
        {
            if (GameManager.Instance.allyList[i].id >= 0)
            {
                RamCharacterAttributes.GetInstance().allys.Add(GameManager.Instance.allyList[i].id);
            }

        }
        RamCharacterAttributes.GetInstance().consumibleItems = GameManager.Instance.MainCharacter.ConsumibleItems;

        //GameManager.Instance.fade.material.SetColor("_Color", Color.black);
    }

	public override void Sleep ()
	{

	}

	public override void Update ()
	{
        counter -= Time.deltaTime;
        if(counter<0)
        {
            counter = float.MaxValue;
            
            GameManager.Instance.DestroyAll();
            RamCharacterAttributes.GetInstance().currentLevel++;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            if(counter<4)
            {
                GameManager.Instance.fade.material.SetColor("_Color", color * (4 - counter)/4);
                mainText.text = ("Closing in "+Math.Floor(counter).ToString());
            }
            else
            {
                mainText.text = ("WIN");
            }
        }
	}

	
	
    //public override void ChangeState(Type type)
    //{
        
    //}
}
