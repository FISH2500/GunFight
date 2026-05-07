using Unity.Netcode;
using UnityEngine;

public class CameraMove : NetworkBehaviour
{
    [SerializeField] private Transform Player;

    Quaternion cameraRot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {

    }

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
            // ƒvƒŒƒCƒ„[‚ÌŒã‚ë•ûŒü‚Ö10—£‚·
            Vector3 offset = -transform.forward * 15f;

            // ã•ûŒü‚ÍŒÅ’è
            offset += new Vector3(0, 10f, 0);

            transform.position = Player.position + offset;
        }
    }

    private void LateUpdate()
    {
        transform.rotation = cameraRot;
    }


}
