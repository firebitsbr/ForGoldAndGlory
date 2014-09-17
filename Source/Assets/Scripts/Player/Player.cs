#region Using

using UnityEngine;
using System.Collections;

#endregion Using

public class Player : MonoBehaviour
{
    #region Statics

    public static Player Instance
    { get { return instance; } }
    private static Player instance;

    #endregion Statics

    #region Members

    public int Level
    { get { return this.level; } }
    public int Experience
    { get { return this.experience; } }
    public Vector3 Position
    { get { return this.localTransform.position; } }

    public Weapon CurrentWeapon;
    public Armor CurrentHelmet;
    public Armor CurrentArmor;
    public Armor CurrentBoots;
    public Transform AimSpot;

    private Transform localTransform;
    private GameObject graphicsObject;
    private SpriteRenderer weaponRenderer;
    private SpriteRenderer helmetRenderer;
    private SpriteRenderer armorRenderer;
    private SpriteRenderer bootsRenderer;

    private int level = 1;
    private int experience = 0;
    private int gold = 0;
    private int totalArmorValue = 0;
    private float maxHealth = 100f;
    private float health = 100f;
    private bool isDead = false;
    private float lastAttack = -1f;
    private bool isHealAvailable = true;
    private float lastHeal = -4f;
    private float healCooldown = 300f;

    #endregion Members

    #region InitAndDestruction

    void Awake()
    { 
        instance = this;
        this.localTransform = transform;
    }

    void OnDestroy()
    { 
        instance = null;
        this.localTransform = null;
    }

    #endregion InitAndDestruction

    #region UnityFunctions

    void Start()
    {
        this.graphicsObject = this.localTransform.Find("Graphics").gameObject;
        this.weaponRenderer = this.localTransform.Find("Graphics/Weapon").GetComponent<SpriteRenderer>();
        this.helmetRenderer = this.localTransform.Find("Graphics/Helmet").GetComponent<SpriteRenderer>();
        this.armorRenderer = this.localTransform.Find("Graphics/Armor").GetComponent<SpriteRenderer>();
        this.bootsRenderer = this.localTransform.Find("Graphics/Boots").GetComponent<SpriteRenderer>();

        Respawn();
        UpdateItem(this.CurrentWeapon, EquipmentManager.ItemCategory.Weapon);
        UpdateItem(this.CurrentHelmet, EquipmentManager.ItemCategory.Helmet);
        UpdateItem(this.CurrentArmor, EquipmentManager.ItemCategory.Armor);
        UpdateItem(this.CurrentBoots, EquipmentManager.ItemCategory.Boots);

        PlayerInfoGui.Instance.UpdateExperienceBar(0f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Home))
        { ModifyGold(1000); }

        CheckupOnHeal();
    }

    #endregion UnityFunctions

    #region Publics

    public void TakeDamage(float damage)
    {
        damage = Mathf.Max(0f, damage - this.totalArmorValue);
        this.health -= damage;
        PlayerInfoGui.Instance.UpdateHealthLabel(this.health, this.maxHealth);
        ParticleManager.Instance.SpawnParticle(ParticleManager.Particle.Blood, this.AimSpot.position, 1f);

        if (health <= 0f && !this.isDead)
        { StartCoroutine(Die()); }
    }

    public void AddExperience(int experience)
    {
        this.experience += experience;
        CheckIfLevelUp();
    }

    public bool HasEnoughGold(int gold)
    { return this.gold >= gold; }

    public void ModifyGold(int gold)
    {
        this.gold += gold;
        PlayerInfoGui.Instance.UpdateGoldLabel(this.gold);
    }

    public void UpdateItem(Item item, EquipmentManager.ItemCategory category)
    {
        if (category == EquipmentManager.ItemCategory.Weapon)
        {
            this.CurrentWeapon = (item as Weapon);
            this.weaponRenderer.sprite = this.CurrentWeapon.Sprite;
        }
        else if (category == EquipmentManager.ItemCategory.Armor)
        { 
            this.CurrentArmor = (item as Armor);
            this.armorRenderer.sprite = this.CurrentArmor.Sprite;
        }
        else if (category == EquipmentManager.ItemCategory.Boots)
        { 
            this.CurrentBoots = (item as Armor);
            this.bootsRenderer.sprite = this.CurrentBoots.Sprite;
        }
        else if (category == EquipmentManager.ItemCategory.Helmet)
        { 
            this.CurrentHelmet = (item as Armor);
            this.helmetRenderer.sprite = this.CurrentHelmet.Sprite;
        }

        this.totalArmorValue = this.CurrentArmor.ArmorValue + this.CurrentBoots.ArmorValue + this.CurrentHelmet.ArmorValue;
        
        PlayerInfoGui.Instance.UpdateDamageLabel(this.CurrentWeapon.Damage);
        PlayerInfoGui.Instance.UpdateArmorLabel(this.totalArmorValue);
    }

    public bool IsReadyToAttack()
    { return Time.time - this.lastAttack >= this.CurrentWeapon.AttackSpeed; }

    public void Attack()
    {
        Enemy enemy = BattleManager.Instance.GetCurrentEnemy();
        if (enemy == null)
        { return; }

        this.CurrentWeapon.UseWeapon(enemy);
        this.lastAttack = Time.time;
    }

    public float GetPercentageToNextLevel()
    {
        int expToNextLevel = ExpNeededToLevel(this.level);
        if (this.level > 1)
        {
            int expForLastLevel = ExpNeededToLevel(this.level - 1);
            return (this.experience - expForLastLevel) / (float)(expToNextLevel - expForLastLevel);
        }
        return this.experience / (float)expToNextLevel;
    }

    public void UseHealSkill()
    {
        if (!this.isHealAvailable)
        { return; }

        this.lastHeal = Time.time;
        this.isHealAvailable = false;
        HealFully();
    }

    #endregion Publics

    #region Privates

    private void Respawn()
    {
        HealFully();
        this.isDead = false;
        BattleManager.Instance.PlayerIsReady();
    }

    private IEnumerator Die()
    {
        this.isDead = true;
        BattleManager.Instance.PlayerHasDied();
        MainGui.Instance.ShowPlayerHaveDied();

        this.graphicsObject.SetActive(false);

        yield return new WaitForSeconds(2f);

        this.graphicsObject.SetActive(true);

        Respawn();
    }

    private void CheckupOnHeal()
    {
        if (this.isHealAvailable)
        { return; }

        if (Time.time - this.lastHeal > this.healCooldown)
        { this.isHealAvailable = true; }

        float percentageLeft = (Time.time - this.lastHeal) / this.healCooldown;
        PlayerInfoGui.Instance.UpdateHealBtn(percentageLeft);
    }

    private void CheckIfLevelUp()
    {
        PlayerInfoGui.Instance.UpdateExperienceBar(GetPercentageToNextLevel());

        if (this.experience < ExpNeededToLevel(this.level))
        { return; }

        this.level++;

        PlayerInfoGui.Instance.UpdateLevelLabel(this.level);
        PlayerInfoGui.Instance.UpdateExperienceBar(0f);

        this.maxHealth += 10f;
        this.health = this.maxHealth;
        PlayerInfoGui.Instance.UpdateHealthLabel(this.health, this.maxHealth);

        MainGui.Instance.ShowLevelUp();
    }

    private int ExpNeededToLevel(int level)
    { return Mathf.RoundToInt(((50f * Mathf.Pow(level, 3f)) - (150f * Mathf.Pow(level, 2)) + (400f * level)) / 3f); }

    private void HealFully()
    {
        this.health = this.maxHealth;
        PlayerInfoGui.Instance.UpdateHealthLabel(this.health, this.maxHealth);
    }

    #endregion Privates

    #region Events

    private void BulletHasReachedTarget()
    {
        Enemy enemy = BattleManager.Instance.GetCurrentEnemy();
        if(enemy == null)
        { return; }

        enemy.TakeDamage(this.CurrentWeapon.Damage);
        this.lastAttack = Time.time;

        BattleManager.Instance.PlayerAttackIsOver(); 
    }

    #endregion Events
}