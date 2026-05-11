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
    public NetworkVariable<ulong> OwnerID=new NetworkVariable<ulong>();//íeÇÃèäéùé“Çï\Ç∑

    private Vector3 targetPos;

    void Start()
    {

    }

    public void SetTargetPos(Vector3 pos) 
    {
        targetPos = pos;
    }

    void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position,
        //                                       targetPos,
        //                                       speed * Time.deltaTime);

        //if (Vector3.Distance(transform.position, targetPos) < 0.1f) 
        //{
        //    if(IsServer) GetComponent<NetworkObject>().Despawn();
        //}
        Vector3 nextPos =
    Vector3.MoveTowards(
        transform.position,
        targetPos,
        speed * Time.deltaTime);

        Vector3 dir = nextPos - transform.position;
        float distance = dir.magnitude;

        RaycastHit hit;

        if (Physics.Raycast(transform.position,
                            dir.normalized,
                            out hit,
                            distance))
        {
            if (hit.collider.CompareTag("Wall"))
            {
                transform.position = hit.point;

                if (IsServer)
                {
                    GetComponent<NetworkObject>().Despawn();
                }

                return;
            }
        }

        transform.position = nextPos;

        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            if (IsServer)
            {
                GetComponent<NetworkObject>().Despawn();
            }
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Wall")
    //    {
    //        if (IsServer)
    //            GetComponent<NetworkObject>().Despawn();
    //    }
    //}
}
