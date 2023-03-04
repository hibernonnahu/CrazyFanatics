using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGenders : MonoBehaviour
{
    public void SetCharacterGender(int g) {
        AttributesValues.characterGender = g;
    }
    public void SetEnemiesGender(int g)
    {
        AttributesValues.enemyGender = g;

    }
}
