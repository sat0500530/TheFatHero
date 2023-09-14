using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Normal,
    Cold,
    Fracture,
    Drowsy,
    Flu
}
public class PlayerStateController : MonoBehaviour
{
    public List<PlayerState> states = new List<PlayerState>();

    public void AddState(PlayerState newState)
    {
        states.Add(newState);
    }

    public void RemoveState(PlayerState stateToRemove)
    {
        if (states.Contains(stateToRemove))
        {
            states.Remove(stateToRemove);
        }
    }
}
