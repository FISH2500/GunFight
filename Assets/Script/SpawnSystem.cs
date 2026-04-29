using NUnit.Framework;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

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

    //NetworkVariable<char> spawnID = new NetworkVariable<char>();

    int spawnCount = 0;

    int clientId;

    private void Awake()
    {
        //接続したPlayerのオブジェクトを取得

    }

    //public override void OnNetworkSpawn() 
    //{
    //    //cam.SetCam();
    //    Debug.Log("スポーン");//スポーンの時に呼ばれる
    //    Debug.Log("ID:" + clientId);
    //}

    public void SetSpawnPosition(GameObject spawnPlayer,ulong clientID) //スポーンしたオブジェクトと何番目にスポーンしたPlayerであるか確認するspawnIDを引数とする
    {
        //clientIDをもとに回転,場所をきめる
        spawnPlayer.transform.rotation = spawn.spawnRot[(int)clientID];
        spawnPlayer.transform.position = spawn.spawnPos[(int)clientID].position;
    }

}
