using UnityEngine;
using System.Collections;
using System;

public class Play : GameState
{

    
	public Play(GameManager g,StateMachine sm):base(sm,g)
	{
        
	}

	public override void Awake ()
	{
        //Debug.Log("idle awake");
        
	}

	public override void Sleep ()
	{

	}

	public override void Update ()
	{
        game.timeMethod();
	}

	
	
    //public override void ChangeState(Type type)
    //{
        
    //}
}
