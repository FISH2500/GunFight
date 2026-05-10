using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NameEnter : MonoBehaviour
{
    [SerializeField]
    Button enterButton;

    [SerializeField]
    TMP_InputField nameInputField;

    [SerializeField]
    GameObject selectChar;//キャラクターセレクト関連UIの親オブジェクト

    public static string PlayerName;

    bool setName=false;//名前がセットされているか

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enterButton.onClick.AddListener(EnterName);
        enterButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        TriggerEnterButton();

        EnterKeyCheck();

    }

    void EnterName() 
    {
        PlayerName = nameInputField.text;

        selectChar.SetActive(true);

        gameObject.SetActive(false);
    }

    //決定ボタンの表示切り替え
    void TriggerEnterButton() 
    {
        if (nameInputField.text.Length > 0)//1文字以上入力されている
        {
            enterButton.gameObject.SetActive(true);
            setName = true;
        }
        else
        {
            enterButton.gameObject.SetActive(false);
            setName = false;
        }
    }

    //Enterキーでも決定できるように変更
    void EnterKeyCheck() 
    {
        if (Input.GetKeyDown(KeyCode.Return)&&setName)//名前がセットされておりEnterキーを押したとき
        {
            EnterName();
        }
    }

}
