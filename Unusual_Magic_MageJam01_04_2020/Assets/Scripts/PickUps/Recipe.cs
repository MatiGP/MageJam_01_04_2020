﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Recipe", menuName = "New Recipe")]
public class Recipe : ScriptableObject
{
    public Sprite recipeIcon;
    public Ingredient[] ingredients = new Ingredient[3];
    public GameObject result;
}
[System.Serializable]
public class Ingredient
{
    public Spice requiredSpice;
    public int requiredAmount;
}
