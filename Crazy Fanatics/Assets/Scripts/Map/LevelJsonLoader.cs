using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelJsonLoader : JsonLoader {
    float objectsRotation = 202.5f;
    public GameObject bound;
    public GameObject[] obstacles;
    public Human[] npcs;
    public Item[] items;
    public NodeCreator nodeCreator;
    protected override void ParseString(string tempString)
    {
        
        Level level = JsonUtility.FromJson<Level>(tempString);
        bound.transform.localScale = Vector3.right * level.bounds[0] + Vector3.forward + Vector3.up * level.bounds[1];
        //Vector3 initialPos = bound.transform.position;
        bound.transform.position+= Vector3.right * ((level.bounds[0]*0.5f)-0.5f)  + Vector3.forward * ((level.bounds[1]*0.5f)-0.5f);
        LoadObstacles(level.obstacles);
        LoadNodes(bound);
        LoadItems(level.items);
       
        LoadTextPrefab(level.textPrefab, level.textPrefabPosition);
        LoadPrefab(level.prefab, level.prefabPosition);
        LoadNpcs(level.npcs);
        GameManager.Instance.AddCastle();
        float[] cameraBounds = new float[4] { -0.5f, level.bounds[0]-0.5f, -0.5f, level.bounds[1]-0.5f };
        LoadCameraBounds(cameraBounds);
        LoadLevelObjetives(level.enemystokill,level.survivaltime);
        LoadMainCharacter(level.maincharinitialpos,level.maincharstartpos);
        LoadExit(level.exitposition);
        Invoke("LateStart", 1);
        GameManager.Instance.SetMaxPlayerLevel(level.maxPlayerLevel);
        
    }

    private void LoadTextPrefab(List<string> textPrefab, List<Vector3> textPrefabPosition)
    {
        for (int i = 0; i < textPrefab.Count; i++)
        {
            GameObject go=Instantiate(Resources.Load("textprefab/" + textPrefab[i])) as GameObject;
            go.transform.position = textPrefabPosition[i];
        }
    }
    private void LoadPrefab(List<string> prefab, List<Vector3> prefabPosition)
    {
        for (int i = 0; i < prefab.Count; i++)
        {
            GameObject go=Instantiate(Resources.Load("prefab/" + prefab[i])) as GameObject;
            go.transform.position = prefabPosition[i];
        }
    }

    void LateStart()
    {
       
        MainCharacter mainChar = GameManager.Instance.MainCharacter;
        foreach (var item in RamCharacterAttributes.GetInstance().allys)
        {
            //Debug.Log("npc id " + item);
            Human go = Instantiate<Human>(npcs[item]);
            go.SetInitialPosition(mainChar.transform.position);
            CombatIA cAI = go.GetComponent<CombatIA>();
            GameManager.Instance.combatIAList.Add(cAI);
            cAI.Convert(mainChar.stats, mainChar, false);
        }
        GameManager.Instance.MainCharacter.UpdateAllysUI();
        Destroy(this.gameObject);
    }
    private void LoadMainCharacter(Vector3 pos,Vector3 startpos)
    {
        MainCharacter mainChar = FindObjectOfType<MainCharacter>();
        mainChar.SetInitialPosition(pos);
        mainChar.transform.position = startpos;
        mainChar.ChangeState(typeof(CharacterIntro));
        
    }
    private void LoadExit(Vector3 pos)
    {
        GameObject.FindGameObjectWithTag("exit").transform.position = pos;
    }

    private void LoadLevelObjetives(List<Vector2> enemystokill,float survivalTime)
    {
        FindObjectOfType<WinningManager>().AddEnemyToKill(enemystokill);
        if(survivalTime>0)
        {
            GameManager.Instance.ActivateSurvivalTime(survivalTime);
        }
    }

    private void LoadCameraBounds(float[] camerabounds)
    {
        Camera.main.gameObject.GetComponent<InputHandler>().camerabounds=camerabounds;
    }

    private void LoadObstacles(int[] obstaclesInfo)
    {
        for (int i = 0; i < obstaclesInfo.Length; i+=3)
        {
            GameObject go= Instantiate<GameObject>(obstacles[obstaclesInfo[i + 2]]);
            go.transform.position = Vector3.right * obstaclesInfo[i] + Vector3.forward * obstaclesInfo[i + 1];
           
            foreach (Transform t in go.transform)
            {
                t.rotation = Quaternion.Euler(Vector3.up * (((go.transform.position.x * go.transform.position.z) * objectsRotation) % 360));
            }
            //Debug.Log(obstacles[i]+" "+ obstacles[i+1] +" "+ obstacles[i+2] );
        }
    }
    private void LoadItems(float[] itemsInfo)
    {
        for (int i = 0; i < itemsInfo.Length; i += 3)
        {
            Item go = Instantiate<Item>(items[(int)(itemsInfo[i + 2])]);
            go.transform.position = Vector3.right * itemsInfo[i] + Vector3.forward * itemsInfo[i + 1];
            //Debug.Log(obstacles[i]+" "+ obstacles[i+1] +" "+ obstacles[i+2] );
        }
    }
    private void LoadNpcs(float[] npcsInfo)
    {
        for (int i = 0; i < npcsInfo.Length; i += 3)
        {
            Human go = Instantiate<Human>( npcs[Mathf.FloorToInt(npcsInfo[i + 2])]);
            go.SetInitialPosition( Vector3.right * npcsInfo[i] + Vector3.forward * npcsInfo[i + 1]);
            CombatIA cAI = go.GetComponent<CombatIA>();
            if(cAI!=null)
            {
                GameManager.Instance.combatIAList.Add(cAI);
            }
            
            //Debug.Log(npcsInfo[i]+" "+ npcsInfo[i+1] +" "+ npcsInfo[i+2] );
        }
    }
    private void LoadNodes(GameObject bound)
    {
       
        nodeCreator.CreateNodes(bound);
    }
    //private Vector3 GetPosition(float p)
    //{

    //}
}
