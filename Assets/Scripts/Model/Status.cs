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
    private int hunger = 50;

    private bool _buff;
    private bool _isCold;
    private bool _isFull;
    private bool _isDrowsy;

    private int _levelPow = 0;
    private int _levelDex = 0;


    public int levelPow
    {
        get => _levelPow;
        set
        {
            _levelPow = value;
            if (player) UIManager.Instance.UpdateKnightStatusInfo();
        }

    }
    public int levelDex
    {
        get => _levelDex;
        set
        {
            _levelDex = value;
            if (player) UIManager.Instance.UpdateKnightStatusInfo();
        }

    }

    public bool IsFull
    {
        get => _isFull;
        set
        {
            _isFull = value;
            if (player) UIManager.Instance.UpdateKnightStatusInfo();
        }
    }

    public bool IsDrowsy
    {
        get => _isDrowsy;
        set
        {
            _isDrowsy = value;
            if (player) UIManager.Instance.UpdateKnightStatusInfo();
        }
    }

    public int Hunger
    {
        get { return hunger;  }
        set
        {
            hunger = Mathf.Clamp(value, 0, 100);
        }
    }

    public bool Buff
    {
        get => _buff;
        set
        {
            _buff = value;
            if (player) UIManager.Instance.UpdateKnightStatusInfo();
        }
    }

    public bool IsCold
    {
        get => _isCold;
        set
        {
            _isCold = value;
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
            return Mathf.Max(power + levelPow + (IsFull ? 2 : 0) + (Buff ? 2 + DataManager.Instance.PrincessPowerSkillValue : 0) - (IsCold ? 1 : 0),1);
        }
        set
        {
            power = Mathf.Max(value , 1);
            if (player) UIManager.Instance.UpdateKnightStatusInfo();
        }
    }


    public int Dex
    {
        get { return Mathf.Clamp((dex + levelDex) / (IsDrowsy ? 2 : 1), 0, 90); }
        set
        {
            dex = Mathf.Clamp(value, 0, 90);
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
