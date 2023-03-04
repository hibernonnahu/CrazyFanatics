using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : State {
    protected GameManager game;
	public GameState(StateMachine sm,GameManager g):base(sm)
    {
        game = g;
    }
}
