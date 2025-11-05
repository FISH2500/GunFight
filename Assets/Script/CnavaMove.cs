using UnityEngine;

public class CnavaMove : MonoBehaviour
{
    [SerializeField] private Transform Player;
    [SerializeField] private float height;

    Vector3 playerpos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerpos = Player.position;
        playerpos.y = height;
        transform.position = playerpos;
    }
}
