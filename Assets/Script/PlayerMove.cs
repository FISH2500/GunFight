using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class PlayerMOve : NetworkBehaviour
{
    [SerializeField] private DynamicJoystick joystick;
    [SerializeField] private float speed;
    [SerializeField] private Animator animator;
    [SerializeField] private Shot shot;
    //[SerializeField] private Transform fanshape;
    private Quaternion targetRotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        joystick = FindObjectOfType<DynamicJoystick>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        AnimatorInputServerRpc(false);
        Move();
        //FanShapeMove();
    }

    void Move()
    {
        
        Vector2 dir = joystick.Direction;//UŒ‚—p‚Ìjoystick
        
        float rotationSpeed = 600f * Time.deltaTime; // ƒXƒ‰ƒCƒ€‚Ì‰ñ“]‘¬“x
        float moveX = joystick.Horizontal;
        float moveZ = joystick.Vertical;
        Vector3 target = new Vector3(moveX, 0, moveZ).normalized;
        if (dir != Vector2.zero)
        {
            AnimatorInputServerRpc(true);
            RotationInputServerRpc(target, rotationSpeed);
        }
        Vector3 move = target * speed * Time.deltaTime;

        MoveInputServerRpc(move);

    }
    [ServerRpc]
    void MoveInputServerRpc(Vector3 move) 
    {
        transform.Translate(move, Space.World);
    }

    [ServerRpc]
    void AnimatorInputServerRpc(bool isActive)
    {
        animator.SetBool("walk", isActive);
    }

    [ServerRpc]
    void RotationInputServerRpc(Vector3 target,float rotationSpeed)
    {
        if (target.magnitude > 0.5f)
        {
            targetRotation = Quaternion.LookRotation(target, Vector3.up);
        }
        if (!shot.isShot) transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);

    }


}
