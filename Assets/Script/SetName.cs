using System.Collections;
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
    [SerializeField]
    PlayerData playerData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public override void OnNetworkSpawn()
    {
        Debug.Log(
    $"Object:{gameObject.name} " +
    $"Owner:{OwnerClientId} " +
    $"Local:{NetworkManager.LocalClientId} " +
    $"IsOwner:{IsOwner}"
);
        playerName.OnValueChanged += ChangeName;//playerNameの文字が変わったら呼ばれる

        UpdateName(playerName.Value.ToString());
        //StartCoroutine(SetNameRoutine());
        if (IsOwner)
        {
            Debug.Log("オーナが実行 " + NameEnter.PlayerName);
            
            //ローカル時点で入力した名前を取得
            SetNameServerRpc(NameEnter.PlayerName);
        }
        
        
    }
    //IEnumerator SetNameRoutine()
    //{
    //    yield return null;

    //    if (IsOwner)
    //    {
    //        SetNameServerRpc(NameEnter.PlayerName);
    //    }
    //}
    [ServerRpc]
    void SetNameServerRpc(string name)
    {
        Debug.Log("受け取った名前: " + name);
        playerName.Value = name;
    }


    // Update is called once per frame
    void Update()
    {

    }

    void ChangeName(FixedString32Bytes preTex,FixedString32Bytes newTex) //前、最新状態のテキストを引数とする
    {
        UpdateName(newTex.ToString());//最新状態の文字を引数として渡して更新
    }

    void UpdateName(string name) 
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = name;
        if(BattleUIManager.instance!=null)
        BattleUIManager.instance.OutPutScoreName(playerData.playerID.Value, name);
    }


}
