using Unity.Netcode;
using UnityEngine;

public class SpawnSystem : NetworkBehaviour
{

    public override void OnNetworkSpawn() 
    {
        Debug.Log("スポーン");//スポーンの時に呼ばれる
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
