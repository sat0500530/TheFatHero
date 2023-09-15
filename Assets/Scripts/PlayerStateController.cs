using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
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

    // ���� �Ŵ������� ȣ���Ͽ� ������ count�� ���ҽ�Ű�� �Լ�
    public void DecreaseStateCount()
    {
        // states �÷����� ���纻�� ����ϴ�.
        List<StateInfo> stateCopy = new List<StateInfo>(states);

        foreach (StateInfo stateInfo in stateCopy)
        {
            stateInfo.count--;

            // ī��Ʈ�� 0�� �Ǹ� ���¸� ����
            if (stateInfo.count <= 0)
            {
                RemoveState(stateInfo.state);
            }
        }

        UIManager.Instance.UpdateStateUI();
    }
}