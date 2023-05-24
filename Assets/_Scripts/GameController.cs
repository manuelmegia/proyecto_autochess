using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    public void changeS()
    {
        GameManager.Instance.ChangeState(GameState.PreparationRound);
        Debug.Log ("You have clicked the button!");
    }
    public void changeF()
    {
        GameManager.Instance.ChangeState(GameState.FightState);
        Debug.Log ("You have clicked the button!");
    }
}
