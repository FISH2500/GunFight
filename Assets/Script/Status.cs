using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Status : NetworkBehaviour
{
    [SerializeField] private Image HPBar;

    [SerializeField]float MaxHP;
    //[SerializeField] float HP;
    [SerializeField] float HealTime;
    [SerializeField] GameObject EnemyCanva;

    private NetworkVariable<float> HP = new NetworkVariable<float>(
       4000f,
       NetworkVariableReadPermission.Everyone,
       NetworkVariableWritePermission.Server
   );

    private Coroutine Heal;



    public override void OnNetworkSpawn()
    {
        // 変更があったら HPBar を更新
        HP.OnValueChanged += OnHPChanged;

        // スポーン時にも UI を更新
        OnHPChanged(HP.Value, HP.Value);
    }

    private void OnHPChanged(float oldValue, float newValue)
    {
        // HPバー更新（全クライアント）
        HPBar.fillAmount = newValue / MaxHP;

        // HP 0 のときはクライアントでもキャンバス削除
        if (newValue <= 0)
        {
            if (EnemyCanva != null)
                Destroy(EnemyCanva);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (!IsOwner) return;

        DeleteBullete bullete = other.GetComponent<DeleteBullete>();
        //
        Debug.Log("clientID:bullete.owner"+ OwnerClientId + ":" + bullete.OwnerID.Value);

        

        if (other.CompareTag("Bullete") &&OwnerClientId!=bullete.OwnerID.Value)//弾を打った人物じゃない場合
        {
            if (IsServer)
            {
                // ダメージの反映はサーバー
                ApplyDamage(bullete.Damage);
            }
            else
            {
                // サーバーにリクエストを送る
                ApplyDamageServerRpc(bullete.Damage);
            }

            // 弾を消す
            other.GetComponent<NetworkObject>().Despawn();
        }
    }

    // サーバー側で HP を減らす
    [ServerRpc]
    private void ApplyDamageServerRpc(float dmg)
    {
        ApplyDamage(dmg);
    }

    private void ApplyDamage(float dmg)
    {
        HP.Value -= dmg;

        if (HP.Value <= 0)
        {
            HP.Value = 0;
            Die();
        }

        // 回復ループ開始
        if (Heal != null) StopCoroutine(Heal);
        Heal = StartCoroutine(ReHeal());
    }

    private IEnumerator ReHeal()
    {
        yield return new WaitForSeconds(HealTime);

        while (HP.Value < MaxHP)
        {
            HP.Value += MaxHP * 0.13f;
            if (HP.Value > MaxHP) HP.Value = MaxHP;

            yield return new WaitForSeconds(HealTime);
        }
    }

    // 死亡処理（サーバーのみ）
    private void Die()
    {
        // 破壊はサーバー管理
        if (IsServer)
        {
            GetComponent<NetworkObject>().Despawn();
        }

        GameManager.instance.Finish();

    }
}
