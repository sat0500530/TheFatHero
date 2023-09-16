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
    public int count; // �� ������ count �߰�
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
            existingState.count = 3; // ���÷� count�� 3���� ����
        }
        else
        {
            // ���ο� ���� ������ �߰�
            states.Add(new StateInfo
            {
                state = newState,
                stateName = name,
                stateDescription = description,
                count = 3 // ���÷� count�� 3���� ����

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

    // �� �Լ��� PlayerState�� ���� ���� �̸��� ��ȯ�մϴ�.
    public string GetStateName(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Cold:
                return "����";
            case PlayerState.Fracture:
                return "����";
            case PlayerState.Drowsy:
                return "�Ƿ�";
            default:
                return "�� �� ����";
        }
    }

    // �� �Լ��� PlayerState�� ���� ���� ������ ��ȯ�մϴ�.
    public string GetStateDescription(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Cold:
                return "���⿡ �ɷ� ���� ��������ϴ�. ����� �Ŀ��� 1 �������ϴ�.";
            case PlayerState.Fracture:
                return "�ٸ��� �η������ϴ�. ����� �ִ� �ൿ���� 1 �������ϴ�.";
            case PlayerState.Drowsy:
                return "�Ƿη� ���� �����ϴ�. ����� ��ø�� 50% �����մϴ�.";
            default:
                return "�� �� ���� �����Դϴ�.";
        }
    }

    // ���� �Ŵ������� ȣ���Ͽ� ������ count�� ���ҽ�Ű�� �Լ�
    public void DecreaseStateCount(int amount)
    {
        // states �÷����� ���纻�� ����ϴ�.
        List<StateInfo> stateCopy = new List<StateInfo>(states);

        foreach (StateInfo stateInfo in stateCopy)
        {
            stateInfo.count -= amount;

            // ī��Ʈ�� 0�� �Ǹ� ���¸� ����
            if (stateInfo.count <= 0)
            {
                RemoveState(stateInfo.state);
            }
        }

        UIManager.Instance.UpdateStateUI();
    }
}