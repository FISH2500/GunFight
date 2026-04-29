using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class SetModelAnimation : MonoBehaviour
{
    [SerializeField]
    Button[] selectCharButton;//キャラクター選択ボタン

    [SerializeField]
    Button enterButton;//決定ボタン

    [SerializeField]
    GameObject setCharacter;

    [SerializeField]
    SelectMode selectMode;

    private int id = -1; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var b in selectCharButton)
        {
            Button btn = b;

            btn.onClick.AddListener(()=>SelectModel(btn));
        }

        enterButton.onClick.AddListener(EnterChar);

        enterButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// ボタンが押された時
    /// </summary>
    /// <param name="clickButton">押されたボタンを引数とする</param>
    void SelectModel(Button clickButton) 
    {
        if (clickButton.GetComponent<SetCharInfo>() == null||
            id == clickButton.GetComponent<SetCharInfo>().id) return;//ボタンのキャラクター情報を取得できない場合

        if (setCharacter!=null)//キャラクターが既にセットされていた場合 
        {
            Destroy(setCharacter);
        }

        GameObject model= clickButton.GetComponent<SetCharInfo>().model;//クリックしたボタンのモデルを取得

        setCharacter=Instantiate(model);//モデルを生成

        id = clickButton.GetComponent<SetCharInfo>().id;

        enterButton.gameObject.SetActive(true);//キャラクターを選択している状態の為決定ボタンを表示する

    }
    /// <summary>
    /// キャラクター決定
    /// </summary>
    void EnterChar() 
    {
        selectMode.SetIndex(id);
        Destroy(setCharacter);
    }
}
