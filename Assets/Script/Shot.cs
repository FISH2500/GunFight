using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Shot : NetworkBehaviour
{
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private GameObject[] bullete;
    [SerializeField] private Transform firepoint;
    [SerializeField] private Transform collidershape;
    [SerializeField] private float addRotateValue;
    [SerializeField] private float power;
    [SerializeField] private float[] bulleteCount;
    [SerializeField] private GameObject bulleteGage_Parent;
    [SerializeField] private Image bulleteGage_Orange;
    [SerializeField] private Image bulleteGage_Gray;
    [SerializeField] private float Gage;
    [SerializeField] private Animator animator;

    public float reloadTime = 0.8f;  // リロードにかかる時間

    private Vector3 target;
    private Vector3 fanShapetarget;
    public bool isShot = false;

    private enum PlayerType  { ShotGun, Assault ,BomuThrow};

    [SerializeField] PlayerType player = new PlayerType();

    [SerializeField] private float ChargeRate;

    private Coroutine reCharge;


    private bool isShotEnd = false;
    private bool isReload = false;
    private bool shooting = false;
    private Vector2 oldJoyStick;

    void Start()
    {

        joystick = FindObjectOfType<FloatingJoystick>();
        joystick.enabled = false;
        Gage = 0.9f;
        collidershape.gameObject.SetActive(false);
        if (IsOwner) 
        {
            bulleteGage_Parent.SetActive(true);//ゲージの親オブジェクトを取得して、自分自身ならりリロードゲージを表示
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        
        if (Gage < 0.9f&&isReload)//ゲージが満タンじゃないかつ発射後0.8s後に回復するようにクールタイムを設ける 
        {
            Gage += ChargeRate * Time.deltaTime;

            if (Gage > 0.9f) Gage = 0.9f;
        }

        Vector2 dir = joystick.Direction;


        if (dir.magnitude>0.1f&&dir.magnitude <= 0.5f)//射撃をキャンセルする条件 スティックが0.5fより小さい場合
        {
            
            oldJoyStick = dir;
        }

        if (dir != Vector2.zero&& dir.magnitude >= 0.4f)//射撃ボタンを動かしている時,スティックを伸ばしていない時
        {
            
            collidershape.gameObject.SetActive(true);
            float fanMoveX = joystick.Horizontal;
            float fanMoveZ = joystick.Vertical;

            Vector3 camForward = Camera.main.transform.forward;
            Vector3 camRight = Camera.main.transform.right;

            // 上方向の成分を消す（地面方向だけ使う）
            camForward.y = 0;
            camRight.y = 0;

            camForward.Normalize();
            camRight.Normalize();


            //fanShapetarget = new Vector3(fanMoveX, 0, fanMoveZ).normalized;
            fanShapetarget = camForward*fanMoveZ+camRight*fanMoveX;

            Quaternion baseRotation = Quaternion.LookRotation(fanShapetarget);

            collidershape.transform.rotation = baseRotation * Quaternion.Euler(0, addRotateValue, 0);
        }
        else 
        {
            
            collidershape.gameObject.SetActive(false);

        }
        bulleteGage_Orange.fillAmount = Gage;



        ShotCheck();
    }

    void ShotCheck() 
    {
        if (0.29 <= Gage)
        {
            //
            //joystickを離したかチェック

            Vector2 dir = joystick.Direction;

            if (dir != Vector2.zero)//joystickを動かしている場合
            {
                float moveX = joystick.Horizontal;
                float moveZ = joystick.Vertical;
                Vector3 camForward = Camera.main.transform.forward;
                Vector3 camRight = Camera.main.transform.right;

                // 上方向の成分を消す（地面方向だけ使う）
                camForward.y = 0;
                camRight.y = 0;

                camForward.Normalize();
                camRight.Normalize();

                //target = new Vector3(moveX, 0, moveZ).normalized;

                target=camForward*moveZ+camRight*moveX;
                
                isShotEnd = false;

                //if(!shooting) shooting = false;

            }

            if (dir == Vector2.zero && !isShotEnd&&0.4f<oldJoyStick.magnitude&&!shooting)//Gageが0より多い場合 
            {
                
                animator.SetBool("isForwardShot", true);
                isReload = false;
                shooting = true;
                
                isShotEnd = true;
                //isShot = true;
                if (reCharge != null)//もし前のこるーちんが処理中の場合ストップする 
                {
                    StopCoroutine(reCharge);
                }
                reCharge = StartCoroutine(ReChargeBullete());//新しくこるーちんの処理を開始する

                RotateServerRpc(target);
                switch (player)
                {
                    case PlayerType.ShotGun:
                        Shot_ShotGun();
                        //Debug.Log("方向:"+target);
                        //    rb.AddForce(new Vector3(target.x + (i * 0.5f - 0.5f)/2, target.y,target.z + (i * 0.5f - 0.5f)/2) * power, ForceMode.Impulse);
                        break;

                    case PlayerType.Assault:
                        
                        Shot_Assault();
                        break;

                    case PlayerType.BomuThrow:
                        Shot_Bomu();
                        break;

                }

            }
        }
    }

    void Reload() 
    {
        
        isReload = true;
        Debug.Log("リロード許可" + isReload);

    }

    void Shot_ShotGun()
    {
        Gage -= 0.3f;
        for (int i = 0; i < bulleteCount[0]; i++)
        {
            BulleteServerRpc(i,target,OwnerClientId);
        }
        shooting = false;
    }

    private IEnumerator ReChargeBullete() 
    {
        
        yield return new WaitForSeconds(reloadTime);//2秒後に処理
        Reload();

    }

    void Shot_Assault()
    {
        if (!isShot)
            StartCoroutine(AssaultBurst());
    }
    private IEnumerator AssaultBurst()
    {
        
        Debug.Log("shot" + isShot);
        Gage -= 0.3f;

        int shots = 6;                  // 撃つ回数
        float interval = 0.1f;          // 発射間隔（秒）
        float sideOffset = 0.5f;        // 左右のズレ幅
        
        for (int i = 0; i < shots; i++)
        {

            Assault_BulleteServerRpc(i, sideOffset, OwnerClientId);

            yield return new WaitForSeconds(interval); // ← 間隔を空ける
        }
        shooting = false;
        isShotEnd = true;
        IsShotCancelServerRpc();
        //isShot = false;
    }

    void Shot_Bomu() 
    {
        if (!isShot)
            StartCoroutine(Bomu());
    }

    private IEnumerator Bomu()
    {
        isShot = true;
        Gage -= 0.3f;
        float interval = 0.1f;

        Vector3 firestart = firepoint.position;

        GameObject Bullete = Instantiate(bullete[2], firestart, transform.rotation);
        Rigidbody rb = Bullete.GetComponent<Rigidbody>();

        if (rb != null)
        {
            DeleteBullete deleteBullete = Bullete.GetComponent<DeleteBullete>();
            //deleteBullete.Owner = gameObject;
            Vector3 forward = transform.forward;

            Vector3 up = transform.up;

            rb.AddForce((forward * deleteBullete.power*(oldJoyStick.magnitude*10) + up*deleteBullete.uppower), ForceMode.Impulse);
        }


        yield return new WaitForSeconds(interval);

        
    }
    [ServerRpc]
    void RotateServerRpc(Vector3 h_target) 
    {
        transform.rotation = Quaternion.LookRotation(h_target);
    }

    [ServerRpc]
    void BulleteServerRpc(int i,Vector3 h_target,ulong shooterID) 
    {
        Vector3 firestart = firepoint.position;

        GameObject Bullete = Instantiate(bullete[0], firestart, transform.rotation);//ターゲット方向に弾を生成して発射する

        var netObj=  Bullete.GetComponent<NetworkObject>();

        netObj.Spawn();

        Rigidbody rb = Bullete.GetComponent<Rigidbody>();

        DeleteBullete deleteBullete = Bullete.GetComponent<DeleteBullete>();

        deleteBullete.Owner = gameObject;
        Debug.Log("発射ID" + shooterID);
        deleteBullete.OwnerID.Value = shooterID;
        if (rb != null)
        {
            Vector3 right = transform.right;

            // 右方向に散らす
            float offset = (i - (bulleteCount[0] - 1) / 2f) * 0.2f;
            Vector3 spreadDir = (h_target + right * offset).normalized;

            rb.AddForce(spreadDir * deleteBullete.power, ForceMode.Impulse);

            Debug.Log("speedDir" + spreadDir + "power" + deleteBullete.power);
        }
    }

    [ServerRpc]
    void Assault_BulleteServerRpc(int i, float sideOffset,ulong shooterID)
    {
        isShot = true;
        Vector3 firestart = firepoint.position;
        Vector3 BulleteSpawn = transform.right * sideOffset * (i % 2 == 0 ? 1 : -1);

        GameObject Bullete = Instantiate(bullete[1], firestart + BulleteSpawn, transform.rotation);
        var netObj = Bullete.GetComponent<NetworkObject>();

        
        Rigidbody rb = Bullete.GetComponent<Rigidbody>();

        if (rb != null)
        {
            
            DeleteBullete deleteBullete = Bullete.GetComponent<DeleteBullete>();
            //deleteBullete.Owner = gameObject;
            //Debug.Log("発射ID"+shooterID);
            deleteBullete.OwnerID.Value = shooterID;
            netObj.Spawn();
            Vector3 forward = transform.forward;
            rb.AddForce(forward * deleteBullete.power, ForceMode.Impulse);
        }
    }

    [ServerRpc]
    void IsShotCancelServerRpc() 
    {
        isShot= false;
    }


}
