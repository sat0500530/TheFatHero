using UnityEngine;

public class Status
{
    public int Level { get; set; } = 1;
    
    private bool player;
    private int maxHp;
    private int currentHp;
    private int power;
    private int dex;
    private int exp = 0;

    private bool _buff;

    public bool Buff
    {
        get => _buff;
        set
        {
            _buff = value;
            if (player) UIManager.Instance.UpdateKnightStatusInfo();
        }
    }
    public int MaxHp
    {
        get { return maxHp; }
        set
        {
            maxHp = Mathf.Max(value, 0);
            // Ensure CurrentHp doesn't exceed MaxHp
            currentHp = Mathf.Min(currentHp, maxHp);
            if (player) UIManager.Instance.UpdateKnightStatusInfo();
        }
    }

    public int CurrentHp
    {
        get { return currentHp; }
        set
        {
            if (player && value < currentHp)
            {
                UIManager.Instance.BloodEffect();
            }
            // Ensure CurrentHp is between 0 and MaxHp
            currentHp = Mathf.Clamp(value, 0, maxHp);
            if (player)
            {
                UIManager.Instance.UpdateKnightStatusInfo();
                if (currentHp == 0)
                {
                    UIManager.Instance.ActiveGameOverObj();
                }
            }
        }
    }

    public int Power
    {
        get
        {
            return power + (Buff ? 2 + DataManager.Instance.PrincessPowerSkillValue : 0);
        }
        set
        {
            power = Mathf.Max(value, 0);
            if (player) UIManager.Instance.UpdateKnightStatusInfo();
        }
    }


    public int Dex
    {
        get => dex;
        set
        {
            dex = Mathf.Max(value, 0);
            if (player) UIManager.Instance.UpdateKnightStatusInfo();
        }
    }

    public int Exp
    {
        get
        {
            return exp;
        }
        set
        {
            exp = Mathf.Max(value, 0);
            if (player) UIManager.Instance.UpdateKnightStatusInfo();
        }
    }

    public Status()
    {

    }

    public Status(int maxHp, int power, int dex, int exp, bool isPlayer = false)
    {
        player = isPlayer;

        MaxHp = maxHp;
        Exp = exp;
        currentHp = maxHp;
        Power = power;
        Dex = dex;

    }
}
