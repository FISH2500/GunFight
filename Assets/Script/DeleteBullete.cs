using Unity.Netcode;
using UnityEngine;

public class DeleteBullete : NetworkBehaviour
{
    [SerializeField] private float Time;
    public float power;
    public float uppower;
    public float Damage;
    public GameObject Owner;
    public ulong OwnerID;

    void Start()
    {
        if(IsServer)
        Destroy(gameObject,Time);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("UŒ‚Ò"+OwnerID);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        //if (Owner==null) 
        //{
        //    Debug.LogError("”­Ël•¨‚ª“Á’è‚Å‚«‚Ä‚È‚¢");
        //}
        //else 
        //{
        //    Debug.Log("”­Ël•¨F" + Owner);
        //}

        if (other.tag == "Wall") 
        {
            if (IsServer)
                Destroy(gameObject);
        }
    }
}
