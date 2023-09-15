
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIState : MonoBehaviour
{
    GameManager _gameManager;
    StateInfo _stateInfo;

    public TextMeshProUGUI name;
    public TextMeshProUGUI description;


    public void Init(StateInfo stateInfo)
    {
        _gameManager = GameObject.Find(nameof(GameManager)).GetComponent<GameManager>();
        _stateInfo = stateInfo;
        name.text = stateInfo.stateName;
        description.text = stateInfo.stateDescription;
    }

}
