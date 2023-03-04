using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Exit : MonoBehaviour {
    private bool exitEnable = false;
    public ParticleSystem particle;
    public GameObject closedParticles;
    private Collider boxCollider;
    private Renderer renderer;
    private Action onUpdate;
    private MainCharacter mainCharacter;
	// Use this for initialization
	void Start () {
        //particle = GetComponent<ParticleSystem>();
        
        particle.Stop();
        boxCollider = GetComponent<SphereCollider>();
        boxCollider.enabled = false;
        renderer = GetComponent<Renderer>();
        onUpdate = () => { };
        mainCharacter = GameManager.Instance.MainCharacter;
	}
	
	// Update is called once per frame
	void Update () {
        onUpdate();
	    //Debug.Log(renderer.isVisible);
	}
    public void Open()
    {
        particle.Play();
        closedParticles.SetActive(false);exitEnable = true;
        boxCollider.enabled = true;
        onUpdate = () => {
            if(!renderer.isVisible)
            {
                mainCharacter.compas.gameObject.SetActive(true);
                mainCharacter.compas.UpdatePointAt(transform.position);
            }
            else
            {
                mainCharacter.compas.gameObject.SetActive(false);
            }
        };
        Human castle = GameManager.Instance.Castle;
        if (castle != null)
        {
            castle.GetComponent<Castle>().StopUpdate();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (exitEnable&& !(GameManager.Instance.MainCharacter.GetCurrentState() is NullState))
        {
            exitEnable = false;
            GameManager.Instance.StateMachine.changeState(typeof(Win));
            mainCharacter.compas.gameObject.SetActive(false);
            GameManager.Instance.MainCharacter.ChangeState(typeof(CharacterExit));
            FxSoundManager.Instance.PlayFx(13);

            GameManager.Instance.MainCharacter.Rigidbody.constraints = RigidbodyConstraints.None;
            GameManager.Instance.MainCharacter.SetAnimation("floating", 0, 1);
            onUpdate = () =>
            {
                //GameManager.Instance.MainCharacter.transform.Rotate(Vector3.forward * Time.deltaTime * 300);
                GameManager.Instance.MainCharacter.transform.localScale *= (1 - Time.deltaTime * 0.5f);
            };
        }
    }
}
