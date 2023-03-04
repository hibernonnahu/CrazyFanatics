using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new inventory item")]
public class InventoryItem : ScriptableObject
{
    public Sprite icon;
    public GameObject floorItem;
    public string weaponName="";
    [Range(1,2)]
    public float damage=1;
    [Range(0, 2)]
    public float attackSpeed = 1;
    [Range(0, 2)]
    public float defense = 1;
    [Range(0, 5)]
    public int extraAlly = 0;
    [Range(0, 3)]
    public float attackRange = 0;
    public enum TypeOfWeapon
    {
        nothing,
        dagger,
        sword,
        axe
        
    }
    public TypeOfWeapon typeOfWeapon;
    public enum Slot
    {
        righthand,
        lefthand,
        neck
    }
    public Slot slot;
}
