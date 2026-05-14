using System;
using Unity.Netcode;
//using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.SceneManagement;
using Unity.Networking.Transport;
using System.Collections.Generic;

public class SelectMode : NetworkBehaviour
{
    [SerializeField]
    GameObject MenuCanva;

    [SerializeField]
    GameObject JoyStickCanva;

    [SerializeField]
    GameObject shutdown;



    [SerializeField]
    GameObject HostorJoin;

    [SerializeField]
    GameObject SelectCharButton;

    [SerializeField]
    SpawnSystem spawnSystem;

    private bool isTrigger=false;

    CharSelect select;

    int gIndex;

    

    private void Awake()
    {

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        select=gameObject.GetComponent<CharSelect>();
        PlayerDataManager.instance.PlayerCountReset();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("接続数"+NetworkManager.ConnectedClients.Count);

        if (NetworkManager.ConnectedClients.Count >= 2 && !isTrigger&&GameManager.instance.isStart.Value)//ホストがスタートボタンを押した場合
        {
            isTrigger = true;

            shutdown.SetActive(false);

        }


    }

    public void Host()//ホストを選択した場合
    {
        NetworkManager.OnClientDisconnectCallback += OnClientDisconnect;
        //NetworkManager.StartHost();
        
        GameStart();

        //SceneManager.LoadScene("BattleScene");

        PlayerSpawnServerRpc(gIndex);
        PlayerDataManager.instance.SetIndexServerRpc(gIndex);
    }

    public void Client()//クライアントを選択した場合
    {
        NetworkManager.OnClientConnectedCallback += OnClientConnected;
        

        //NetworkManager.StartClient();
        
        GameStart();

        //SceneManager.LoadScene("BattleScene");

    }
    public void OnLeaveButton()//切断した場合
    {
        NetworkManager.Singleton.Shutdown();
        //ReturnMenu();
        //NetworkManager.Singleton.SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
        SceneManager.LoadScene("SampleScene");
    }
    public void OnRetryButton()//再戦する場合 
    {
        Debug.Log("再戦");

        //PlayerSpawnServerRpc();Playerのスポーン
        NetworkManager.Singleton.SceneManager.LoadScene("BattleScene", LoadSceneMode.Single);

    //.GetLocalPlayerObject());

        //PlayerSpawnServerRpc(gIndex);

    }

    void GameStart() 
    {
        MenuCanva.SetActive(false);
        JoyStickCanva.SetActive(true);
    }

    void ReturnMenu() 
    {
        MenuCanva.SetActive(true);
        JoyStickCanva.SetActive(false);
    }


    private void OnClientConnected(ulong clientId)
    {
        // 自分自身ならスポーン処理
        if (clientId == NetworkManager.LocalClientId)
        {
            GameStart();
            PlayerSpawnServerRpc(gIndex);
            //PlayerDataManager.instance.SetIndexServerRpc(gIndex);
        }

        NetworkManager.OnClientConnectedCallback -= OnClientConnected;
    }

    private void OnClientDisconnect(ulong clientid) 
    {
        Debug.Log("呼ばれたclient"+clientid+"HostID"+ NetworkManager.ServerClientId);
        if (clientid == NetworkManager.ServerClientId) 
        {
            Debug.Log("ホストが切断");
        }
    }

    public void SetIndex(int index) 
    {
        gIndex = index;
        
        HostorJoin.SetActive(true);
        SelectCharButton.SetActive(false);
        
    }

    [ServerRpc(RequireOwnership = false)]
    public void PlayerSpawnServerRpc(int index, ServerRpcParams rpcParams = default)
    {
        ulong clientID = rpcParams.Receive.SenderClientId;

        GameObject prefab = select.Character[index];

        var playerObj = Instantiate(prefab);

        spawnSystem.SetSpawnPosition(playerObj, PlayerDataManager.instance.playerCount);

        playerObj.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientID);

        //playerObj.GetComponent<PlayerData>().playerID= PlayerDataManager.instance.playerCount;接続番号を取得

        PlayerDataManager.instance.playerCount++;

    }




}
