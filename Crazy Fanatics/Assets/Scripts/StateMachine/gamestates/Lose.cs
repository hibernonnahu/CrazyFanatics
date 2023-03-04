using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class Lose : GameState
{
    protected Human loseBy;
    private float reloadTime = 1.55f;
    private float counter;
    private float zoomSpeed = 20f;
    private float lerpSpeed = 4f;
    private Quaternion initialCameraRotation;
	public Lose(GameManager g,StateMachine sm):base(sm,g)
	{
        if(Camera.main)
        initialCameraRotation= Camera.main.transform.rotation;
	}

	public override void Awake ()
	{
        //Debug.Log("idle awake");
        loseBy = GameManager.Instance.MainCharacter;
        counter = reloadTime;
        SoundManager.Instance.PlayMusic("lose");
        Time.timeScale = 0.06f;
        Camera.main.GetComponent<InputHandler>().enabled = false;
	}

	public override void Sleep ()
	{
        Camera.main.GetComponent<InputHandler>().enabled = true;
        Time.timeScale = 1f;
        Camera.main.fieldOfView = 60;
        Camera.main.transform.rotation=initialCameraRotation;
    }

	public override void Update ()
	{
        counter -= Time.deltaTime;
        Camera.main.fieldOfView -=  Time.deltaTime*zoomSpeed;
        Camera.main.transform.forward += Vector3.Slerp(Camera.main.transform.forward, ((loseBy.transform.position+Vector3.up*2f) - Camera.main.transform.position), Time.deltaTime*lerpSpeed);
        if (counter<0)
        {
            counter = float.MaxValue;
            GameManager.Instance.DestroyAll();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if(Input.GetMouseButton(0)&&counter<reloadTime*0.75f)
        {
            counter = -1;
        }
	}

	
	
    //public override void ChangeState(Type type)
    //{
        
    //}
}
