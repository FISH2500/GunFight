using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Shot : MonoBehaviour
{
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private GameObject[] bullete;
    [SerializeField] private Transform firepoint;
    [SerializeField] private Transform collidershape;
    [SerializeField] private float addRotateValue;
    [SerializeField] private float power;
    [SerializeField] private float[] bulleteCount;
    [SerializeField] private Image bulleteGage_Orange;
    [SerializeField] private Image bulleteGage_Gray;
    [SerializeField] private float Gage;
    [SerializeField] private Animator animator;

    public float reloadTime = 0.8f;  // リロードにかかる時間

    private Quaternion targetRotation;
    private Vector3 target;
    private Vector3 fanShapetarget;
    public bool isShot = false;
    bool isShotEnd=true;
    bool isReload=false;
    float vector = 1;
    private enum PlayerType  { ShotGun, Assault };

    [SerializeField] PlayerType player = new PlayerType();

    [SerializeField] private float ChargeRate;

    private Coroutine reCharge;

    void Start()
    {
        Gage = 0.9f;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Gage < 0.9f&&isReload)//ゲージが満タンじゃないかつ発射後0.8s後に回復するようにクールタイムを設ける 
        {
            Gage += ChargeRate * Time.deltaTime;

            if (Gage > 0.9f) Gage = 0.9f;
        }

        Vector2 dir = joystick.Direction;
        if (dir != Vector2.zero)//射撃ボタンを動かしている時
        {
            
            collidershape.gameObject.SetActive(true);
            float fanMoveX = joystick.Horizontal;
            float fanMoveZ = joystick.Vertical;


            fanShapetarget = new Vector3(fanMoveX, 0, fanMoveZ).normalized;

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

            if (dir != Vector2.zero)
            {
                float moveX = joystick.Horizontal;
                float moveZ = joystick.Vertical;


                target = new Vector3(moveX, 0, moveZ).normalized;
                isShotEnd = false;
            }

            if (dir == Vector2.zero && !isShotEnd)//Gageが0より多い場合 
            {
                animator.SetBool("isForwardShot", true);
                isReload = false;
                Debug.Log("攻撃");
                isShotEnd = true;
                //isShot = true;
                if (reCharge != null)//もし前のこるーちんが処理中の場合ストップする 
                {
                    StopCoroutine(reCharge);
                }
                reCharge = StartCoroutine(ReChargeBullete());//新しくこるーちんの処理を開始する
                transform.rotation = Quaternion.LookRotation(target);

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

            Vector3 firestart = firepoint.position;

            GameObject Bullete = Instantiate(bullete[0], firestart, transform.rotation);//ターゲット方向に弾を生成して発射する
            Rigidbody rb = Bullete.GetComponent<Rigidbody>();

            DeleteBullete deleteBullete = Bullete.GetComponent<DeleteBullete>();
            if (rb != null)
            {
                Vector3 right = transform.right;

                // 右方向に散らす
                float offset = (i - (bulleteCount[0] - 1) / 2f) * 0.2f;
                Vector3 spreadDir = (target + right * offset).normalized;

                rb.AddForce(spreadDir * deleteBullete.power, ForceMode.Impulse);
            }
        }
    }

    private IEnumerator ReChargeBullete() 
    {
        
        yield return new WaitForSeconds(reloadTime);//2秒後に処理
        Reload();

    }

    //void Shot_Assault() 
    //{
    //    Gage -= 0.3f;
    //    for (int i = 0; i < bulleteCount[1]; i++)
    //    {
    //        vector *= -1;
    //        Vector3 firestart = firepoint.position;

    //        Vector3 BulleteSpawn = transform.right * 0.5f * vector;

    //        GameObject Bullete = Instantiate(bullete[1], firestart+ BulleteSpawn, transform.rotation);//ターゲット方向に弾を生成して発射する
    //        Rigidbody rb = Bullete.GetComponent<Rigidbody>();

    //        DeleteBullete deleteBullete = Bullete.GetComponent<DeleteBullete>();
    //        if (rb != null)
    //        {
    //            Vector3 forward = transform.forward;

    //            // 右方向に散らす
    //            Vector3 offset = transform.right * 0.5f;


    //            rb.AddForce(forward * deleteBullete.power, ForceMode.Impulse);
    //        }
    //    }
    //}
    void Shot_Assault()
    {
        if (!isShot)
            StartCoroutine(AssaultBurst());
    }
    private IEnumerator AssaultBurst()
    {
        isShot = true;
        Gage -= 0.3f;

        int shots = 6;                  // 撃つ回数
        float interval = 0.1f;          // 発射間隔（秒）
        float sideOffset = 0.5f;        // 左右のズレ幅

        for (int i = 0; i < shots; i++)
        {
            Vector3 firestart = firepoint.position;
            Vector3 BulleteSpawn = transform.right * sideOffset * (i % 2 == 0 ? 1 : -1);

            GameObject Bullete = Instantiate(bullete[1], firestart + BulleteSpawn, transform.rotation);
            Rigidbody rb = Bullete.GetComponent<Rigidbody>();

            if (rb != null)
            {
                DeleteBullete deleteBullete = Bullete.GetComponent<DeleteBullete>();
                Vector3 forward = transform.forward;
                rb.AddForce(forward * deleteBullete.power, ForceMode.Impulse);
            }

            yield return new WaitForSeconds(interval); // ← 間隔を空ける
        }

        isShot = false;
    }
    private IEnumerator ShotDelay() 
    {
        yield return new WaitForSeconds(0.5f);



    }


}
