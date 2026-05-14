using Unity.Netcode;
using UnityEngine;

public class PlayerData : NetworkBehaviour
{

    public NetworkVariable<int> playerID = new NetworkVariable<int>();


}
