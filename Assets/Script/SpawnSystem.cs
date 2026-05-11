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

    int clientId;

    public int SpawnIndex;//何番目にスポーンしたのかチェックする

    private void Awake()
    {
        //接続したPlayerのオブジェクトを取得

    }

    private void Start()
    {
        if (!IsServer) return;
        PlayerSpawn();
    }

    public override void OnNetworkSpawn()
    {
        //cam.SetCam();
        Debug.Log("スポーン");//スポーンの時に呼ばれる
        Debug.Log("ID:" + clientId);


        

    }
    /// <summary>
    /// スポーン位置を決める関数
    /// </summary>
    /// <param name="spawnPlayer">スポーンするキャラ</param>
    /// <param name="spawnIndex">何番目にスポーンしたPlayerであるか</param>
    public void SetSpawnPosition(GameObject spawnPlayer,int spawnIndex) 
    {
        

        //clientIDをもとに回転,場所をきめる
        spawnPlayer.transform.rotation = spawn.spawnRot[spawnIndex];
        spawnPlayer.transform.position = spawn.spawnPos[spawnIndex].position;
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
        int spawnIndex = 0;
        //Playerのスポーン
        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            ulong clientID = client.ClientId;

            int index = PlayerDataManager.instance.playerSelectIndex[clientID];

            GameObject prefab = Character[index];

            var playerObj = Instantiate(prefab);

            SetSpawnPosition(playerObj, spawnIndex);

            spawnIndex++;

            playerObj.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientID);
        }
    }
    //リスポーン
    public void Respawn() 
    {
        //現在残っているPlayerをすべてリセット
        PlayerDespawn();

        NetworkManager.Singleton.SceneManager.LoadScene("BattleScene", LoadSceneMode.Single);

    }

    //全キャラクターをスポーン
    public void PlayerAllRespawn() 
    {
        PlayerDespawn();

        PlayerSpawn();
    }

    /// <summary>
    /// SpawnIndexを初期化する関数
    /// </summary>
    public void InitSpawnIndex()
    {
        SpawnIndex=0;
    }


    /// <summary>
    /// スポーンする場合にSpawnIndexを増やす関数
    /// </summary>
    public void AddSpawnIndex() 
    {
        SpawnIndex++;
    }

    /// <summary>
    /// 現在のプレイヤーが何番目なのかを確認する関数
    /// </summary>
    /// <returns>SpawnIndex</returns>
    public int GetSpawnIndex() 
    {
        return SpawnIndex;
    }

    

}
