using Unity.Netcode;
using UnityEngine;

public class DeleteBullete : NetworkBehaviour
{
    [SerializeField] private float Time;
    public float power;
    public float uppower;
    public float Damage;
    public GameObject Owner;
    public NetworkVariable<ulong> OwnerID=new NetworkVariable<ulong>();

    void Start()
    {
        if(IsServer)
        Destroy(gameObject,Time);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall") 
        {
            if (IsServer)
                GetComponent<NetworkObject>().Despawn();
        }
    }
}
