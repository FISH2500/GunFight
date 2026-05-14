using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerDataManager : NetworkBehaviour
{

    public static PlayerDataManager instance { private set; get; }

    public int[] playerSelectIndex = new int[2];

    //public Dictionary<ulong ,int> playerIndex = new Dictionary<ulong ,int>();//Playerの接続番号を確かめる

    public int playerCount = 0;

    private void Awake()
    {
        //すでにオブジェクトが存在している場合削除
        if (instance != null) 
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        

        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //playerCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //選択キャラの保存
    [ServerRpc(RequireOwnership = false)]
    public void SetIndexServerRpc(int index, ServerRpcParams rpcParams = default)
    {
        ulong clientID = rpcParams.Receive.SenderClientId;

        NetworkObject playerObj = NetworkManager.Singleton.SpawnManager
    .GetLocalPlayerObject();

        playerSelectIndex[playerCount] = index;

        Debug.Log($"保存: {playerObj.GetComponent<PlayerData>().playerID.Value} → {index}");

        playerCount++;

    }

    //Playerカウントのリセット
    public void PlayerCountReset()
    {
        playerCount = 0;
    }

    ////Player番号の登録
    //public void PlayerNumRegistration(ulong clientID) 
    //{
    //    //if (!playerIndex.ContainsKey(clientID))
    //    {
    //        //playerIndex[clientID] = playerCount%2;
    //    }
    //    Debug.Log("clientID" + clientID + "index" + playerIndex.Count);
    //    playerCount++;
    //}

    //public int GetPlayerNum(ulong clientID) 
    //{
    //    if(playerIndex.TryGetValue(clientID,out int index))//そのclientIDが登録されていない場合
    //    {
    //        return index;//クライアントIDに基づいて番号をチェック
    //    }
    //    Debug.LogWarning("未登録のclientID:"+ clientID);
    //    return -1;
    //}
}
