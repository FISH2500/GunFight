using NUnit.Framework;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnSystem : NetworkBehaviour
{
    [System.Serializable]
    public class Spawn
    {
        public List<Transform> spawnPos=new List<Transform>();
        public List<Quaternion> spawnRot = new List<Quaternion>();



    }
    [SerializeField]
    Spawn spawn;

    [SerializeField]
    CameraMove cam;

    [SerializeField]
    GameObject[] Character;

    SelectMode selectMode;

    int clientId;

    private void Awake()
    {
        //接続したPlayerのオブジェクトを取得

    }

    private void Start()
    {
        GameObject selectObj = GameObject.Find("CharSelectManager");

        selectMode=selectObj.GetComponent<SelectMode>();

        if (selectMode == null) 
        {
            Debug.Log("SelectModeスクリプトが見つからない");
        }



        PlayerSpawn();

    }

    public override void OnNetworkSpawn()
    {
        //cam.SetCam();
        Debug.Log("スポーン");//スポーンの時に呼ばれる
        Debug.Log("ID:" + clientId);




    }

    public void SetSpawnPosition(GameObject spawnPlayer,ulong clientID) //スポーンしたオブジェクトと何番目にスポーンしたPlayerであるか確認するspawnIDを引数とする
    {
        //clientIDをもとに回転,場所をきめる
        spawnPlayer.transform.rotation = spawn.spawnRot[(int)clientID];
        spawnPlayer.transform.position = spawn.spawnPos[(int)clientID].position;
    }

    public void PlayerDespawn() 
    {
        //現在残っているPlayerをすべてリセット
        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)//Listに格納されているPlayerを参照
        {
            var playerObj = client.PlayerObject;//全Playerのオブジェクトを取得

            if (playerObj != null)
            {
                playerObj.Despawn();
            }
        }
    }

    public void PlayerSpawn() 
    {
        //Playerのスポーン
        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            ulong clientID = client.ClientId;

            int index = selectMode.playerIndex[clientID];

            GameObject prefab = Character[index];

            SetSpawnPosition(prefab, clientID);

            var playerObj = Instantiate(prefab);

            playerObj.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientID);
        }
    }
    //リスポーン
    public void Respawn() 
    {

        

        //現在残っているPlayerをすべてリセット
        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)//Listに格納されているPlayerを参照
        {
            var playerObj = client.PlayerObject;//全Playerのオブジェクトを取得

            if (playerObj != null)
            {
                playerObj.Despawn(); 
            }
        }

        NetworkManager.Singleton.SceneManager.LoadScene("BattleScene", LoadSceneMode.Single);


    }

}
