using UnityEngine;
using System.Collections;
using System;

public class CharacterIntro : HumanState
{

    Vector3 direction;
    Vector4 color = new Vector4(0, 0, 0, 1);
	public CharacterIntro(Human human,StateMachine sm):base(sm,human)
	{
        
	}

	public override void Awake ()
	{
        //Debug.Log("idle awake");
        
        direction = GameManager.Instance.MainCharacter.initialPosition - human.transform.position;
        direction = CustomMath.XZNormalize(direction);
        GameManager.Instance.fade.material.SetColor("_Color", Color.black);
        human.LookAt(GameManager.Instance.MainCharacter.initialPosition);
        human.SetAnimation("walk", 0, human.stats.speed);
    }

	public override void Sleep ()
	{
        
        GameManager.Instance.fade.material.SetColor("_Color", color *0);
    }

	public override void Update ()
	{
        float dist = (GameManager.Instance.MainCharacter.initialPosition.x * Vector3.right + GameManager.Instance.MainCharacter.initialPosition.z * Vector3.forward - (human.transform.position.x * Vector3.right + human.transform.position.z * Vector3.forward)).sqrMagnitude;
        GameManager.Instance.fade.material.SetColor("_Color",color* dist *0.03f);
        //Debug.Log(GameManager.Instance.MainCharacter.initialPosition + " " + human.transform.position);
        //Debug.Log((GameManager.Instance.MainCharacter.initialPosition.x * Vector3.right + GameManager.Instance.MainCharacter.initialPosition.z * Vector3.forward - (human.transform.position.x * Vector3.right + human.transform.position.z * Vector3.forward)).sqrMagnitude);
       if (dist<0.1f)
        {
            ChangeState(typeof(Idle));
        }
        else
        {
            human.Rigidbody.velocity = direction*human.stats.speed;
        }
	}

	
	public override void OnCollision(Collision col)
	{}
	public override void OnTrigger(Collider col){}
    //public override void ChangeState(Type type)
    //{

    //}
   
}
