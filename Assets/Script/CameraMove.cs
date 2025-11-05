using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private Transform Player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
}
