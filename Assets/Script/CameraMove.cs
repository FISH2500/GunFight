using Unity.Netcode;
using UnityEngine;

public class CameraMove : NetworkBehaviour
{
    [SerializeField] private Transform Player;

    Quaternion cameraRot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraRot = transform.rotation;

        if (!IsLocalPlayer) 
        {
            gameObject.SetActive(false);
        }

        transform.SetParent(null);

    }

    // Update is called once per frame
    void Update()
    {
        
        if (Player != null)
        {
            Vector3 player = Player.transform.position;

            transform.position = new Vector3(player.x, player.y + 13.75f, player.z - 10);
        }
    }

    private void LateUpdate()
    {
        transform.rotation = cameraRot;
    }
}
