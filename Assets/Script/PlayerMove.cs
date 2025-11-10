using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMOve : MonoBehaviour
{
    [SerializeField] private DynamicJoystick joystick;
    [SerializeField] private FloatingJoystick Attack_joystick;
    [SerializeField] private float speed;
    [SerializeField] private Animator animator;
    [SerializeField] private Shot shot;
    //[SerializeField] private Transform fanshape;
    private Quaternion targetRotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("walk", false);
        Move();
        //FanShapeMove();
    }

    void Move()
    {
        
        Vector2 dir = joystick.Direction;//UŒ‚—p‚Ìjoystick
        Vector2 attackdir = Attack_joystick.Direction;//UŒ‚—p‚Ìjoystick
        float rotationSpeed = 600f * Time.deltaTime; // ƒXƒ‰ƒCƒ€‚Ì‰ñ“]‘¬“x
        float moveX = joystick.Horizontal;
        float moveZ = joystick.Vertical;
        Vector3 target = new Vector3(moveX, 0, moveZ).normalized;
        if (dir != Vector2.zero)
        {
            animator.SetBool("walk", true);
            if (target.magnitude > 0.5f)
            {
                targetRotation = Quaternion.LookRotation(target, Vector3.up);
            }
            if(!shot.isShot) transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);

        }
        Vector3 move = target * speed * Time.deltaTime;

        transform.Translate(move, Space.World);

    }

    //void FanShapeMove() 
    //{
    //    Vector2 attackdir = Attack_joystick.Direction;//UŒ‚—p‚Ìjoystick
    //    float rotationSpeed = 600f * Time.deltaTime; // ‰ñ“]‘¬“x
    //    float moveX = Attack_joystick.Horizontal;
    //    float moveZ = Attack_joystick.Vertical;
    //    Vector3 target = new Vector3(moveX, 0, moveZ).normalized;

    //    if (target.magnitude > 0.5f)
    //    {
    //        targetRotation = Quaternion.LookRotation(target, Vector3.up);
    //    }
    //    fanshape.transform.rotation = Quaternion.RotateTowards(fanshape.transform.rotation, targetRotation, rotationSpeed);
    //}


}
