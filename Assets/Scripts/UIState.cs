
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIState : MonoBehaviour
{
    GameManager _gameManager;
    StateInfo _stateInfo;
    GameObject _gameObject;

    public TextMeshProUGUI name;
    public TextMeshProUGUI description;
    public Text turnText;


    public void Init(StateInfo stateInfo)
    {
        _gameManager = GameObject.Find(nameof(GameManager)).GetComponent<GameManager>();
        _stateInfo = stateInfo;
        _gameObject = this.gameObject;

        if (stateInfo.state == PlayerState.Cold)
        {
            _gameObject.GetComponent<Image>().color = Color.blue;
        }
        else if (stateInfo.state == PlayerState.Drowsy)
        {
            _gameObject.GetComponent<Image>().color = new Color(255f / 255f, 165f / 255f, 0f / 255f);
        }
        else
        {
            _gameObject.GetComponent<Image>().color = Color.red;
        }
        name.text = stateInfo.stateName;
        description.text = stateInfo.stateDescription;
        turnText.text = stateInfo.count.ToString() + "ео";
    }

}
