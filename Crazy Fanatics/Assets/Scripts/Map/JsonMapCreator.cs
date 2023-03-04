using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System;

public class JsonMapCreator : MonoBehaviour {
    public int mapName = 0;
    public int maxPlayerLvl = 100;
    JsonData mapJson;
    Level lvl;
    
    // Use this for initialization
    private void Awake()
    {
        //PREPARE MAP
        lvl = new Level();
        
        FindObstacles();
        FindPrefab();
        FindHumans();
        FindMainChar();
        FindExit();
        FindIntro();
        FindItems();
        FindTextPrefab();
        
        lvl.maxPlayerLevel = maxPlayerLvl;
        LoadLevelObjectives();
        
    }

    private void FindTextPrefab()
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("textprefab");
        List<string> name = new List<string>();
        List<Vector3> pos = new List<Vector3>();
        foreach (var item in go)
        {
            name.Add(item.name);
            pos.Add(AdjustPostionX(item.transform.position.x) * Vector3.right+ AdjustPostionZ(item.transform.position.z) * Vector3.forward+Vector3.up*item.transform.position.y);
        }
        lvl.textPrefab = name;
        lvl.textPrefabPosition = pos;
    }
    private void FindPrefab()
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("prefab");
        List<string> name = new List<string>();
        List<Vector3> pos = new List<Vector3>();
        foreach (var item in go)
        {
            name.Add(item.name);
            pos.Add(AdjustPostionX(item.transform.position.x) * Vector3.right + AdjustPostionZ(item.transform.position.z) * Vector3.forward + Vector3.up * item.transform.position.y);
            Destroy(item);
        }
        lvl.prefab = name;
        lvl.prefabPosition = pos;
    }

    private void FindMainChar()
    {
        GameObject mainCharacter=GameObject.FindGameObjectWithTag("maincharacter");
        lvl.maincharinitialpos = (AdjustPostionX(mainCharacter.transform.position.x)-1) * Vector3.right + AdjustPostionZ(mainCharacter.transform.position.z) * Vector3.forward;
    }
    private void FindExit()
    {
        Exit exit = FindObjectOfType<Exit>();
        lvl.exitposition = (AdjustPostionX(exit.transform.position.x)-1) * Vector3.right + AdjustPostionZ(exit.transform.position.z) * Vector3.forward;
    }
    private void FindIntro()
    {
        GameObject intro = GameObject.FindGameObjectWithTag("intro");
        if (intro != null)
        {
            lvl.maincharstartpos = (AdjustPostionX(intro.transform.position.x) - 1) * Vector3.right + AdjustPostionZ(intro.transform.position.z) * Vector3.forward;
        }
        else
        {
            GameObject mainCharacter = GameObject.FindGameObjectWithTag("maincharacter");
            lvl.maincharstartpos = (AdjustPostionX(mainCharacter.transform.position.x) - 1) * Vector3.right + AdjustPostionZ(mainCharacter.transform.position.z) * Vector3.forward;
        }
    }
    private void LoadLevelObjectives()
    {
        LevelObjetives lo = FindObjectOfType<LevelObjetives>();
        lvl.enemystokill = lo.enemysToKill;
        if(lo.survivalTime>0)
        {
            lvl.survivaltime = lo.survivalTime;
        }
    }
    private void FindItems()
    {
        Item[] items = FindObjectsOfType<Item>();
        List<float> auxList = new List<float>();
        GameObject bound = GameObject.FindGameObjectWithTag("bounds");
        lvl.bounds = new int[] { (int)(bound.transform.localScale.x), (int)(bound.transform.localScale.y) };
        foreach (var item in items)
        {
            if (item.id >= 0)
            {

                auxList.Add(AdjustPostionX(item.transform.position.x ));
                auxList.Add(AdjustPostionZ(item.transform.position.z ));
                auxList.Add((item.id));
            }
        }
        lvl.items = auxList.ToArray();
    }
    private void FindObstacles()
    {
        Obstacle[] obstacles= FindObjectsOfType<Obstacle>();
        List<int> auxList = new List<int>();
        GameObject bound = GameObject.FindGameObjectWithTag("bounds");
        lvl.bounds = new int[] { (int)(bound.transform.localScale.x), (int)(bound.transform.localScale.y) };
        foreach (var obstacle in obstacles)
        {
            if(obstacle.id>=0)
            {
            
                auxList.Add(AdjustPostionX(obstacle.transform.position.x));
                auxList.Add(AdjustPostionZ(obstacle.transform.position.z));
                auxList.Add((obstacle.id));
            }
        }
        lvl.obstacles = auxList.ToArray();
    }
    private void FindHumans()
    {
        Human[] enemys = FindObjectsOfType<Human>();
        List<float> auxList = new List<float>();
        
        foreach (var enemy in enemys)
        {
            if (enemy.id >= 0)
            {

                auxList.Add(AdjustPostionX(enemy.transform.position.x ));
                auxList.Add(AdjustPostionZ(enemy.transform.position.z ));
                auxList.Add((enemy.id));
            }
        }
        lvl.npcs = auxList.ToArray();
    }
    void Start () {
        mapJson= JsonMapper.ToJson(lvl);
        File.WriteAllText(Application.dataPath + "/Resources/level/level-" + mapName.ToString()+".json", mapJson.ToString());
	}
	private int AdjustPostionX(float pos)
    {
        return (int)(pos + (lvl.bounds[0] * 0.5f) - (0.5f * (lvl.bounds[0] % 2)));
    }
    private int AdjustPostionZ(float pos)
    {
        return (int)(pos + (lvl.bounds[1] * 0.5f) - (0.5f * (lvl.bounds[1] % 2)));
    }

}
