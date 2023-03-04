using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new stats")]
public class Stats :ScriptableObject  {
    public int id=-1;
    public int rewardXP = 0;
    public int rewardCoin = 0;
    public ParticleSystem onHitEffect;
    public GameObject rewardInventoryItem;
    [Range(0,1)]
    public float rewardInventoryItemChance=0;
    public float Radius = 5f;
    public float AttackDistance = 0f;
    public float height = 1.5f;
    public float scale = 1;
    public int attackSoundCode=0;
    public int[] humanSkinA;
    public GameObject skinA;
    public int[] humanSkinB;

    public GameObject skinB;
    public int maxlife = 100;
    public int damage = 7;
    public int defense = 1;
    public int charmNeeded = 999;
    
    public float hitTime = 1f;
    public float speed = 1f;
    [Header("IA")]
    public float protectFarSpeed = 1.2f;
    public float protectionSpeed = 1f;
    public float protectionVisionRange = 4f;
    public float persecutionRange = 5f;
    public float patrolWalkRange = 2f;
    public float protectPatrolWalkRange = 2f;
    public enum DecitionType
    {
        none,
        input,
        attack
    }
    [SerializeField]
    public DecitionType decitionType;
    public enum OnRestType
    {
       
        patrol,
        none,
        attackMain,
        castSpellAoE,
        healerBuffer,
        attackCastle
    }
    [SerializeField]
    public OnRestType onRestType;
}
