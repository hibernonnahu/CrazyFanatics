using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct PoolData
{
    public string name;
    public Pool pool;
}
public class GameManager : MonoBehaviour {
    public Transform vault;
    public Renderer fade;
    public float barTime=3f;

    Exit exit;
    TMPro.TextMeshProUGUI mainText;
    public Action timeMethod=()=> { };
     float counter = 0;
    [SerializeField]
    public PoolData[] poolSerializableDictionary;
    private Dictionary<string, Pool> poolDictionary;
    public Dictionary<string,Pool> GetPoolDictionary()
    {
        return poolDictionary;
    }
    public List<Human> combatIAList=new List<Human>();
    public List<Human> allyList = new List<Human>();

    private StateMachine stateMachine;

    public void DestroyAll()
    {
        
        Time.timeScale = 1;
        Astar.instance = null;
        instance = null;
        SoundManager.Instance = null;
        GameObject[] objets = FindObjectsOfType<GameObject>();
        foreach (var item in objets)
        {
            Destroy(item);
        }
    }

    public void OnWin()
    {
        if (exit == null)
        {
            StateMachine.changeState(typeof(Win));
        }
        else
        {
            exit.Open();
        }
    }

    private static GameManager instance;
    private int maxPlayerLevel;
    public int MaxPlayerLevel
    {
        get
        {
            return maxPlayerLevel;
        }
    }
    internal void SetMaxPlayerLevel(int maxPlayerLevel)
    {
        this.maxPlayerLevel = maxPlayerLevel;
    }

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    public StateMachine StateMachine
    {
        get
        {
            return stateMachine;
        }

       
    }
    private MainCharacter mainCharacter;
    public MainCharacter MainCharacter
    {
        get
        {
            return mainCharacter;
        }
    }
    private Human castle;
   
    public Human Castle
    {
        get
        {
            return castle;
        }
    }
    void Awake () {
        instance = this;
        stateMachine = new StateMachine();
        mainCharacter = FindObjectOfType<MainCharacter>();
        //Vector3 v3=new Vector3(66,0,44);
        //Debug.Log("inverse sqr "+CustomMath.XZNormalize(v3));
        //Debug.Log("normalized " + v3.normalized);

    }

    public void ActivateSurvivalTime(float survivalTime)
    {
        timeMethod = Survival;
        counter = survivalTime;
    }
    private void Survival()
    {
        counter -= Time.deltaTime;
        if(counter<0)
        {

            timeMethod = () => { };
            mainText.text = "Clear";
            castle.GetComponentInChildren<ParticleSystem>().Play();
            FxSoundManager.Instance.PlayFx(12);

            for (int i = 0; i < combatIAList.Count; i++)
            {
                combatIAList[i].Kill();
            }
            OnWin();
        }
        else
        {
            mainText.text = "Protect the cauldron: "+Math.Floor(counter).ToString()+"''";
        }
    }
    private void Start()
    {
        
        
        LoadDictionarys();
        stateMachine.addState(new Play(this, stateMachine));
        stateMachine.addState(new Win(this, stateMachine));
        stateMachine.addState(new Lose(this, stateMachine));
        stateMachine.addState(new LoseCastle(this, stateMachine));

        GameObject go = GameObject.FindGameObjectWithTag("maintext");
        if (go != null)
        {
            mainText = go.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        }
        exit = FindObjectOfType<Exit>();
    }
    public void AddCastle()
    {
        GameObject go = GameObject.FindGameObjectWithTag("castle");
        if(go!=null)
        {
            castle = go.GetComponent<Human>();

        }

    }
   

    // Update is called once per frame
    void Update () {
        stateMachine.Update();
    }
    private void LoadDictionarys()
    {
        poolDictionary = new Dictionary<string, Pool>();
        for (int i = 0; i < poolSerializableDictionary.Length; i++)
        {
            poolDictionary.Add(poolSerializableDictionary[i].name, poolSerializableDictionary[i].pool);
        }
        
    }
    
    
}
