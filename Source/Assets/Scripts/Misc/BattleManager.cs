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
    private int planetId = 0;

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

        this.currentState = BattleState.Waiting;
    }

    public void EnemyHasDied()
    {
        Player.Instance.AddExperience(this.currentEnemy.Experience);
        Player.Instance.ModifyGold(this.currentEnemy.Gold);
        Destroy(this.currentEnemy.gameObject);
        this.currentEnemy = null;

        this.currentState = BattleState.Waiting;
    }

    public void PlayerIsReady()
    { this.currentState = BattleState.DecidingTurn; }

    public void PlayerAttackIsOver()
    { this.currentState = BattleState.PlayerHasAttacked; }

    public void EnemyAttackIsOver()
    { this.currentState = BattleState.EnemyHasAttacked; }

    #endregion Publics

    #region Privates

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
        this.currentEnemy = Instantiate(this.listOfEnemies[this.planetId], this.SpawnPoint.position, Quaternion.identity) as Enemy;
        this.isSpawningEnemy = false;

        this.currentState = BattleState.DecidingTurn;
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
        this.currentState = BattleState.WaitingForPlayer;
        Player.Instance.Attack();
    }

    private void LetEnemyAttack()
    {
        this.currentState = BattleState.WaitingForEnemy;
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

    void OnSwitchingPlanet(int planetId)
    {
        DestroyEnemy();
        this.planetId = planetId;
    }

    #endregion Events
}