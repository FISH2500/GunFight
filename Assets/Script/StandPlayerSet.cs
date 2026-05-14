using Unity.Netcode;
using UnityEngine;

public class StandPlayerSet : NetworkBehaviour
{

    public void Connect()
    {
        // // ©•ª‚ÌPlayeræ“¾
        NetworkObject playerObj = NetworkManager.Singleton.SpawnManager
             .GetLocalPlayerObject();
        Debug.Log("playerObj" + playerObj);
        if(playerObj!=null)
            playerObj.GetComponent<Status>().StandStartServerRpc();//Player‚ğ‘Ò‹@ƒ‚[ƒh‚É‚·‚é
    }
}
