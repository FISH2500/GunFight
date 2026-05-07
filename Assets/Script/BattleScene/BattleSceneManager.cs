using Unity.Netcode;
using UnityEngine;

public class BattleSceneManager : MonoBehaviour
{
    void Start()
    {
        Connect();
    }

    void Connect()
    {
       // // スティック取得
       //// Joystick stick = FindObjectOfType<Joystick>();

       // // 自分のPlayer取得
       // NetworkObject playerObj = NetworkManager.Singleton.SpawnManager
       //     .GetLocalPlayerObject();

       // if (playerObj == null)
       // {
       //     Debug.LogWarning("Player not found");
       //     return;
       // }
       // // 移動用スティックの参照
       // PlayerMove player = playerObj.GetComponent<PlayerMove>();
       // if (player != null) player.FindJoyStick();

       // //攻撃用スティックの参照
       // Shot shot = playerObj.GetComponent<Shot>();
       // if (shot != null) shot.FindFloatingJoyStick();

       // 
       // //player.SetStick(stick);
    }
}
