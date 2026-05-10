using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    private enum EnemyType { ShotGun,Assault,Throw};

    private enum EnemyState { Idle, Attack, Run };

    [SerializeField] private EnemyType enemyType;

    [SerializeField] private Transform targetPlayer;

    [SerializeField] private LayerMask targetLayerMask;

    [SerializeField]float attackDistance;

    [SerializeField] float idleDistance;

    [SerializeField] float walkDistance;

    [SerializeField]
    int[] bulleteCount;

    [SerializeField]
    private Transform firepoint;

    [SerializeField]
    private GameObject[] bullete;

    [SerializeField] private float ChargeRate;

    [SerializeField] private float speed;

    [SerializeField] private float fireRate;

    [SerializeField] private Animator animator;


    public float reloadTime = 0.8f;

    private Coroutine reCharge;


    EnemyState state;

    Vector3 targetQ;

    Vector3 dir;

    float Gage;
    [SerializeField]
    float interavl;

    bool isShot=false;

    bool isReload=true;

    bool isIdle;

    bool isRun;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (targetPlayer == null) return;


        if (Gage < 0.9f && isReload)//ゲージが満タンじゃないかつ発射後0.8s後に回復するようにクールタイムを設ける 
        {
            Gage += ChargeRate * Time.deltaTime;

            if (Gage > 0.9f) Gage = 0.9f;
        }
        if (Gage <= 0f) 
        {
            Debug.Log("ガス欠");
            isRun = true;
        }
        else 
        {
            isRun = false;
        }
        Enemy_ShotGun_Move();
        //Debug.Log("Inter:Time" + interavl+":"+Time.time);


    }

    void Enemy_ShotGun_Move() 
    {
        switch (state)
        {
            case EnemyState.Idle:

                break;

            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Run:

                break;
        }
        dir = targetPlayer.position - transform.position;
        dir.y = 0;
        float distance=Vector3.Distance(targetPlayer.position, transform.position);
        Vector3 move = dir.normalized * speed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(dir);

        if (distance < attackDistance)//攻撃状態 
        {
            state = EnemyState.Attack;
        }
        //if(distance<idleDistance)//距離が近すぎたら止まる
        //{
        //    isIdle = true;
        //}

        if (!isRun) 
        {
            transform.Translate(move, Space.World);
        }

        if (isRun) 
        {
            transform.rotation = Quaternion.LookRotation(-dir);
            transform.Translate(-move, Space.World);
        }




    }

    void Shot_ShotGun()
    {
        Gage -= 0.3f;
        isReload = false;
        //fireRate = 0;
        interavl = Time.time+fireRate;
        for (int i = 0; i < bulleteCount[0]; i++)
        {
            Vector3 firestart = firepoint.position;

            GameObject Bullete = Instantiate(bullete[0], firestart, firepoint.rotation);
            Rigidbody rb = Bullete.GetComponent<Rigidbody>();
            DeleteBullete deleteBullete = Bullete.GetComponent<DeleteBullete>();
            deleteBullete.Owner = gameObject;
            if (rb != null)
            {

                Vector3 right = transform.right;

                // 右方向に散らす
                float offset = (i - (bulleteCount[0] - 1) / 2f) * 0.2f;

                Vector3 spreadDir = (dir.normalized + right * offset).normalized;

                //rb.AddForce(spreadDir * deleteBullete.power, ForceMode.Impulse);
            }
        }



        
    }

    private IEnumerator ReChargeBullete()
    {

        yield return new WaitForSeconds(reloadTime);//2秒後に処理
        Reload();

    }

    void Reload()
    {
        isReload = true;
    }

    void Attack() 
    {


        transform.rotation = Quaternion.LookRotation(dir);
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 5f);



        if (0.29f <= Gage && Time.time >= interavl)//攻撃するときの処理 
        {
            animator.SetBool("isForwardShot", true);
            switch (enemyType)
            {
                case EnemyType.ShotGun:

                    Shot_ShotGun();
                    break;
                case EnemyType.Assault:
                    if (!isShot) 
                    {
                        StartCoroutine(AssaultBurst());
                    }
                    break;
                case EnemyType.Throw:

                    break;
            }

            if (reCharge != null)//もし前のこるーちんが処理中の場合ストップする 
            {
                StopCoroutine(reCharge);
            }
            reCharge = StartCoroutine(ReChargeBullete());//新しくこるーちんの処理を開始する
        }
    }

    private IEnumerator AssaultBurst()
    {
        isShot = true;
        Gage -= 0.3f;

        int shots = 6;                  // 撃つ回数
        float shotinterval = 0.1f;          // 発射間隔（秒）
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
                deleteBullete.Owner = gameObject;
                Vector3 forward = transform.forward;
                //rb.AddForce(forward * deleteBullete.power, ForceMode.Impulse);
            }

            yield return new WaitForSeconds(shotinterval); // ← 間隔を空ける
        }

        isShot = false;
    }



}
