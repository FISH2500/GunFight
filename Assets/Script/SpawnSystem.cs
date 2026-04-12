using NUnit.Framework;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnSystem : NetworkBehaviour
{
    [System.Serializable]
    public class Spawn
    {
        public List<Vector3> spawnPos=new List<Vector3>();
        public List<Quaternion> spawnRot = new List<Quaternion>();



    }
    [SerializeField]
    Spawn spawn;

    [SerializeField]
    CameraMove cam;

    int clientId;

    private void Awake()
    {
        clientId = NetworkManager.ConnectedClients.Count;
        transform.position = spawn.spawnPos[clientId - 1];
        transform.rotation = spawn.spawnRot[clientId - 1];

    }

    public override void OnNetworkSpawn() 
    {
        //cam.SetCam();
        Debug.Log("スポーン");//スポーンの時に呼ばれる
        Debug.Log("ID:" + clientId);
    }
}
