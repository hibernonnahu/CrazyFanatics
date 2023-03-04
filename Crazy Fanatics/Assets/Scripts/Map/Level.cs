using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level  {
    public float survivaltime=-1;
    public List<Vector2> enemystokill;
    public Vector3 maincharstartpos;
    public Vector3 maincharinitialpos;
    public int[] bounds;
    public int[] obstacles;
    public float[] items;
    public float[] npcs;
    public Vector3 exitposition;
    public int maxPlayerLevel;
    public List<string> textPrefab;
    public List<Vector3> textPrefabPosition;
    public List<string> prefab;
    public List<Vector3> prefabPosition;

}
