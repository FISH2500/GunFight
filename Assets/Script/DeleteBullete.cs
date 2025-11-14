using UnityEngine;

public class DeleteBullete : MonoBehaviour
{
    [SerializeField] private float Time;
    public float power;
    public float uppower;
    public float Damage;

    void Start()
    {
        Destroy(gameObject,Time);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
