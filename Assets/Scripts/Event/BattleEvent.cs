using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using System;
using System.Linq;
using Unity.VisualScripting;

public class BattleEvent : MonoBehaviour
{
    private GameManager _gameManager;
    private UIManager _uiManager;
    private Player _knight;
    private Monster _monster;

    public Image monsterImg;
    private bool _isLastBoss = false;

    private int _attDefCount;

    [Header("CombatPanelUI")]
    private GameObject _combatPanel;
    public TextMeshProUGUI combatText;
    public TextMeshProUGUI monsterInf;

    private int maxLines = 5;
    private int lineCount = 0;
    public GameObject scrollbarVertical;

    public GameObject combatPanelExitButton;
    public GameObject gameOverButton;



    public void Init(Player knight, Monster monster)
    {
        _combatPanel ??= gameObject;
        _gameManager ??= GameObject.Find(nameof(GameManager)).GetComponent<GameManager>(); 
        _uiManager ??= GameObject.Find(nameof(UIManager)).GetComponent<UIManager>();

        _knight = knight;
        _monster = monster;
        monsterImg.sprite = _monster.Sprite;
        _attDefCount = 0;
        ClearCombatText();
    }

    public void Execute(bool isLastBoss = false)
    {
        _isLastBoss = isLastBoss;

        combatText.text = string.Empty;
        _combatPanel.SetActive(true);
        combatPanelExitButton.SetActive(false);

        UpdateMonsterInfoText();

        StartCoroutine(Battle());
    }

    bool Dodges(int unitDex)
    {
        System.Random random = new();

        int randomNumber = random.Next(1, 101);

        return randomNumber >= unitDex;
    }

    IEnumerator Battle()
    {
        bool knightTurn = true;
        bool monsterTurn = false;

        if (_knight.Status.Dex <= _monster.Status.Dex)
        {
            knightTurn = false;
            monsterTurn = true;
        }

        int playerDam = Mathf.Max(_knight.Status.Power, 1);
        int monsterDam = Mathf.Max(_monster.Status.Power, 1);

        //if ((playerDam == monsterDam) && playerDam == 0)
        //{
        //    playerDam = monsterDam = 1;
        //}

        while (true)
        {
            if (knightTurn)
            {
                
                if (Dodges(_monster.Status.Dex))
                {
                    _monster.Status.MaxHp -= playerDam;
                    OutputCombatText2(playerDam);
                }
                else
                {
                    OutputCombatMissText(_monster.Name, "<color=#008000>용사</color>");
                }


                knightTurn = false;
                monsterTurn = true;

                UpdateMonsterInfoText();

                if (_monster.Status.MaxHp <= 0)
                {
                    CombatPlayerWinText(_monster.Name);
                    
                    yield return PerformLevelUp();
                    SoundManager.Instance.Play(3);
                    
                    if(_isLastBoss) _gameManager.ShowSelectArtifact(); // 아티펙트 선택 화면 추가
                    
                    combatPanelExitButton.SetActive(true);
                    combatPanelExitButton.GetComponent<Button>().onClick.AddListener(End);
                    // 종료 로직
                    yield break;
                }

                
                _attDefCount++;
                if (DataManager.Instance.ARTI_AddAtack)
                {
                    var value = 2 * DataManager.Instance.ARTI_AddAtack_Interval;

                    if (_attDefCount >= value)
                    {
                        AppendBattleInfoText("\n<color=#682699>아티펙트의 효과로 한번 더 공격합니다.</color>");
                        knightTurn = true;
                        monsterTurn = false;
                        _attDefCount = -1;
                    }
                }
            }
            else if (monsterTurn)
            {
                if (Dodges(_knight.Status.Dex))
                {
                    _knight.Status.CurrentHp -= monsterDam;
                    OutputCombatText(_monster.Name, "<color=#008000>용사</color>", monsterDam, _knight.Status.CurrentHp);

                }
                else
                {
                    OutputCombatMissText("<color=#008000>용사</color>", _monster.Name);

                }

                monsterTurn = false;
                knightTurn = true;

                if (_knight.Status.CurrentHp <= 0)
                {
                    CombatMonsterWinText();
                    _uiManager.ActiveGameOverObj();
                    yield return new WaitForSeconds(2f);
                    _combatPanel.SetActive(false);
                    yield break;
                }
                
                _attDefCount++;
            }

            yield return new WaitForSeconds(0.6f);
        }
    }

    void Attack((string name, Status status) attacker, (string name, Status status) receiver)
    {
        _monster.Status.MaxHp -= _knight.Status.Power;
        OutputCombatText("<color=#008000>용사</color>", _monster.Name, _knight.Status.Power, _monster.Status.MaxHp);

    }

    private void End()
    {
        _combatPanel.SetActive(false);
        
        // 공통 동작
        combatPanelExitButton.SetActive(true);
        if (_knight.Status.Buff)
        {
            _knight.Status.Buff = false;
        }
        _gameManager.EventPrinting = false;
        
        // 보스를 깬 경우, 클리어 정보 추가
        if (_isLastBoss)
        {
            _gameManager.ClearBoss();
        }
    }



    #region Text Related

    void OutputCombatText(string name1, string name2, int name1power, int name2currentHP)
    {
        if (name2currentHP < 0)
        {
            name2currentHP = 0;
        }

        lineCount++;

        string currentText = combatText.text;
        string newCombatInfo = name1 + "(이)가 " + name1power + " 의 피해를 입혔습니다. " + name2 + "의 남은 HP = " + name2currentHP;
        string updatedText = currentText + "\n" + newCombatInfo;

        combatText.text = updatedText;


        if (lineCount > maxLines)
        {
            ScrollCombatText();
        }

    }

    void OutputCombatText2(int power)
    {

        lineCount++;

        string currentText = combatText.text;
        string newCombatInfo = "<color=#008000>용사</color>가 " + power + " 의 피해를 입혔습니다. " ;
        string updatedText = currentText + "\n" + newCombatInfo;

        combatText.text = updatedText;


        if (lineCount > maxLines)
        {
            ScrollCombatText();
        }

    }

    void OutputCombatMissText(string name1, string name2)
    {
        lineCount++;

        string currentText = combatText.text;
        string newCombatInfo = name1 + "(이)가 " + name2 + " 공격을 <color=#0019FA>회피</color>했습니다.";
        string updatedText = currentText + "\n" + newCombatInfo;

        combatText.text = updatedText;


        if (lineCount > maxLines)
        {
            ScrollCombatText();
        }

    }

    void TestCombat()
    {

        OutputCombatText("player", "monster", 5, 5);
    }


    void CombatPlayerWinText(string monsterName)
    {
        lineCount++;

        string currentText = combatText.text;
        string newCombatInfo = "<color=#008000>용사</color>가 " + monsterName + "(을)를 무찔렀습니다!";
        string updatedText = currentText + "\n" + newCombatInfo;

        combatText.text = updatedText;

        if (lineCount > maxLines)
        {
            ScrollCombatText();
        }

    }

    void CombatMonsterWinText()
    {
        lineCount++;

        string currentText = combatText.text;
        string newCombatInfo = "<color=#008000>용사</color>의 눈앞이 깜깜해집니다..";
        string updatedText = currentText + "\n" + newCombatInfo;

        combatText.text = updatedText;

        if (lineCount > maxLines)
        {
            ScrollCombatText();
        }
    }

    private void ScrollCombatText()
    {
        RectTransform contentRectTransform = combatText.transform.parent.GetComponent<RectTransform>();
        contentRectTransform.sizeDelta = new Vector2(contentRectTransform.sizeDelta.x, contentRectTransform.sizeDelta.y + 50f);

        StartCoroutine(UpdateScroll());
    }

    IEnumerator UpdateScroll()
    {
        yield return new WaitForSeconds(.01f);
        scrollbarVertical.GetComponent<Scrollbar>().value = 0f;
    }

    void ClearCombatText()
    {
        combatText.text = "";
        lineCount = 0;
    }

    string UpdateMonsterInfoText()
    {
        return monsterInf.text = $"HP: {_monster.Status.MaxHp}, 파워: {_monster.Status.Power}";
    }

    void AppendBattleInfoText(string text)
    {
        lineCount++;
        combatText.text += text;
        ScrollCombatText();
    }

    IEnumerator PerformLevelUp()
    {
        _knight.Status.Exp += _monster.Status.Exp;
        int expNeed = DataManager.Instance.ExpNeedForLevelUp[_knight.Status.Level -1];

        yield return new WaitForSeconds(0.5f);

        AppendBattleInfoText($"\n경험치를 {_monster.Status.Exp} 획득했습니다.");

        if (_knight.Status.Exp >= expNeed)
        {
            _knight.Status.Level++;
            _knight.Status.Exp -= expNeed;
            //레벨업 시, 최대 체력으로 회복
            //_knight.Status.CurrentHp = _knight.Status.MaxHp;
            
            yield return new WaitForSeconds(0.5f);
            AppendBattleInfoText($"\n용사의 레벨이 {_knight.Status.Level}로 올랐다!");
            _gameManager.StatusPoint++;
        }
    }
    #endregion
}