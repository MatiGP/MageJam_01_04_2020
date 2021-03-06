﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerPoints : MonoBehaviour
{
    public static PlayerPoints instance;
    public event EventHandler<OnPointsAddedArgs> OnPointsAdded;
    public class OnPointsAddedArgs : EventArgs
    {
        public int points;
    }
    [SerializeField] int points;


    private void Awake()
    {
        instance = this;
    }

    public void AddPoints(int pointsToAdd)
    {
        points += pointsToAdd;
        OnPointsAdded?.Invoke(this, new OnPointsAddedArgs() {points = points});
    }

    public int GetPoints()
    {
        return points;
    }

    public void ResetPoints()
    {
        points = 0;
    }
}
