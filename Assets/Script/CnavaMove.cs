using Unity.Netcode;
using UnityEngine;

public class CnavaMove : NetworkBehaviour
{
    [SerializeField] private Transform Player;
    [SerializeField] private float height;

    Vector3 playerpos;
    Quaternion playerrot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //if(IsOwner)
        playerrot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        

        //transform.rotation=playerrot;

        playerpos = Player.position;
        playerpos.y = height;
        transform.position = playerpos;
    }

    private void LateUpdate()
    {
        transform.rotation = playerrot;
    }


}
