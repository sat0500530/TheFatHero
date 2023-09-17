using System.Collections.Generic;
using UnityEngine;

public class FieldEvent : MonoBehaviour
{
    private GameManager _gameManager;
    private UIImgText _uiImgTxt;
    
    /// <summary>
    /// 텍스트
    /// </summary>
    public string Text { get; set; }
    public Sprite Sprite { get; set; }
    
    #region 선택지
    /// <summary>
    /// 선택지 (선택지과 인덱스 동일할 것)
    /// </summary>
    public string Question { get; set; }
    
    /// <summary>
    /// 확률 (선택지과 인덱스 동일할 것)
    /// </summary>
    public string Percent { get; set; }
    
    /// <summary>
    /// 응답 (선택지과 인덱스 동일할 것)
    /// </summary>
    public string Answer { get; set; }
    #endregion
    
    public void Execute(FieldEventInfo info)
    {
        gameObject.SetActive(true);
        _uiImgTxt ??= GetComponent<UIImgText>();
        _gameManager ??= GameObject.Find(nameof(GameManager)).GetComponent<GameManager>();


        switch (info.Type){
            case EventType.HP:
                _gameManager.knight.Status.CurrentHp += info.EffectAmount;
            break;
            case EventType.Power:
                _gameManager.knight.Status.levelPow += info.EffectAmount;

            break;
            case EventType.Dex:
                _gameManager.knight.Status.levelDex += info.EffectAmount;

            break;
            case EventType.Exp:
                _gameManager.knight.Status.Exp += info.EffectAmount;

            break;
            case EventType.Hunger:
                _gameManager.GetHunger(-info.EffectAmount);

            break;
            case EventType.StateCount:
                _gameManager._playerStateController.DecreaseStateCount(info.EffectAmount);
            break;
            case EventType.Cold:
                _gameManager._playerStateController.AddState(PlayerState.Cold, _gameManager._playerStateController.GetStateName(PlayerState.Cold), _gameManager._playerStateController.GetStateDescription(PlayerState.Cold));
            break;
            case EventType.Cost:
                Debug.Log(_gameManager.knight.Cost);
                _gameManager.knight.Cost += info.EffectAmount;
                Debug.Log(_gameManager.knight.Cost);

            break;

        }

        _uiImgTxt.Init(info.Sprite, End, info.GetText);
    }


    void End()
    {
        gameObject.SetActive(false);
        _gameManager.EventPrinting = false;
    }
}
