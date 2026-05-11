using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class SetName : NetworkBehaviour
{
    private NetworkVariable<FixedString32Bytes> playerName=new NetworkVariable<FixedString32Bytes>//すべてのPlayerに対して同期させたい文字
        ("NoName",
        NetworkVariableReadPermission.Everyone,//読み取り許可
        NetworkVariableWritePermission.Server//書き込み許可
        );

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public override void OnNetworkSpawn()
    {
        Debug.Log("オーナ? " + IsOwner);

        playerName.OnValueChanged += ChangeName;//playerNameの文字が変わったら呼ばれる

        UpdateName(playerName.Value.ToString());

        if (IsOwner)
        {
            Debug.Log("オーナが実行 " + NameEnter.PlayerName);

            //playerName.Value = NameEnter.PlayerName;
            //ローカル時点で入力した名前を取得
            SetNameServerRpc(NameEnter.PlayerName);
        }
        
        
    }

    [ServerRpc]
    void SetNameServerRpc(string name)
    {
        Debug.Log("受け取った名前: " + name);
        playerName.Value = name;
    }


    // Update is called once per frame
    void Update()
    {
        //gameObject.GetComponent<TextMeshProUGUI>().text = playerName.Value.ToString();
    }

    void ChangeName(FixedString32Bytes preTex,FixedString32Bytes newTex ) //前、最新状態のテキストを引数とする
    {
        UpdateName(newTex.ToString());//最新状態の文字を引数として渡して更新
    }

    void UpdateName(string name) 
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = name;
        if(BattleUIManager.instance!=null)
        BattleUIManager.instance.OutPutScoreName(PlayerDataManager.instance.playerCount, name);
    }
}
