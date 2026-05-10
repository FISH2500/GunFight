using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinButton : MonoBehaviour
{
    [SerializeField]
    Button hostButton;

    [SerializeField]
    Button joinButton;

    [SerializeField]
    TMP_InputField inputCodeField;

    [SerializeField]
    SetRelay setRelay;


    bool pushHost=false;//ホストボタンが押された
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hostButton.onClick.AddListener(PushHostButon);
        joinButton.onClick.AddListener(PushJoinButton);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //参加ボタンが押された時
    void PushJoinButton() 
    {
        string joinCode= inputCodeField.text;

        setRelay.JoinRelay(joinCode);

    }
    //ホストボタンが押された時
    void PushHostButon() 
    {
        gameObject.SetActive(false);
        setRelay.CreateRelay();
    }

}
