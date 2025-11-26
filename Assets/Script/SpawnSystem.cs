using NUnit.Framework;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnSystem : NetworkBehaviour
{
    public List<Vector3> spawnPos;


    private void Awake()
    {
        
    }

    public override void OnNetworkSpawn() 
    {
        Debug.Log("スポーン");//スポーンの時に呼ばれる

        //transform.position = spawnPos[];

    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
