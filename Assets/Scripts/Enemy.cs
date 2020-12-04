using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Handles interaction with the Enemy */

[RequireComponent(typeof(CharacterStat))]
public class Enemy : Interactable
{
    PlayerManager playerManager;
    CharacterStat myStats;

    void Start(){
        playerManager = PlayerManager.instance;
        myStats = GetComponent<CharacterStat>();
    }

    public override void Interact(){
        base.Interact();

        // Attack the enemy
        CharacterCombat playerCombat = playerManager.player.GetComponent<CharacterCombat>();

        if(playerCombat != null){
            playerCombat.Attack(myStats);
        }
    }
}
