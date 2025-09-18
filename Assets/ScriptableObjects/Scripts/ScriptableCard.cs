using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum FunctionType
{
    Shielding,
    Damaging,
    Drawing,
    Energizing
};
public enum Rarity
{ 
    Common,
    Uncommon,
    Rare,
    SpecialRare
};

[System.Serializable]
public struct CardFunction
{
    public FunctionType Effect;
    public int Min, Max;
}

[CreateAssetMenu(fileName = "ScriptableCard", menuName = "ScriptableObjects/ScriptableCard")]
public class ScriptableCard : ScriptableObject
{

    public string Name;
    public Sprite[] Sprites;
    public Rarity Rarity;
    public int EnergyCost;

    public List<CardFunction> Functions = new();
}
