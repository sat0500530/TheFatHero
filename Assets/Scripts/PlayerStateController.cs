using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Cold,
    Fracture,
    Drowsy
}

[System.Serializable]
public class StateInfo
{
    public PlayerState state;
    public string stateName;
    public string stateDescription;
    public int count; // 각 상태의 count 추가
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
            existingState.count = 3; // 예시로 count를 3으로 설정
        }
        else
        {
            // 새로운 상태 정보를 추가
            states.Add(new StateInfo
            {
                state = newState,
                stateName = name,
                stateDescription = description,
                count = 3 // 예시로 count를 3으로 설정

            });

            switch (newState)
            {
                case PlayerState.Cold :
                    _gameManager.knight.Status.IsCold = true;
                    break;
                case PlayerState.Fracture:
                    _gameManager.KnightMaxCost -= 1;
                    break;
                case PlayerState.Drowsy:
                    _gameManager.knight.Status.IsDrowsy = true;
                    break;
            }
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

        switch (stateToRemove)
        {
            case PlayerState.Cold:
                _gameManager.knight.Status.IsCold = false;
                break;
            case PlayerState.Fracture:
                _gameManager.KnightMaxCost += 1;
                break;
            case PlayerState.Drowsy:
                _gameManager.knight.Status.IsDrowsy = false;
                break;
        }
        UIManager.Instance.UpdateStateUI();
    }

    // 이 함수는 PlayerState에 따라 상태 이름을 반환합니다.
    public string GetStateName(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Cold:
                return "감기";
            case PlayerState.Fracture:
                return "골절";
            case PlayerState.Drowsy:
                return "피로";
            default:
                return "알 수 없음";
        }
    }

    // 이 함수는 PlayerState에 따라 상태 설명을 반환합니다.
    public string GetStateDescription(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Cold:
                return "감기에 걸려 몸이 허약해집니다. 기사의 파워가 1 낮아집니다.";
            case PlayerState.Fracture:
                return "다리가 부러졌습니다. 기사의 최대 행동력이 1 낮아집니다.";
            case PlayerState.Drowsy:
                return "피로로 인해 졸립니다. 기사의 민첩이 50% 감소합니다.";
            default:
                return "알 수 없는 상태입니다.";
        }
    }

    // 게임 매니저에서 호출하여 상태의 count를 감소시키는 함수
    public void DecreaseStateCount(int amount)
    {
        // states 컬렉션의 복사본을 만듭니다.
        List<StateInfo> stateCopy = new List<StateInfo>(states);

        foreach (StateInfo stateInfo in stateCopy)
        {
            stateInfo.count -= amount;

            // 카운트가 0이 되면 상태를 제거
            if (stateInfo.count <= 0)
            {
                RemoveState(stateInfo.state);
            }
        }

        UIManager.Instance.UpdateStateUI();
    }
}