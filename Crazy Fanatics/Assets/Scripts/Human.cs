using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Human : MonoBehaviour,ITargeteable {
    protected Animator animator;
    protected Action onRest=()=> { };
    public bool debug=false;
    public int id = -1;
    private ParticleSystem onHitEffect;
    public ParticleSystem OnHitEffect {
        get { return onHitEffect; }
    }
    public List<Human> enemyList;
    public List<Human> allyList;
    protected float counter = 0;
    protected float tick = 1f;
    protected bool dead = false;
    public int enemyLayer;
    public Human protectedHuman;
    private Collider realCollider;
    protected GameObject[] sword=new GameObject[2];
    protected GameObject[] dagger = new GameObject[2];
    protected GameObject[] axe = new GameObject[2];
    private LineRenderer lineEffect;
    public LineRenderer LineEffect
    {
        get
        {
            return lineEffect;
        }
    }
    public void PlaySwordSound() {
        FxSoundManager.Instance.PlayFx(stats.attackSoundCode);
    }
    public virtual void OnDeadEnter()
    {
        
    }
    public Human GetHuman() { return this; }
    public Item GetItem() { return null; }
    
    public void SetSelected(bool b) { selected.SetActive(b); }
    public GameObject GetTargetGameObject()
    {
        return gameObject;
    }
    public float GetRadius()
    {
        return stats.Radius;
    }
    private GameObject rayEffect;
    public GameObject RayEffect
    {
        get
        {
            return rayEffect;
        }
        set
        {
            rayEffect = value;
            lineEffect = RayEffect.GetComponentInChildren<LineRenderer>();
        }
    }

    virtual public void OnDeadUpdate()
    {
        transform.localScale *= 0.9f;
    }

    private GameObject effect;
    public GameObject Effect
    {
        get
        {
            return effect;
        }
        set
        {
            effect = value;
            
        }
    }
    public bool Dead
    {
        get
        {
            return dead;
        }
        set
        {
            dead = value;
        }
    }
    virtual public void SetAnimation(string name,float transition, float speed)
    {
        if(animator!=null)
        {
            animator.speed = speed;
            animator.CrossFade(name, transition);
        }
    }

    public virtual void LoadInventoryItem(InventoryItem inventoryItem, bool drop = true)
    {
        
    }

    private float skillEffectDuration;
    public float SkillEffectDuration
    {
        get
        {
            return skillEffectDuration;
        }
        set
        {
            skillEffectDuration = value;
        }
    }

    public virtual float GetWeaponAttackDistance()
    {
        return 0;
    }

    

    public GameObject mainBar;
    public GameObject bar;
    protected Item item;
    public Item Item
    {
        get
        {
            return item;
        }
        set
        {
            item = value;
        }
    }
    private ITargeteable target;
    public ITargeteable Target
    {
        get
        {
            return target;
        }
        set
        {
            target = value;
        }
    }

    public float GetLifePercent()
    {
        return (float)life/GetMaxLife();
    }
    private void TurnOffRayEffect()
    {
        if (rayEffect != null)
        {
            rayEffect.SetActive(false);
        }
    }
    private void TurnOffEffect()
    {
        if (Effect != null)
        {
            Effect.SetActive(false);
        }
    }
    virtual public void MarkPosition(bool v)
    {
        
    }

    public void CancelSkill()
    {
        if(skill!=null)
        {
            skill.CancelSkillEffect(this);
        }
    }

    public CoolDownImgHandler skillImgHandler;
    public CoolDownImgHandler itemConsumibleHandler;
    public Text itemConsumibleCountText;
    
    
    
    public Stats stats;
    public Skill skill;
   
    private float skillCounter = 0;
    private float itemConsumibleCounter = 0;
    public float ItemConsumibleCounter {
        get { return itemConsumibleCounter; }
        set { itemConsumibleCounter = value; }
    }
    protected List<ICooldown> cooldownList = new List<ICooldown>();

    protected int life;
    public int Life {
        get { return life; }
    }
    
    
    public GameObject selected;
    public Vector3 initialPosition;

    public void CastSkill()
    {
        cooldownList.Add(skill);
        SetAnimation(skill.animation, 0, 1);
        if (target != null)
        {
            LookAt(target.GetHuman().transform.position);
        }
        skill.OnExecute(target, this);
        
        
    }

    public Action decitionBehaviour;
    protected GameObject skinA;
    protected GameObject skinB;
    public GameObject currentSkin ;
    protected StateMachine stateMachine;



    public void LoadSkill(Skill skill, bool direct = false)
    {

        this.skill = skill;
        if (skill == null)
        {
            skillImgHandler.TurnOn(false);
        }
        else
        {

            skill.Init(this);
            if (direct) {
                skillImgHandler.TurnOn(true);
            }
            else
            {
                GameObject go = SetInteractionFeedBack("skill");
                InteractionFeedback ifaux = go.GetComponent<InteractionFeedback>();
                // go.GetComponent<Material>().color=(Color.black);
                ifaux.SetObjectToGo(null, () =>
                {

                    skillImgHandler.TurnOn(true);

                });
            }

        }
        
        RamCharacterAttributes.GetInstance().skill = skill;
    }
    internal void LookAt(Vector3 point)
    {
        currentSkin.transform.LookAt(Vector3.right * point.x + Vector3.up * Transform.position.y + Vector3.forward * point.z);

    }

    public List<Node> path;
    private Vector3 dodge;
    public Vector3 Dodge
    {
        get
        {
            return dodge;
        }
        set
        {
            dodge = value;
        }
    }
    private Vector3 destiny;
    public Vector3 Destiny
    {
        get
        {
            return destiny;
        }
        set
        {
            destiny = value;
        }
    }
    protected Vector3 inputDestiny;
    public Vector3 InputDestiny
    {
        get
        {
            return inputDestiny;
        }
        set
        {
            inputDestiny = value;
            destiny = value;
        }
    }

    public void RenewPathToPosition(Vector3 position)
    {
        Node destinyNode = Astar.getInstance().GetClosestNode(position);
      
        if (path == null  )
        {
            CreatePath(destinyNode);
            
        }
        else
        {
            int count = path.Count;
            if (count > 1 && path[count - 1] != destinyNode || count <= 1)
            {
                CreatePath(destinyNode);
            }
        }
    }

    public void Kill()
    {
        stateMachine.currentState.ChangeState(typeof(Dead));
    }

    private void CreatePath(Node destinyNode)
    {
        path = Astar.getInstance().getPath(Astar.getInstance().GetClosestNode(transform.position),
                    destinyNode);
    }
    private new Rigidbody  rigidbody;
    public Rigidbody Rigidbody
    {
        get
        {
            return this.rigidbody;
        }
        set
        {
            this.rigidbody = value;
        }
    }
    private new Transform transform;
    public Transform Transform
    {
        get
        {
            return this.transform;
        }
        set
        {
            this.transform = value;
        }
    }

    public float SkillCounter
    {
        get
        {
            return skillCounter;
        }

        set
        {
            skillCounter = value;
        }
    }




    //void PopulateDecitionList()
    //{
    //    string[] list = Enum.GetNames(typeof(DecitionType));
    //}
    virtual protected TextMesh SetTextVariationFeedBack(int num,Color positive,Color negative,string addText="",float yoffset=0,string gotoTag="")
    {
        Pool p = GameManager.Instance.GetPoolDictionary()["text"];
        GameObject go= p.GetElement(transform.position+Vector3.up*(stats.height+ yoffset));//fix error
        if(go==null)
        {
            return null;
        }
        //if(gotoTag!="")
        //{
        //    go.GetComponent<InteractionFeedback>().GoTo(gotoTag);
        //}
        //else
        //{
        //    go.GetComponent<InteractionFeedback>().GoNormal();
        //}
        

        TextMesh tm = go.GetComponent<TextMesh>();
        if (num<=0)
        {
            tm.color = negative;
        }
        else
        {
            tm.color = positive;
        }
        
        tm.text = addText+ num.ToString();
        return tm;
    }
    public GameObject SetInteractionFeedBack(string name,float optionalTime=-1)
    {
        Pool p = GameManager.Instance.GetPoolDictionary()[name];
        return p.GetElement(transform.position+Vector3.up*stats.height,true,optionalTime);
    }
    public void GiveReward(Human killer)
    {
        const float vel = 2.7f;
        const float addIVel = 0.3f;
        const float timer = 50f;
        const float random = 0.5f;
        const float upVel = 2f;
        Pool p = GameManager.Instance.GetPoolDictionary()["xp"];
        for (int i = 0; i < stats.rewardXP; i++)
        {
            Item xptemp = p.GetElement(transform.position + Vector3.up * stats.height, true, timer).GetComponent<Item>();
            xptemp.xpAdd = 1;
            xptemp.Renew();
            xptemp.GetComponent<Rigidbody>().velocity = ((transform.position + Vector3.forward * UnityEngine.Random.Range(-random, random) + Vector3.right * UnityEngine.Random.Range(-random, random)) - killer.transform.position) * (vel+i*addIVel) + Vector3.up * upVel;
        }
       
        Pool p2 = GameManager.Instance.GetPoolDictionary()["gold"];
        for (int i = 0; i < stats.rewardCoin; i++)
        {
            Item goldtemp = p2.GetElement(transform.position + Vector3.up * stats.height, true, timer).GetComponent<Item>();
            goldtemp.goldAdd = 1;
            goldtemp.GetComponent<Rigidbody>().velocity = ((transform.position + Vector3.forward * UnityEngine.Random.Range(-random, random) + Vector3.right * UnityEngine.Random.Range(-random, random)) - killer.transform.position) * (vel + i * addIVel) + Vector3.up * upVel;
            goldtemp.Renew();
        }
        if(stats.rewardInventoryItem&&stats.rewardInventoryItemChance>= UnityEngine.Random.Range(0,1f))
        {
            GameObject go=Instantiate(stats.rewardInventoryItem);
            go.transform.position = transform.position + Vector3.up * stats.height;
            go.GetComponent<Rigidbody>().velocity = ((transform.position + Vector3.forward * UnityEngine.Random.Range(-random, random) + Vector3.right * UnityEngine.Random.Range(-random, random)) - killer.transform.position) * (vel ) + Vector3.up * upVel;
        }
        Debug.Log("avisar al trigger");
    }
    virtual public void UpdateSkillUI(float percent)
    {

    }
    protected virtual int GetGenderCode() {
        return 0;
    }
    Action onHitEffectExecution = () => { };
    public void OnMakeHitEffect() {
        onHitEffectExecution();
    }
    protected virtual void Awake()
    {
        if (stats.onHitEffect != null) {
            onHitEffect = Instantiate<ParticleSystem>(stats.onHitEffect);
            onHitEffectExecution = () =>
            {
                Vector3 offset = CustomMath.Normalize(transform.position - target.GetHuman().transform.position);
                onHitEffect.transform.position = target.GetHuman().transform.position + offset * target.GetRadius() + Vector3.up * target.GetHuman().stats.height * 0.6f;
                onHitEffect.Stop();
                onHitEffect.Play();
            };
        }
        CapsuleCollider[] cc= GetComponents<CapsuleCollider>();
        foreach (var item in cc)
        {
            if(!item.isTrigger)
            {
                realCollider = item;
            }
        }
        int gender = GetGenderCode();
        if (stats.humanSkinA!=null&&stats.humanSkinA.Length > 5)
        {
            skinA = Instantiate<GameObject>( Resources.Load("Models/" + gender.ToString()) as GameObject, gameObject.transform);
            Skinner skinner = skinA.GetComponent<Skinner>();
            skinner.SetHead(stats.humanSkinA[1]);
            skinner.SetBack(stats.humanSkinA[2]);
            skinner.SetBody(stats.humanSkinA[3]);
            skinner.SetHands(stats.humanSkinA[4]);
            skinner.SetFoots(stats.humanSkinA[5]);
            
            sword[0] = Utils.FindGameObjectInChildren("sword", skinA);
            dagger[0] = Utils.FindGameObjectInChildren("dagger", skinA);
            axe[0] = Utils.FindGameObjectInChildren("axe", skinA);

            sword[0].SetActive(stats.humanSkinA[6] == 1);
            dagger[0].SetActive(stats.humanSkinA[6] == 2);
            axe[0].SetActive(stats.humanSkinA[6] == 3);

            Destroy(skinner);
        }
        else
        skinA = Instantiate<GameObject>(stats.skinA,gameObject.transform);
        currentSkin = skinA;
        animator =currentSkin.GetComponent<Animator>();
        if (stats.humanSkinB != null && stats.humanSkinB.Length >5)
        {
            skinB = Instantiate<GameObject>(Resources.Load("Models/" + gender.ToString()) as GameObject, gameObject.transform);
            Skinner skinner = skinB.GetComponent<Skinner>();
            skinner.SetHead(stats.humanSkinB[1]);
            skinner.SetBack(stats.humanSkinB[2]);
            skinner.SetBody(stats.humanSkinB[3]);
            skinner.SetHands(stats.humanSkinB[4]);
            skinner.SetFoots(stats.humanSkinB[5]);
            Destroy(skinB.GetComponent<Skinner>());
            sword[1] = Utils.FindGameObjectInChildren("sword", skinB);
            dagger[1] = Utils.FindGameObjectInChildren("dagger", skinB);
            axe[1] = Utils.FindGameObjectInChildren("axe", skinB);
            sword[1].SetActive(stats.humanSkinA[6] == 1);
            dagger[1].SetActive(stats.humanSkinA[6] == 2);
            axe[1].SetActive(stats.humanSkinA[6] == 3);
        }
        else
        skinB = Instantiate<GameObject>(stats.skinB, gameObject.transform);
        skinB.SetActive(false);
        skinA.transform.localScale = skinB.transform.localScale = Vector3.one * stats.scale;
        this.rigidbody = GetComponent<Rigidbody>();
        this.transform = GetComponent<Transform>();
        SetDecitionBehavior();
        stateMachine = new StateMachine();
        stateMachine.addState(new Idle(this, stateMachine));
        stateMachine.addState(new WaitAnimation(this, stateMachine));
        stateMachine.addState(new NullState(this, stateMachine));
    }
    public  void EnableCollider(bool b) {
        realCollider.enabled = b;
    }
    private void SetDecitionBehavior()
    {
        switch(stats.decitionType)
        {
            case Stats.DecitionType.input:
                decitionBehaviour = MovementInput;
                break;
            case Stats.DecitionType.attack:
                decitionBehaviour = Alert;
                break;
            case Stats.DecitionType.none:
                decitionBehaviour = ()=> { };
                break;
        }
    }
    virtual protected void SetOnRestBehavior()
    {
        switch (stats.onRestType)
        {
            case Stats.OnRestType.none:
                onRest = () => { };
                break;
        }
    }
    protected virtual void Start()
    {
       if(protectedHuman==null)
        {
            protectedHuman = this;
        }
        if(skill!=null)
        {
            skill.Init(this);
        }
        
        if(selected!=null)
        {
            selected.SetActive(false);
        }
        life = GetMaxLife();
        
        UpdateLifeBar();
        UpdateMainBarView(0);
        mainBar.transform.localPosition = Vector3.up * stats.height;
        counter *= UnityEngine.Random.Range(0, 3);
    }
    private void UpdateMainBarView(float s)
    {
        mainBar.transform.localScale = mainBar.transform.localScale.y * Vector3.up + mainBar.transform.localScale.z * Vector3.forward + Vector3.right * s;
    }
    virtual public void ActivateSelected(bool active)
    {
        
    }
    virtual public void HelpWanted()
    {
        HumanState hs = (HumanState)(stateMachine.currentState);
        hs.OnHelpWanted();
    }
    virtual public void OnProtectedAttack()
    {

    }
    virtual public void Alert() {
        
    }
    
    protected void CheckForEnemies()
    {
       
        if (Target != null)
        {
            if(Target.GetTargetGameObject() == protectedHuman.gameObject)
            {
                if(protectedHuman.Target!=null)
                {
                    target = protectedHuman.target;
                    stateMachine.currentState.ChangeState(typeof(GoToPositionTarget));
                }
                return;
            }
            else if(this != protectedHuman&&target == protectedHuman.target)
            {
                return;
            }
            //Debug.Log((Target.transform.position - protectedHuman.transform.position).sqrMagnitude);
            float dist = (Target.GetTargetGameObject(). transform.position - transform.position).sqrMagnitude;
            if (!(dist > stats.Radius && dist > target.GetRadius()))
            {
                return;
            }
            else if ((Target.GetTargetGameObject().transform.position - protectedHuman.transform.position).sqrMagnitude > stats.persecutionRange)
            {
                Target = null;
            }
           
        }
        if (enemyList.Count > 0)
        {
            float minDist = float.MaxValue;
            Human closest = null;
           // Human secondClosest = null;
            foreach (var item in enemyList)
            {
                if (item != this && (protectedHuman.transform.position - item.transform.position).sqrMagnitude < stats.protectionVisionRange)
                {
                    float dist = (Transform.position - item.Transform.position).sqrMagnitude;
                    if (dist < minDist)
                    {
                        minDist = dist;
                        //secondClosest = closest;
                        closest = item;
                    }
                }

            }
            GameObject targetGO=null;
            if(target!=null)
            {
                targetGO = Target.GetTargetGameObject();
            }
            GameObject closestGO = null;
            if (closest != null)
            {
                closestGO = closest.gameObject;
            }
            if ((!(closestGO == targetGO) && !(Target == null && closest == null)))
            {
                //if (secondClosest != null && !IsClosetTo(closest))
                //{
                //    Target = secondClosest;
                //}
                //else
                {
                    Target = closest;
                }
                stateMachine.currentState.ChangeState(typeof(GoToPositionTarget));
            }
            else if(Target == null)
            {
                OnCancelPersecution();
            }
        }
        else
        {
            OnCancelPersecution();
        }
    }
    protected bool IsClosetTo(Human closest)
    {
        float dist = (transform.position - closest.transform.position).sqrMagnitude;
        foreach (var item in enemyList)
        {
            if (item != this)
            {
                if ((item.transform.position - closest.transform.position).sqrMagnitude < dist)
                {
                    return false;
                }
            }
        }
        return true;
    }
    virtual protected void MovementInput() { }
    virtual public Type OnPointArrive()
    {
        return null;
    }
    public State GetCurrentState()
    {
        return stateMachine.currentState;
    }
    private void Update()
    {
        barUpdate();
        for (int i = cooldownList.Count - 1; i >= 0; i--)
        {
            cooldownList[i].UpdateCoolDown(cooldownList, this);
        }
    }
    private void FixedUpdate()
    {
       
        stateMachine.Update();
        
    }
   
    virtual public void SetInitialPosition(Vector3 initialPos)
    {
        initialPosition  = initialPos;
        transform.position = Vector3.right * initialPos.x + Vector3.up * transform.position.y + Vector3.forward * initialPos.z;
    }
    private void OnCollisionEnter(Collision collision)
    {
        stateMachine.currentState.OnCollision(collision);
    }
    private void OnCollisionStay(Collision collision)
    {
        //Debug.Log("coll stay");
        stateMachine.currentState.OnCollisionStay(collision);
    }
    public virtual void OnCancelPersecution()
    {
        onRest();
    }
    
    virtual public void ReceiveXP(int xp)
    {
        
    }
    virtual public void ReceiveGold(int gold)
    {

    }
    virtual public void ActivateSkillUI(bool b) { }
    virtual public void Convert(Stats stats,Human h,bool xp=true)
    { }
    virtual protected bool RoomToConvert(Human h)
    {
        return ((h.allyList.Count - 1) < h.GetMaxAllys());
    }
    virtual public bool CanConvert(Human victim)
    {
        //return humanHitter.GetCharm() - stats.charmCancel > life && RoomToConvert(humanHitter.protectedHuman);
        return false;
    }
    virtual public int TakeHit(Human humanHitter,int damage)
    {
        //Debug.Log("takehit echarm:"+ enemyStats.charm +" - charmCancel:"+ stats.charmCancel + " >=life" + stats.life);
        //if(CanConvert(humanHitter,damage))
        //{
        //    Human hp = humanHitter.protectedHuman;
            
        //    Convert(stats, hp);
        //    return 0;
        //}
        //else
        {
            int dmg = damage - GetDefense();
            if(dmg<1)
            {
                dmg = 1;
            }
            
            life -= dmg;
            if(life<=0)
            {
                life = 0;
                Die(humanHitter);
            }
           // else
            {
                if (humanHitter.gameObject == GameManager.Instance.MainCharacter.gameObject||gameObject == GameManager.Instance.MainCharacter.gameObject)
                {
                    SetTextVariationFeedBack(-dmg, Color.green, Color.red);
                }
                
            }
            UpdateLifeBar();
            return dmg;
        }
    }
    
    virtual public int ModifyLife(int variationLife)
    {
        if (variationLife != 0)
        {
            life += variationLife;
            if (life <= 0)
            {
                life = 0;
                Die(null);
            }

            else
            {
                int maxLife = GetMaxLife();
                if (life > maxLife)
                {
                    variationLife -= life - maxLife;
                    life = maxLife;
                }
                if (variationLife > 0)
                {
                    SetTextVariationFeedBack(variationLife, Color.green, Color.red, "", 0, "lifebar");

                }
                else if (variationLife < 0)
                {
                    SetTextVariationFeedBack(variationLife, Color.green, Color.red);

                }
            }
            UpdateLifeBar();
        }
        return variationLife;
    }
    virtual protected void Die(Human murderHuman)
    {
       if(murderHuman!=null&&murderHuman.enemyLayer==LayerMask.NameToLayer("victim"))
        {
            //murderHuman.protectedHuman.ReceiveXP(stats.rewardXP);
            //murderHuman.protectedHuman.ReceiveGold(stats.rewardCoin);
            GiveReward(murderHuman);
        }

        stateMachine.currentState.ChangeState(typeof(Dead));
    }
    public void ChangeState(Type t)
    {
        stateMachine.currentState.ChangeState((t));
    }
    Action barUpdate=()=> { };
    private float barCounter;
    protected void UpdateLifeBar()
    {
        UpdateMainBarView(0.15f);
        barCounter = GameManager.Instance.barTime;
        barUpdate = () => {
            barCounter -= Time.deltaTime;
            if (barCounter < 0)
            {
                barUpdate= () =>
                {
                    float aux2 = mainBar.transform.localScale.x;
                    aux2 -= Time.deltaTime;
                    if (aux2 <= 0)
                    {
                        UpdateMainBarView(0);
                        barUpdate = () => { };
                    }
                    else
                    {
                        UpdateMainBarView(aux2);
                    }
                };
            }
        };
        float aux = ((float)life / GetMaxLife());
        
        bar.transform.localScale = Vector3.up * aux + Vector3.right + Vector3.forward;
    }
    virtual public int GetDamage()
    {
        return stats.damage;
    }
    virtual public int GetDefense()
    {
        return stats.defense;
    }
    virtual public float GetCastTime()
    {
        return stats.hitTime;
    }
    
    virtual public int GetMaxLife()
    {
        return stats.maxlife;
    }
    virtual public int GetMaxAllys()
    {
        return 30;
    }
}
