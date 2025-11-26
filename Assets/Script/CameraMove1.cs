using UnityEngine;

public class CameraMove1 : MonoBehaviour
{
    [SerializeField] private Transform Target;
    [SerializeField] private float yPos;
    [SerializeField] private float zPos;

    Vector3 TargetPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        TargetPos = Target.position;

        TargetPos.y = yPos;
        TargetPos.z=zPos;

        transform.position = TargetPos;

    }
}
