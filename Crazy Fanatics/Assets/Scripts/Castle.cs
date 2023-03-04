using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Castle : Human {
    private MainCharacter mainCharacter;
    private Action onUpdate;
    private Renderer renderer;

    protected override void Start()
    {
        base.Start();
        stateMachine.addState(new Dead(this, stateMachine));
        renderer =currentSkin.GetComponentInChildren<Renderer>();
        onUpdate = () => {

            if (!renderer.isVisible)
            {
                mainCharacter.compas.gameObject.SetActive(true);
                mainCharacter.compas.UpdatePointAt(transform.position);
            }
            else
            {
                mainCharacter.compas.gameObject.SetActive(false);
            }
        };
        mainCharacter = GameManager.Instance.MainCharacter;
    }
    private void Update()
    {
        onUpdate();
    }
    public void StopUpdate()
    {
        mainCharacter.compas.gameObject.SetActive(false);
        onUpdate = () => { };
    }
    protected override void Die(Human murderHuman)
    {

        ChangeState(typeof(Dead));
        
        GameManager.Instance.StateMachine.changeState(typeof(LoseCastle));
    }
}
