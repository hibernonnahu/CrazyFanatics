using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatIA : Human {

    // public bool converted = false;
    Human mainTarget;


    protected override int GetGenderCode()
    {
        if (AttributesValues.enemyGender == 2) {
            return UnityEngine.Random.Range(0, 2);
        }
        return AttributesValues.enemyGender;
    }
    protected override void Start()
    {
        base.Start();

        stateMachine.addState(new RandomWalk(this, stateMachine));
        stateMachine.addState(new ProtectedRandomWalk(this, stateMachine));
        stateMachine.addState(new GoToPositionTarget(this, stateMachine));
        stateMachine.addState(new GoThroughtPathPoint(this, stateMachine));
        stateMachine.addState(new GoThroughtPathTarget(this, stateMachine));
        stateMachine.addState(new GoToPositionInput(this, stateMachine));
        stateMachine.addState(new Dead(this, stateMachine));
        stateMachine.addState(new Hit(this, stateMachine));

        stateMachine.changeState(typeof(RandomWalk));
        if(protectedHuman!=GameManager.Instance.MainCharacter)
        {
            enemyLayer = LayerMask.NameToLayer("human");
            enemyList = GameManager.Instance.allyList;
            allyList = GameManager.Instance.combatIAList;
            SetOnRestBehavior();
           
        }

        LookAt(transform.position - Vector3.forward);

    }
    override protected void SetOnRestBehavior()
    {
        switch (stats.onRestType)
        {
            case Stats.OnRestType.none:
                onRest = ()=> { };
                decitionBehaviour = Alert;
                break;
            case Stats.OnRestType.patrol:
                onRest = Patrol;
                decitionBehaviour = Alert;
                break;
            case Stats.OnRestType.attackMain:
                mainTarget = GameManager.Instance.MainCharacter;
                onRest = ChaseMainTarget;
                decitionBehaviour = AlertOnMainTarget;
                break;
            case Stats.OnRestType.attackCastle:
                mainTarget = GameManager.Instance.Castle;
                onRest = ChaseMainTarget;
                decitionBehaviour = AlertOnMainTarget;
                break;
            case Stats.OnRestType.castSpellAoE:

                onRest = CastSpellAoE;
                decitionBehaviour = CastSpellAoE;
                break;
            case Stats.OnRestType.healerBuffer:

                onRest = castBuffSpell;//curar 
                decitionBehaviour = Alert;
                break;
        }
    }
    protected void CheckForEnemiesOnMainTarget()
    {
        if(debug)
        { }
        if (Target != null)
        {


            //Debug.Log((Target.transform.position - protectedHuman.transform.position).sqrMagnitude);
            float dist = (Target.GetTargetGameObject().transform.position - transform.position).sqrMagnitude;
            if (!(dist > stats.Radius && dist > Target.GetRadius()))
            {
                return;
            }
            else if (mainTarget.gameObject!=Target.GetTargetGameObject() && (Target.GetTargetGameObject().transform.position - protectedHuman.transform.position).sqrMagnitude > stats.persecutionRange)
            {
                Target = null;
            }

        }
        if (enemyList.Count > 0)
        {
            float minDist = float.MaxValue;
            Human closest = null;
            Human secondClosest = null;
            foreach (var item in enemyList)
            {
                if (item != this && (protectedHuman.transform.position - item.transform.position).sqrMagnitude < stats.protectionVisionRange)
                {
                    float dist = (Transform.position - item.Transform.position).sqrMagnitude;
                    if (dist < minDist)
                    {
                        minDist = dist;
                        secondClosest = closest;
                        closest = item;
                    }
                }

            }
            GameObject targetGO = null;
            if(Target!=null)
            {
                targetGO = Target.GetTargetGameObject();
            }
            if ((!(closest == targetGO) && !(Target == null && closest == null)))
            {
                if (secondClosest != null && !IsClosetTo(closest))
                {
                    Target = secondClosest;
                    stateMachine.currentState.ChangeState(typeof(GoToPositionTarget));
                }
                else if(closest!=null)
                {
                    Target = closest;
                    stateMachine.currentState.ChangeState(typeof(GoToPositionTarget));
                }
                
            }
            else if (Target == null)
            {
                OnCancelPersecution();
            }
        }
        else
        {
            OnCancelPersecution();
        }
    }
    
    private void castBuffSpell()
    {
        GetCurrentState().ChangeState(typeof(Idle));
        bool castBuff = false;
        if (skill != null && skill.IsAvailable(this))
        {
            foreach (var human in allyList)
            {
                if(skill.IACastCondition(human)&&(human.transform.position-transform.position).sqrMagnitude<skill.castDistanse&&!(skill.afectType==Skill.Afects.allysonly&&human==this))
                {
                    Target = human;
                    CastSkill();
                    castBuff = true;
                    break;
                }
            }

        }
        if(!castBuff)
        {
            Patrol();
        }
    }
    private void CastSpellAoE()
    {
        if (skill != null && skill.IsAvailable(this))
        {
            CastSkill();
           
        }
    }
    private void ChaseMainTarget()
    {
        if(debug)
        { }
        Target = mainTarget;
        stateMachine.currentState.ChangeState(typeof(GoToPositionTarget));
    }
    
    override public void Convert(Stats enemyStats,Human h,bool xp=true)
    {
        //converted = true;
      
        
        h.Target = null;
        selected.SetActive(false);
       
        
        
        protectedHuman = h;
        
        skinA.SetActive(false);
        skinB.SetActive(true);
        currentSkin = skinB;
        animator = currentSkin.GetComponent<Animator>();
        //currentSkin.transform.Find("bar").GetComponent<MeshRenderer>().enabled = false;
        gameObject.layer = h.gameObject.layer;
        enemyLayer = h.enemyLayer;
        if (GameManager.Instance.combatIAList.Contains(this))//se hace azul
        {
            WinningManager.Instance.CheckEnemyKill(stats.id);
            GameManager.Instance.combatIAList.Remove(this);
            GameManager.Instance.allyList.Add(this);
           // GameManager.Instance.MainCharacter.UpdateAllysUI();
            enemyList = GameManager.Instance.combatIAList;
            allyList = GameManager.Instance.allyList;
            if(Stats.OnRestType.healerBuffer!=stats.onRestType)
            {
                onRest = Patrol;
            }
            else
            {
                onRest = castBuffSpell;
            }
            Debug.Log("avisar al trigger2");
        }
        //else//rojito
        //{
            
        //    GameManager.Instance.combatIAList.Add(this);
        //    GameManager.Instance.allyList.Remove(this);
        //    //GameManager.Instance.MainCharacter.UpdateAllysUI();
        //    enemyList = GameManager.Instance.allyList;
        //    allyList = GameManager.Instance.combatIAList;
        //    SetOnRestBehavior();
        //}
        if (xp)
        {
            h.protectedHuman.ReceiveXP(stats.rewardXP);
            GameObject go = SetInteractionFeedBack("charm");
            InteractionFeedback ifaux = go.GetComponent<InteractionFeedback>();
           // go.GetComponent<Material>().color=(Color.black);
            ifaux.SetObjectToGo(GameManager.Instance.MainCharacter.GetUINewAllyGameObject().gameObject, GameManager.Instance.MainCharacter.UpdateAllysUI);
            //Debug.Break();
            //Debug.Log("ifaux null "+ (ifaux==null));
        }
        stateMachine.changeState(typeof(ProtectedRandomWalk));

        decitionBehaviour = Alert;

        
    }
    public override void Alert()
    {
        if (counter < 0)
        {
            if (debug)
            { }
            CheckForEnemies();
            counter = tick;
        }
        counter -= Time.deltaTime;
    }
    private  void AlertOnMainTarget()
    {
        if (counter < 0)
        {
            CheckForEnemiesOnMainTarget();
            counter = tick;
        }
        counter -= Time.deltaTime;
    }
    public override void OnProtectedAttack()
    {
        if(protectedHuman!=this&&protectedHuman.Target!=null&&(Target==null||Target==protectedHuman.Target))
        {
            Target = protectedHuman.Target;
            stateMachine.currentState.ChangeState(typeof(Idle));
            stateMachine.currentState.ChangeState(typeof(GoToPositionTarget));
        }
    }
    public override Type OnPointArrive()
    {
        if(protectedHuman==this)
        {
            return typeof(RandomWalk);
        }
        else
        {
            return typeof(ProtectedRandomWalk);
        }
    }
    
   
    private void Patrol()
    {
        if(protectedHuman!=this)
        {
            initialPosition = protectedHuman.transform.position;
            stateMachine.currentState.ChangeState(typeof(Idle));
            stateMachine.currentState.ChangeState(typeof(ProtectedRandomWalk));

        }
        else
        {
            stateMachine.currentState.ChangeState(typeof(RandomWalk));
        }
        
    }
    public override void OnCancelPersecution()
    {
        onRest();
        
    }

    
    override protected void Die(Human murderHuman)
    {
       
        GameManager.Instance.allyList.Remove(this);
        GameManager.Instance.combatIAList.Remove(this);
        GameManager.Instance.MainCharacter.UpdateAllysUI();
        base.Die(murderHuman);
    }
}
