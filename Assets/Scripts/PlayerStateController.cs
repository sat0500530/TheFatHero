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
        // �̹� ����Ʈ�� �ִ� �������� Ȯ��
        StateInfo existingState = states.Find(stateInfo => stateInfo.state == newState);

        if (existingState != null)
        {
            Debug.Log("�̹� �ִٴ�");
            // �̹� �ִ� ���� ������ ������Ʈ
            existingState.stateName = name;
            existingState.stateDescription = description;
        }
        else
        {
            // ���ο� ���� ������ �߰�
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

    // �� �Լ��� PlayerState�� ���� ���� �̸��� ��ȯ�մϴ�.
    public string GetStateName(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Normal:
                return "����";
            case PlayerState.Cold:
                return "����";
            case PlayerState.Fracture:
                return "����";
            case PlayerState.Drowsy:
                return "����";
            case PlayerState.Flu:
                return "����";
            default:
                return "�� �� ����";
        }
    }

    // �� �Լ��� PlayerState�� ���� ���� ������ ��ȯ�մϴ�.
    public string GetStateDescription(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Normal:
                return "���°� �����Դϴ�.";
            case PlayerState.Cold:
                return "������ ���� ���⿡ �ɷȽ��ϴ�.";
            case PlayerState.Fracture:
                return "������ �λ��� �ֽ��ϴ�.";
            case PlayerState.Drowsy:
                return "�Ƿη� ���� �����ϴ�.";
            case PlayerState.Flu:
                return "���� ������ �ֽ��ϴ�.";
            default:
                return "�� �� ���� �����Դϴ�.";
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