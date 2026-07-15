using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class PlayerMove : NetworkBehaviour
{
    [SerializeField] private DynamicJoystick joystick;
    [SerializeField] private float speed;
    [SerializeField] private Animator animator;
    [SerializeField] private Shot shot;
    //[SerializeField] private Transform fanshape;
    private Quaternion targetRotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public override void OnNetworkSpawn()
    {
        if (!GetComponent<PlayerData>().isControll)//コントローラーじゃない場合
        {
            Debug.Log("ネットワークスポーン");
            joystick = FindObjectOfType<DynamicJoystick>();
            joystick.enabled = false;
        }
    }

    void Start()
    {
        
        
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
        
        //Vector2 dir = joystick.Direction;//攻撃用のjoystick
        
        float rotationSpeed = 600f * Time.deltaTime; // スライムの回転速度
        //float moveX = joystick.Horizontal;
        //float moveZ = joystick.Vertical;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector2 dir = new Vector2(moveX, moveZ);
        //if (dir.magnitude<0.3f)
        //{
        //    return;
        //}


        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        // 上方向の成分を消す（地面方向だけ使う）
        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        //Vector3 target = new Vector3(moveX, 0, moveZ).normalized;

        Vector3 target=moveX*camRight+moveZ*camForward;

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

    //ジョイスティックを参照する関数
    public void FindJoyStick() 
    {
        Debug.Log("スティックを参照");
        joystick = FindObjectOfType<DynamicJoystick>();
        //joystick.enabled = false;
    }


}
