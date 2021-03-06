﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    bool interacted;
    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && !interacted)
        {
            GetComponent<Animator>().SetTrigger("save");
            interacted = true;
        }
        
    }

    public void SaveState()
    {
        SaveSystem.instance.SaveState();
        SaveSystem.instance.SaveCheckpoint(transform.position);
    }

    public void DisableCheckpoint()
    {
        gameObject.SetActive(false);
    }
}
