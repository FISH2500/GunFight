using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetErrorPanel : MonoBehaviour
{

    [SerializeField]
    GameObject errorPanel;

    [SerializeField]
    TextMeshProUGUI errorText;

    [SerializeField]
    Button enterButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enterButton.onClick.AddListener(PushEnterButton);
    }

    public void ShowErroPanale(string errorCode,GameObject joinButton) 
    {
        errorPanel.SetActive(true);//エラーのメッセージボックスを表示
        errorText.text = errorCode;//エラーの内容
        joinButton.SetActive(true);
    }

    //エラーのメッセージボックスのOKボタンが押された時
    private void PushEnterButton() 
    {
        errorPanel.SetActive(false);
        errorText.text = "";
    }
}
