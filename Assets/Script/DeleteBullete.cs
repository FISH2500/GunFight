using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class DeleteBullete : NetworkBehaviour
{
    //[SerializeField] private float Time;
    public float speed;
    public float uppower;
    public float Damage;
    public GameObject Owner;
    public NetworkVariable<ulong> OwnerID=new NetworkVariable<ulong>();

    private Vector3 targetPos;

    void Start()
    {
        //if(IsServer)
        //Destroy(gameObject,Time);
    }

    public void SetTargetPos(Vector3 pos) 
    {
        targetPos = pos;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position,
                                               targetPos,
                                               speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.1f) 
        {
            Destroy(gameObject);
        }
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
