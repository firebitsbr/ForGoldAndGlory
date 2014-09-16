#region Using

using UnityEngine;
using System.Collections;

#endregion Using

public class Enemy : MonoBehaviour
{
    #region Members

    public bool IsAlive
    { get { return this.health > 0f; } }
    public Vector3 Position
    { get { return this.localTransform.position; } }

    public GameObject HealthbarObj;
    public GameObject BulletObj;
    public Transform AimSpot;
    public Transform FireSpot;
    public float HealthbarOffset = 1f;

    public float MaxHealth = 20f;
    public float AttackSpeed = 1f;
    public float Damage = 10f;
    public int Experience = 10;
    public int Gold = 10;

    private GameObject localObject;
    private Transform localTransform;

    private EnemyHealthbar healthBar;
    private float health = 20f;
    private float lastAttack = -1f;

    #endregion Members

    #region InitAndDestruction

    void Awake()
    {
        this.localObject = gameObject;
        this.localTransform = transform; 
    }

    void OnDestroy()
    {
        this.localObject = null;
        this.localTransform = null;

        if (this.healthBar != null)
        { Destroy(this.healthBar.gameObject); }
    }

    #endregion InitAndDestruction

    #region UnityFunctions

    void Start()
    {
        this.health = this.MaxHealth;
        this.lastAttack = Time.time;

        SpawnHealthbar();
    }

    #endregion UnityFunctions

    #region Publics

    public void TakeDamage(float damage)
    {
        this.health -= damage;
        this.healthBar.UpdateBar(this.health, this.MaxHealth);

        if (health <= 0f)
        { Die(); }
    }

    public bool IsReadyToAttack()
    { return Time.time - this.lastAttack >= this.AttackSpeed; }

    public void Attack()
    {
        GameObject go = Instantiate(this.BulletObj, Vector3.zero, Quaternion.identity) as GameObject;
        go.GetComponent<Bullet>().Initialize(this.localObject, this.FireSpot.position, Player.Instance.AimSpot.position);

        this.lastAttack = Time.time;
    }

    #endregion Publics

    #region Privates

    private void SpawnHealthbar()
    {
        GameObject obj = MainGui.Instance.SpawnFollowObj(this.HealthbarObj, transform, Vector3.up * this.HealthbarOffset);
        this.healthBar = obj.GetComponent<EnemyHealthbar>();
    }

    private void Die()
    { BattleManager.Instance.EnemyHasDied(); }

    #endregion Privates

    #region Events

    private void BulletHasReachedTarget()
    { 
        Player.Instance.TakeDamage(this.Damage);
        this.lastAttack = Time.time;

        BattleManager.Instance.EnemyAttackIsOver();
    }

    #endregion Events
}
