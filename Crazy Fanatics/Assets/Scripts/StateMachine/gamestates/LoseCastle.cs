using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class LoseCastle : Lose
{
    
	public LoseCastle(GameManager g,StateMachine sm):base(g,sm)
	{
       
	}

	public override void Awake ()
	{
        base.Awake();
        //Debug.Log("idle awake");
        loseBy = GameManager.Instance.Castle;
        loseBy.GetComponent<Castle>().StopUpdate();
        GameManager.Instance.MainCharacter.ChangeState(typeof(DeadCastle));
       
	}

	
	

	
	
    //public override void ChangeState(Type type)
    //{
        
    //}
}
