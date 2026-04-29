using System;
using Unity.Netcode;
//using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.SceneManagement;
using Unity.Networking.Transport;

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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        select=gameObject.GetComponent<CharSelect>();
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

    public void Host()//ホストを選択した場合s 
    {
        NetworkManager.OnClientDisconnectCallback += OnClientDisconnect;
        //NetworkManager.StartHost();
        
        GameStart();

        PlayerSpawnServerRpc(gIndex);

    }

    public void Client()//クライアントを選択した場合
    {
        NetworkManager.OnClientConnectedCallback += OnClientConnected;
        

        //NetworkManager.StartClient();
        
        GameStart();

    }
    public void OnLeaveButton()//切断した場合
    {
        NetworkManager.Singleton.Shutdown();
        ReturnMenu();
        SceneManager.LoadScene("SampleScene");
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

        spawnSystem.SetSpawnPosition(prefab, clientID);

        var playerObj = Instantiate(prefab);

        playerObj.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientID);
    }


}
