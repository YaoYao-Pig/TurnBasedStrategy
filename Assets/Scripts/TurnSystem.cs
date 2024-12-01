using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    private int turnNumber;
    private static TurnSystem _instance;

    public event EventHandler OnTurnChanged;
    public static TurnSystem Instance
    {
        get
        {
            return _instance;
        }
        private set
        {
            ;
        }
    }

    private bool isPlayerTurn = true;
    private void Awake()
    {
        if (_instance == null) _instance = this;
        else
        {
            Destroy(gameObject);
        }
    }
    public void NextTurn()
    {
        turnNumber++;
        isPlayerTurn = !isPlayerTurn;
        OnTurnChanged?.Invoke(this,EventArgs.Empty);
    }

    public int GetTurnNumber()
    {
        return turnNumber;
    }

    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }

}
