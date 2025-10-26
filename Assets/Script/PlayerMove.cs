using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMOve : MonoBehaviour
{
    [SerializeField] private VariableJoystick joystick;
    [SerializeField] private float speed;

    private Quaternion targetRotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        float rotationSpeed = 600f * Time.deltaTime; // ƒXƒ‰ƒCƒ€‚Ì‰ñ“]‘¬“x
        float moveX = joystick.Horizontal;
        float moveZ = joystick.Vertical;
        Vector3 target = new Vector3(moveX, 0, moveZ).normalized;
        if (target.magnitude > 0.5f)
        {
            targetRotation = Quaternion.LookRotation(target, Vector3.up);
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);

        Vector3 move = target * speed * Time.deltaTime;

        transform.Translate(move, Space.World);

    }

}
