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

[System.Serializable]
public class StateInfo
{
    public PlayerState state;
    public string stateName;
    public string stateDescription;
}

public class PlayerStateController : MonoBehaviour
{
    private GameManager _gameManager;
    public List<StateInfo> states = new List<StateInfo>();

    public void Awake()
    {
        _gameManager = GameObject.Find(nameof(GameManager)).GetComponent<GameManager>();
    }
    public void AddState(PlayerState newState, string name, string description)
    {
        // 이미 리스트에 있는 상태인지 확인
        StateInfo existingState = states.Find(stateInfo => stateInfo.state == newState);

        if (existingState != null)
        {
            Debug.Log("이미 있다능");
            // 이미 있는 상태 정보를 업데이트
            existingState.stateName = name;
            existingState.stateDescription = description;
        }
        else
        {
            // 새로운 상태 정보를 추가
            states.Add(new StateInfo
            {
                state = newState,
                stateName = name,
                stateDescription = description
            });
            AddState(newState);
        }


        UIManager.Instance.UpdateStateUI();
    }

    public void RemoveState(PlayerState stateToRemove)
    {
        StateInfo stateInfoToRemove = states.Find(stateInfo => stateInfo.state == stateToRemove);

        if (stateInfoToRemove != null)
        {
            states.Remove(stateInfoToRemove);
        }
        UIManager.Instance.UpdateStateUI();
    }

    // 이 함수는 PlayerState에 따라 상태 이름을 반환합니다.
    public string GetStateName(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Normal:
                return "정상";
            case PlayerState.Cold:
                return "감기";
            case PlayerState.Fracture:
                return "골절";
            case PlayerState.Drowsy:
                return "졸림";
            case PlayerState.Flu:
                return "독감";
            default:
                return "알 수 없음";
        }
    }

    // 이 함수는 PlayerState에 따라 상태 설명을 반환합니다.
    public string GetStateDescription(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Normal:
                return "상태가 정상입니다.";
            case PlayerState.Cold:
                return "추위로 인해 감기에 걸렸습니다.";
            case PlayerState.Fracture:
                return "골절된 부상이 있습니다.";
            case PlayerState.Drowsy:
                return "피로로 인해 졸립니다.";
            case PlayerState.Flu:
                return "독감 증상이 있습니다.";
            default:
                return "알 수 없는 상태입니다.";
        }
    }

    void AddState(PlayerState state)
    {


        switch (state)
        {
            case PlayerState.Normal:
                _gameManager.knight.Status.IsCold = true;
                break;
            case PlayerState.Cold:
                _gameManager.knight.Status.IsCold = true;
                break;
            case PlayerState.Fracture:
                _gameManager.KnightMaxCost -= 1;
                break;
            case PlayerState.Drowsy:
                _gameManager.KnightMaxCost -= 1;
                break;
            case PlayerState.Flu:
                _gameManager.KnightMaxCost -= 1;
                break;
        }
    }

}