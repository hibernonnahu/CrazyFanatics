using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
[System.Serializable]
public class StateMachine
{
		public List<State> stateList;
		public State currentState;
    private string currentStateName = "";
		public Type lastState;
        public Type nextState;
        public StateMachine ()
		{
				stateList = new List<State> ();
		}

		public void addState (State state)
		{
				if (state != null) {
						stateList.Add (state);
						if (stateList.Count == 1) {
								currentState = state;
								state.Awake ();
						}
				}
		}

		public void changeState (Type type)
		{
				if (type != currentState.GetType ()) {

						int l = stateList.Count;
						for (int i=0; i<l; i++) {
								if (stateList [i].GetType () == type) {

										currentState.Sleep ();
										lastState = currentState.GetType();
					//Debug.Log("state change : laststate: "+lastState);
										currentState = stateList [i];
                                        currentStateName = type.Name;
										currentState.Awake ();
										break;
								}

						}
				}
				
		}

		public void changeState<T> () where T :State
		{
				if (typeof(T) != currentState.GetType ()) {
			
						int l = stateList.Count;
						for (int i=0; i<l; i++) {
								if (stateList [i].GetType () == typeof(T)) {
					
										currentState.Sleep ();
										currentState = stateList [i];
										currentState.Awake ();
										break;
								}
				
						}
				}
		
		}

		public void Update ()
		{
				currentState.Update ();

		}
}
