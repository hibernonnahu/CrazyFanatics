using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainCharacter : Human {
    private const float doubleClickTime = 1f;
    private  float minDoubleClickTime = 0.15f;
    private float lastClickTime = 0;
    private CharacterClass characterClass;
    public ParticleSystem particlesLvlUp;
    private RectTransform lifeBar;
    private RectTransform xpBar;
    private Text lifeText;
    private TextMeshProUGUI lvlText;
    private Text allyText;
    private Text xpText;
    private TextMeshProUGUI goldText;
    public ItemConsumible itemConsumible;
    private int[] consumibleItems  ;
    public int[] ConsumibleItems {
        get { return consumibleItems; }
    }
    public GameObject cauldrin;
    private StatsButton statsButton;
    private InteractuableAlone lvlUpIcon;
    private GameObject mouseX;
    private int LayerGround;
    private Func<bool> blockMouse;
    protected const float Y_OFFSET = 0.1f;

    internal void UpdateConsumibleItemCount(int count)
    {
        
        itemConsumibleCountText.text = count.ToString();
    }

    private int mask ;
    public Compas compas;
    private GameObject[] allysUI = new GameObject[20];
    private Image[] allysUIImage = new Image[20];
    public InventoryItem neck;
    public InventoryItem rightHand;
    public InventoryItem leftHand;

    public ParticleSystem[] claudrinNewObjectParticles;
    private GameObject charmedIcon;
    protected override int GetGenderCode()
    {
        return AttributesValues.characterGender;
    }
    protected override void Awake()
    {
        base.Awake();
        
        if (Application.platform==RuntimePlatform.Android)
        {
            blockMouse = BlockMouseAndroid;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            
        }
        else
        {
            minDoubleClickTime = 0;
            blockMouse = BlockMouseDesktop;
        }
        charmedIcon = GameObject.FindGameObjectWithTag("charmedicon");
        GameObject allysImage = GameObject.FindGameObjectWithTag("allysgraphicui");
        if(allysImage!=null)
            for (int i = 0; i < 20; i++)
            {
                foreach (Transform child in allysImage.transform)
                {
                    if(child.name==i.ToString())
                    {
                        allysUI[i] = child.gameObject;
                        allysUIImage[i] = child.GetComponent<Image>();
                    }
                }
                
            }
        
       
        stateMachine.addState(new CharacterIntro(this, stateMachine));
        compas=gameObject.GetComponentInChildren<Compas>();
        compas.gameObject.SetActive(false);
        
        
    }

    internal void NullAllys(bool v)
    {
        foreach (var item in allyList)
        {
            if (item != this) {
                if(v)
                {
                    item.transform.position = item.transform.position.x * Vector3.right + item.transform.position.z * Vector3.forward + Vector3.up * -1000;
                }
                else
                    item.transform.position = item.transform.position.x * Vector3.right + item.transform.position.z * Vector3.forward ;


            }
        }
    }

    public  ParticleSystem[] particlesAlchemy;
    public void SetParticleAmount(ParticleSystem particles, int a) {
        particles.gameObject.SetActive(true);

        var mainModule = particles.main;
        mainModule.maxParticles = a;
        particles.Play();
        if (a == 0) {
            particles.Stop();
            particles.gameObject.SetActive(false);
        }
    }
    private GameObject weapons;
    public GameObject Weapons {
        get { return weapons; }
    }
    protected override void Start()
    {
        base.Start();
        tick = 0.1f;
        mask = 1 << LayerMask.NameToLayer("wall");
        characterClass = new Balanced(this);
        lvlUpIcon = GameObject.FindGameObjectWithTag("lvlupicon").GetComponent<InteractuableAlone>();
        lvlUpIcon.gameObject.SetActive(false);
        //particlesLvlUp = GetComponent<ParticleSystem>();
        particlesLvlUp.transform.position = transform.position;
        particlesLvlUp.transform.SetParent(transform);
        statsButton = FindObjectOfType<StatsButton>();
        lifeBar=GameObject.FindGameObjectWithTag("lifebar").GetComponent<RectTransform>();
        xpBar = GameObject.FindGameObjectWithTag("xpbar").GetComponent<RectTransform>();
        lifeText = GameObject.FindGameObjectWithTag("lifetext").GetComponent<Text>();
        lvlText = GameObject.FindGameObjectWithTag("lvltext").GetComponent<TextMeshProUGUI>();
        allyText = GameObject.FindGameObjectWithTag("allytext").GetComponent<Text>();
        xpText = GameObject.FindGameObjectWithTag("xptext").GetComponent<Text>();
        goldText = GameObject.FindGameObjectWithTag("goldtext").GetComponent<TextMeshProUGUI>();
        mouseX = GameObject.FindGameObjectWithTag("mouseX");
        mouseX.SetActive(false);
        enemyList = GameManager.Instance.combatIAList;
        allyList = GameManager.Instance.allyList;
        LayerGround = 1 << LayerMask.NameToLayer("Ground");
        LayerGround += 1 << LayerMask.NameToLayer("victim");
        LayerGround += 1 << LayerMask.NameToLayer("item");
        //LayerGround += 1 << LayerMask.NameToLayer("item");
        
        stateMachine.addState(new GoToPositionInput(this, stateMachine));
        stateMachine.addState(new GoToPositionTarget(this, stateMachine));
        stateMachine.addState(new GoThroughtPathPoint(this, stateMachine));
        stateMachine.addState(new GoThroughtPathTarget(this, stateMachine));
        stateMachine.addState(new Dead(this, stateMachine));
        stateMachine.addState(new DeadCastle(this, stateMachine));
        stateMachine.addState(new Hit(this, stateMachine));
        stateMachine.addState(new GoToPositionItem(this, stateMachine));
        stateMachine.addState(new CharacterExit(this, stateMachine));
       
        enemyLayer = LayerMask.NameToLayer("victim");
        GameManager.Instance.allyList.Add(this);
        UpdateCharacterUI();
        
        LoadSkill(RamCharacterAttributes.GetInstance().skill,true);
        consumibleItems = RamCharacterAttributes.GetInstance().consumibleItems;
        LoadItemConsumible(RamCharacterAttributes.GetInstance().itemConsumible);
        LoadInventoryItem(RamCharacterAttributes.GetInstance().rightHand,false);
        
        weapons = Utils.FindGameObjectInChildren("weapons", currentSkin);
    }
    public void LoadItemConsumible(ItemConsumible itemConsumible)
    {
        this.itemConsumible = itemConsumible;
        if (itemConsumible == null)
        {
            itemConsumibleHandler.TurnOn(false);
        }
        else
        {

            this.itemConsumible.Init(this);


            itemConsumibleHandler.TurnOn(true);


            itemConsumibleCountText.text = consumibleItems[(int)itemConsumible.type].ToString();

        }

        RamCharacterAttributes.GetInstance().itemConsumible = itemConsumible;
    }
    public void OnSkillButton() { ActivateSkillOnEnemy(); }
    protected void HideItemInventory(InventoryItem iiAux, int index)
    {
        switch (iiAux.slot)
        {
            case InventoryItem.Slot.righthand:
                axe[index].SetActive(false);
                dagger[index].SetActive(false);
                sword[index].SetActive(false);

                break;
            case InventoryItem.Slot.lefthand:
                break;
            case InventoryItem.Slot.neck:
                break;
            default:
                break;
        }
    }
    public override void LoadInventoryItem(InventoryItem inventoryItem,bool drop=true)
    {
        InventoryItem iiAux;
        switch (inventoryItem.slot)
        {
            case InventoryItem.Slot.righthand:
                iiAux = rightHand;
                HideItemInventory(iiAux,0);
                RamCharacterAttributes.GetInstance().rightHand = inventoryItem;
                break;
            case InventoryItem.Slot.lefthand:
                iiAux = leftHand;
                break;
            case InventoryItem.Slot.neck:
                iiAux = neck;
                break;
            default:
                iiAux = neck;
                break;
        }
        //iiAux soltar
        iiAux = inventoryItem;
        ShowItemInventory(iiAux,drop);
    }

    private void ShowItemInventory(InventoryItem iiAux,bool drop)
    {
        switch (iiAux.slot)
        {
            case InventoryItem.Slot.righthand:
                if(drop)
                DropInventoryItem(rightHand);
                rightHand = iiAux;
                
                switch (iiAux.typeOfWeapon)
                {
                    case InventoryItem.TypeOfWeapon.nothing:
                        HideItemInventory(iiAux,0);
                        break;
                    case InventoryItem.TypeOfWeapon.dagger:
                        dagger[0].SetActive(true);
                        break;
                    case InventoryItem.TypeOfWeapon.sword:
                        sword[0].SetActive(true);

                        break;
                    case InventoryItem.TypeOfWeapon.axe:
                        axe[0].SetActive(true);

                        break;
                    default:
                        break;
                }
                break;
            case InventoryItem.Slot.lefthand:
                break;
            case InventoryItem.Slot.neck:
                break;
            default:
                break;
        }
    }

    private void DropInventoryItem(InventoryItem rightHand)
    {
        
        if(rightHand.typeOfWeapon!=InventoryItem.TypeOfWeapon.nothing)
        {
            Transform vaultItemTransform = GameManager.Instance.vault.Find(rightHand.floorItem.name);
            
            if(vaultItemTransform==null)
            {
                vaultItemTransform=Instantiate(rightHand.floorItem).transform;
            }
            else
            {
                vaultItemTransform.parent = null;
            }
            vaultItemTransform.GetComponent<Item>().Renew();
            vaultItemTransform.position = transform.position+Vector3.up*stats.height;
            vaultItemTransform.GetComponent<Rigidbody>().velocity = Vector3.up * 3;
        }
    }

    

    public override void SetAnimation(string name,float transition, float totaltime)
    {
        animator.speed = totaltime;
        animator.CrossFade(name, transition);
    }
    public override void OnDeadUpdate()
    {
        
    }
    public override void OnDeadEnter()
    {
        SetAnimation("death", 0, 1);
    }
   
    
    public override void UpdateSkillUI(float percent)
    {
        skillImgHandler.UpdateCooldown(1 - percent);
        
    }
    public  void UpdateItemConsumibleUI(float percent)
    {
        itemConsumibleHandler.UpdateCooldown(1 - percent);

    }
    public override void ActivateSelected(bool active)
    {
        if(Target!=null)
        {
            Target.SetSelected(active);
            
        }
    }
    public override void Alert()
    {
        if (counter < 0)
        {
            CheckForEnemies();
            counter = tick;
        }
        counter -= Time.deltaTime;
    }
    private void UpdateCharacterUI()
    {
        int nextLvlXP = LvlXp.GetXPToLvl(RamCharacterAttributes.GetInstance().playerLevel);
        float lifePercent = ((float)(life) / GetMaxLife());
        float xpPercent = ((float)(RamCharacterAttributes.GetInstance().currentXP) / nextLvlXP);
        lifeBar.localScale = Vector3.right *lifePercent+Vector3.up + Vector3.forward;
        xpBar.localScale = Vector3.right * xpPercent+ Vector3.up + Vector3.forward;
        xpText.text = RamCharacterAttributes.GetInstance().currentXP.ToString() + "/" +nextLvlXP.ToString();
        goldText.text = RamCharacterAttributes.GetInstance().gold.ToString() ;
        lifeText.text = life.ToString()+"/"+ GetMaxLife().ToString();
        lvlText.text = "Lvl: "+ RamCharacterAttributes.GetInstance().playerLevel.ToString();
        UpdateMaxAllysUI();
        //UpdateAllysUI();
        
        
    }
    public Image GetUINewAllyGameObject()
    {
        int total = allyList.Count - 2;
        if (total < 0)
        { return null; }
        //Debug.Log("allyPos: " + total.ToString() + "/" + RamCharacterAttributes.GetInstance().MaxAllyPoint.ToString());
        return allysUIImage[total];
    }
    public void UpdateAllysUI()
    {
        //allyText.text = "Allys: " + (allyList.Count - 1).ToString() + "/" + RamCharacterAttributes.GetInstance().MaxAllyPoint.ToString();
       
        int total = allyList.Count-1;
        if(total==0)
        {
            charmedIcon.transform.position = Vector3.up * 1000000;
        }
        else
        {
            charmedIcon.transform.position = allysUIImage[total - 1].transform.position;
        }
        for (int i = 0; i < allysUIImage.Length; i++)
        {
            if(i<total)
            {
                allysUIImage[i].color = Color.white;
            }
            else
            {
                allysUIImage[i].color = Color.black;
            }
        }
    }
    public override void ReceiveXP(int xp)
    {
        SetTextVariationFeedBack(xp, Color.cyan, Color.red,"xp:",0,"xpbar");
        RamCharacterAttributes.GetInstance().currentXP += xp;
        int nextLvl=LvlXp.GetXPToLvl(RamCharacterAttributes.GetInstance().playerLevel);
        while (RamCharacterAttributes.GetInstance().currentXP >= nextLvl)//levelup
        {

            LevelUp();
            nextLvl = LvlXp.GetXPToLvl(RamCharacterAttributes.GetInstance().playerLevel);
        }
            UpdateCharacterUI();
    }
    public override void ReceiveGold(int gold)
    {
        if(gold!=0)
        {
            SetTextVariationFeedBack(gold, Color.yellow, Color.red, "$", -0.25f);
            RamCharacterAttributes.GetInstance().gold += gold;

            UpdateCharacterUI();
        }
        
    }
    private void LevelUp()
    {
        if (GameManager.Instance.MaxPlayerLevel > RamCharacterAttributes.GetInstance().playerLevel)
        {
            int nextLvl = LvlXp.GetXPToLvl(RamCharacterAttributes.GetInstance().playerLevel);
            characterClass.OnLevelUp();
            RamCharacterAttributes.GetInstance().currentXP -= nextLvl;
            RamCharacterAttributes.GetInstance().playerLevel++;
            FxSoundManager.Instance.PlayFx(10);

            life = GetMaxLife();
            //RamCharacterAttributes.GetInstance().AvailablePoints++;
            //RamCharacterAttributes.GetInstance().LeadershipPoints++;
            //statsButton.UpdatePointsText();
            particlesLvlUp.transform.position = transform.position;
            particlesLvlUp.Play();
            lvlUpIcon.Activate(transform.position);
            UpdateLifeBar();
        }
    }
    public override float GetWeaponAttackDistance()
    {
        return rightHand.attackRange;
    }
    public override int TakeHit(Human h,int damage)
    {
        int dmg=base.TakeHit(h,damage);
        UpdateCharacterUI();
        return dmg;
    }
    override protected void MovementInput()
    {
        if (Input.GetKeyDown(KeyCode.L)&&debug)
        {
            LevelUp();
        }
            if (Input.GetButton("Fire1") && !CancelButtonOnUI.IsOnUI())
        {
            //Debug.ClearDeveloperConsole();
            //Debug.Log("fire");
            

            // Raycast
            
           // if(!(blockMouse()))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 20f, LayerGround))
                {
                    if (LayerMask.NameToLayer("victim") == hit.collider.gameObject.layer )
                    {
                        bool dobleClick = false;
                        if (Input.GetButtonDown("Fire1"))
                        {
                            float dif = Time.fixedTime - lastClickTime;
                           
                            if (Target != null && dif <doubleClickTime&&dif>minDoubleClickTime&& hit.collider.gameObject == Target.GetTargetGameObject())
                            {
                                dobleClick = true;
                            }
                                lastClickTime = Time.fixedTime;
                            //Debug.Log(lastClickTime);
                        }
                        if (Target == null || hit.collider.gameObject != Target.GetTargetGameObject())
                        {
                            Target = hit.collider.gameObject.GetComponent<Human>();
                            foreach (var ally in allyList)
                            {
                                if (ally != this)
                                {
                                    ally.HelpWanted();
                                }
                            }
                            if (CastRayCastToPositionObstructed(Target.GetTargetGameObject().transform.position))
                            {
                                RenewPathToPosition(Target.GetTargetGameObject().transform.position);
                                stateMachine.currentState.ChangeState(typeof(GoThroughtPathTarget));
                            }
                            else
                            {

                                
                                stateMachine.currentState.ChangeState(typeof(GoToPositionTarget));
                            }
                        }
                        else if(dobleClick&&skill!=null&&skill.afectType==Skill.Afects.enemys&& skill.IsAvailable(this))
                        {
                            //Debug.Log("Power atemp");
                            ActivateSkillOnEnemy();
                            
                        }
                        
                    }
                    else if (LayerMask.NameToLayer("item") == hit.collider.gameObject.layer)
                    {
                        Target = hit.collider.gameObject.GetComponent<Item>();
                        //Destiny = hit.point.x * Vector3.right + Vector3.forward * hit.point.z;

                        if (CastRayCastToPositionObstructed(Target.GetTargetGameObject().transform.position))
                        {
                            RenewPathToPosition(Target.GetTargetGameObject().transform.position);
                            stateMachine.currentState.ChangeState(typeof(GoThroughtPathTarget));
                        }
                        else
                        {


                            stateMachine.currentState.ChangeState(typeof(GoToPositionTarget));
                        }
                    }
                    else
                    {
                        Vector3 tmp = hit.point.x * Vector3.right + Vector3.forward * hit.point.z;
                        if((tmp-CustomMath.Vector3NotNew(transform.position.x,0,transform.position.z)).sqrMagnitude>0.4f)
                        {
                            if (CastRayCastToPositionObstructed(tmp))
                            {
                                inputDestiny = tmp;
                                RenewPathToPosition(tmp);
                                stateMachine.currentState.ChangeState(typeof(GoThroughtPathPoint));
                            }
                            else
                            {
                                InputDestiny = tmp;
                                stateMachine.currentState.ChangeState(typeof(GoToPositionInput));
                            }
                            MarkPosition(true);
                        }
                        
                        //Debug.Log("gotoPoint " + InputDestiny);
                        
                        
                    }

                }
                else
                {
                    Debug.Log("didnt finde anything ... layer:");
                }
            }
            
            
           

        }
    }

    private void ActivateSkillOnEnemy()
    {
        stateMachine.currentState.ChangeState(typeof(GoToPositionTarget));
        if (stateMachine.currentState is GoToPositionTarget)
        {
            HumanState hs = (HumanState)(stateMachine.currentState);
            hs.OnSkillActivated();
            ActivateSkillUI(true);
        }
    }

    private Color red = new Vector4(1, 0, 0, 0.15f);
    private Color white = new Vector4(1, 1, 1, 0.15f);
    public override void ActivateSkillUI(bool b)
    {

        if (skill != null)
        {
            if (b)

            {
                skillImgHandler.background.color = red;
            }
            else
            {
                if (skillImgHandler.background.color == red)
                {
                    skillImgHandler.background.color = white;
                }

            }

        }
        
    }
    private bool CastRayCastToPositionObstructed(Vector3 position)
    {
        
        RaycastHit hit;
        Vector3 origin = transform.position.x * Vector3.right + transform.position.z * Vector3.forward + Vector3.up * Y_OFFSET;
        //Debug.Log("origin " + origin);
        //Debug.Log("destiny " + human.Destiny);
        Vector3 destiny = (position.x * Vector3.right + Y_OFFSET * Vector3.up + position.z * Vector3.forward);
            Debug.DrawRay(origin, destiny- origin, Color.green);
         if (Physics.Raycast(origin,
                destiny- (origin),
                out hit, 10f, mask))
            {
                //Debug.Log("hit wall at " + hit.point);
                if (IsWallCloseThanTarget(origin, hit.point,destiny))
                {
                return true;

                }
                else
                {
                return false;
                }
            }
            else
            {

            return true;
            }
        
    }
    public void CastItemConsumible()
    {
        if (Target != null)
        {
            cooldownList.Add(itemConsumible);
        ;
       
            LookAt(Target.GetHuman().transform.position);
        
        itemConsumible.OnExecute(Target, this);
        }

    }
    private bool IsWallCloseThanTarget(Vector3 origin, Vector3 wallPoint,Vector3 destiny)
    {
        return ((wallPoint - origin).sqrMagnitude <
            (destiny - origin).sqrMagnitude);
    }
    private bool BlockMouseDesktop()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
    private bool BlockMouseAndroid()
    {
        return EventSystem.current.IsPointerOverGameObject(1);
    }
    public override Type OnPointArrive()
    {
        return typeof(Idle);
    }
    public override bool CanConvert(Human victim)
    {
        return itemConsumible != null && itemConsumible.autoCasting && itemConsumible.IsAvailable(this) && itemConsumible.charm >= victim.stats.charmNeeded&&consumibleItems[(int)itemConsumible.type]>0&&RoomToConvert(this);
    }
    public override void OnCancelPersecution()
    {
        
        stateMachine.currentState.ChangeState(typeof(Idle));
    }
    protected override TextMesh SetTextVariationFeedBack(int num,Color positive, Color negative,string addText="",float yoffset=0,string gotoTag="")
    {
        TextMesh tm = base.SetTextVariationFeedBack(num,positive,negative,addText,yoffset,gotoTag);
        if(tm==null)
        {
            return null;
        }
        if(num<0)
        {
            tm.color = Color.magenta;
        }
        return tm;
    }
    
    protected override void Die(Human murderHuman)
    {

        ChangeState(typeof(Dead));
        GameManager.Instance.StateMachine.changeState(typeof(Lose));
    }
    override public int GetDamage()
    {
        return (int)((stats.damage+ RamCharacterAttributes.GetInstance().strengthPoint *AttributesValues.damageXStrength)*rightHand.damage);
    }
    override public int GetDefense()
    {
        return stats.defense + (int)( RamCharacterAttributes.GetInstance().agilityPoint * AttributesValues.defenseXAgility);
    }
    override public float GetCastTime()
    {
        return (stats.hitTime-(RamCharacterAttributes.GetInstance().agilityPoint * AttributesValues.casttimeXAgility))*rightHand.attackSpeed;
    }
    
    public override int GetMaxLife()
    {
        return base.GetMaxLife()+ RamCharacterAttributes.GetInstance().strengthPoint *AttributesValues.lifeXStrength+RamCharacterAttributes.GetInstance().playerLevel*AttributesValues.lifeXLevel;
    }
    public override int GetMaxAllys()
    {
        return (int)(RamCharacterAttributes.GetInstance().MaxAllyPoint*AttributesValues.maxAllysValue);
    }
    public void AddStrength(bool forced=false)
    {
        if(RamCharacterAttributes.GetInstance().AvailablePoints >0||forced)
        {
            RamCharacterAttributes.GetInstance().strengthPoint ++;
            if(!forced)
            {
                RamCharacterAttributes.GetInstance().AvailablePoints--;
            }
            ModifyLife(AttributesValues.lifeXStrength);
            //SetTextVariationFeedBack(1, Color.magenta, Color.white, "STRENGTH: ",-0.5f);
        }
       
    }
    public override int ModifyLife(int variationLife)
    {
        int aux=base.ModifyLife(variationLife);
        UpdateCharacterUI();
        return aux;
    }
    public void AddAgility(bool forced = false)
    {
        if (RamCharacterAttributes.GetInstance().AvailablePoints > 0 || forced)
        {
            RamCharacterAttributes.GetInstance().agilityPoint++;
            if (!forced)
            {
                RamCharacterAttributes.GetInstance().AvailablePoints--;
            }
            //SetTextVariationFeedBack(1, Color.yellow, Color.white, "AGILITY: ", -0.5f);
        }

    }
    
    private void UpdateMaxAllysUI()
    {
        for (int i = 0; i < allysUIImage.Length; i++)
        {

            if (i < RamCharacterAttributes.GetInstance().maxAllyPoint)
            {

                allysUI[i].SetActive(true);
            }
            else
            {
                allysUI[i].SetActive(false);
            }
        }
    }
    public void AddMaxAlly(bool forced = false)
    {
        if (RamCharacterAttributes.GetInstance().LeadershipPoints > 0 || forced)
        {
            RamCharacterAttributes.GetInstance().maxAllyPoint++;
            GameObject go = SetInteractionFeedBack("charmblack");
            if(go)
            {
                InteractionFeedback ifaux = go.GetComponent<InteractionFeedback>();
                // go.GetComponent<Material>().color=(Color.black);
                ifaux.SetObjectToGo(allysUIImage[RamCharacterAttributes.GetInstance().maxAllyPoint - 1].gameObject, UpdateMaxAllysUI);
            }
            
           
            //UpdateAllysUI();
            if (!forced)
            {
                RamCharacterAttributes.GetInstance().LeadershipPoints--;
            }
            //SetTextVariationFeedBack(1, Color.blue, Color.white, "Max Allys: ", -0.5f);
        }

    }
    public override void SetInitialPosition(Vector3 initialPos)
    {
        initialPosition = initialPos;
    }
    public override void MarkPosition(bool v)
    {
        mouseX.SetActive(v);
        mouseX.transform.position = InputDestiny.x*Vector3.right+Vector3.forward*InputDestiny.z+Vector3.up*0.1f;
    }
}
