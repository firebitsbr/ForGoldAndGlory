#region Using

using UnityEngine;
using System.Collections;

#endregion Using

public class BattleManager : MonoBehaviour
{
    #region Enums

    private enum BattleState
    { 
        Waiting,            // 0
        WaitingForEnemy,    // 1
        EnemyHasAttacked,   // 2
        WaitingForPlayer,   // 3
        PlayerHasAttacked,  // 4
        DecidingTurn        // 5
    }

    #endregion Enums

    #region Statics

    public static BattleManager Instance
    { get { return instance; } }
    private static BattleManager instance;

    #endregion Statics

    #region Members

    public Enemy[] listOfEnemies;
    public Transform SpawnPoint;

    private bool isSpawningEnemy = false;
    private Enemy currentEnemy;
    private float enemySpawnRate = 2f;
    private int areaId = 0;

    private BattleState currentState = BattleState.Waiting;

    #endregion Members

    #region InitAndDestruction

    void Awake()
    { instance = this; }

    void OnDestroy()
    { instance = null; }

    #endregion InitAndDestruction

    #region UnityFunctions

    void Start()
    { GameMaster.Instance.AddListener(gameObject); }

    void Update()
    { CheckOnBattle(); }

    #endregion UnityFunctions

    #region Publics

    public Enemy GetCurrentEnemy()
    { return this.currentEnemy; }

    public void PlayerHasDied()
    {
        if (this.currentEnemy == null)
        { return; }

        Destroy(this.currentEnemy.gameObject);
        this.currentEnemy = null;

        SwitchState(BattleState.Waiting);
    }

    public void EnemyHasDied()
    {
        Player.Instance.AddExperience(this.currentEnemy.Experience);
        Player.Instance.ModifyGold(this.currentEnemy.Gold);
        Destroy(this.currentEnemy.gameObject);
        this.currentEnemy = null;

        SwitchState(BattleState.Waiting);
    }

    public void PlayerIsReady()
    { SwitchState(BattleState.DecidingTurn); }

    public void PlayerAttackIsOver()
    { SwitchState(BattleState.PlayerHasAttacked); }

    public void EnemyAttackIsOver()
    { SwitchState(BattleState.EnemyHasAttacked); }

    #endregion Publics

    #region Privates

    private void SwitchState(BattleState newState)
    {
        if (this.currentState == newState)
        { return; }

        this.currentState = newState;
    }

    private void CheckOnBattle()
    {
        if ((this.currentEnemy == null || !this.currentEnemy.IsAlive) && !this.isSpawningEnemy)
        { StartCoroutine(SpawnNewEnemy()); }
        else if (this.currentEnemy != null && this.currentEnemy.IsAlive)
        { UpdateBattle(); }
    }

    private IEnumerator SpawnNewEnemy()
    {
        this.isSpawningEnemy = true;
        yield return new WaitForSeconds(this.enemySpawnRate);

        if (!Player.Instance.IsDead)
        {
            this.currentEnemy = Instantiate(this.listOfEnemies[this.areaId], this.SpawnPoint.position, Quaternion.identity) as Enemy;
            SwitchState(BattleState.DecidingTurn);
        }

        this.isSpawningEnemy = false;
    }

    private void UpdateBattle()
    {
        if (this.currentState == BattleState.Waiting)
        { return; }
        if (this.currentState == BattleState.WaitingForEnemy || this.currentState == BattleState.WaitingForPlayer)
        { return; }

        if (this.currentState == BattleState.DecidingTurn)
        {
            if (Player.Instance.IsReadyToAttack())
            { LetPlayerAttack(); }
            else if (this.currentEnemy.IsReadyToAttack())
            { LetEnemyAttack(); }
        }
        else if (this.currentState == BattleState.PlayerHasAttacked)
        {
            if (this.currentEnemy.IsReadyToAttack())
            { LetEnemyAttack(); }
            else if (Player.Instance.IsReadyToAttack())
            { LetPlayerAttack(); }
        }
        else if (this.currentState == BattleState.EnemyHasAttacked)
        {
            if (Player.Instance.IsReadyToAttack())
            { LetPlayerAttack(); }
            else if (this.currentEnemy.IsReadyToAttack())
            { LetEnemyAttack(); }
        }
    }

    private void LetPlayerAttack()
    {
        SwitchState(BattleState.WaitingForPlayer);
        Player.Instance.Attack();
    }

    private void LetEnemyAttack()
    {
        SwitchState(BattleState.WaitingForEnemy);
        this.currentEnemy.Attack();
    }

    private void DestroyEnemy()
    {
        if (this.currentEnemy == null)
        { return; }

        Destroy(this.currentEnemy.gameObject);
        this.currentEnemy = null;
    }

    #endregion Privates

    #region Events

    void OnSwitchingArea(int areaId)
    {
        DestroyEnemy();
        this.areaId = areaId;
    }

    #endregion Events
}