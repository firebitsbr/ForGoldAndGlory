#region Using

using UnityEngine;
using System.Collections;

#endregion Using

public class PlayerInfoGui : MonoBehaviour
{
    #region Statics

    public static PlayerInfoGui Instance
    { get { return instance; } }
    private static PlayerInfoGui instance;

    #endregion Statics

    #region Members

    public UILabel LevelLabel;
    public UISprite ExperienceBar;
    public UILabel HealthLabel;
    public UISprite Healthbar;
    public UILabel GoldLabel;
    public UISprite HealingCooldown;
    public UILabel DamageInfoLabel;
    public UILabel ArmorInfoLabel;

    private float experiencebarWidth = 0f;
    private float healthbarWidth = 0f;

    #endregion Members

    #region InitAndDestruction

    void Awake()
    { instance = this; }

    void OnDestroy()
    { instance = null; }

    #endregion InitAndDestruction

    #region UnityFunctions

    void Start()
    {
        this.experiencebarWidth = this.ExperienceBar.width;
        this.healthbarWidth = this.Healthbar.width; 
    }

    #endregion UnityFunctions

    #region Publics

    public void UpdateExperienceBar(float percentageLeft)
    { this.ExperienceBar.width = Mathf.RoundToInt(this.healthbarWidth * percentageLeft); }

    public void UpdateLevelLabel(int level)
    {  this.LevelLabel.text = "Lv. " + level; }

    public void UpdateHealthLabel(float health, float maxHealth)
    { 
        this.HealthLabel.text = Mathf.Max(health, 0).ToString("0"); 
        this.Healthbar.width = Mathf.RoundToInt((health / maxHealth) * this.healthbarWidth);
    }

    public void UpdateGoldLabel(long gold)
    { this.GoldLabel.text = gold.ToString("N0"); }

    public void UpdateHealBtn(float percentageLeft)
    { this.HealingCooldown.fillAmount = 1f - percentageLeft; }

    public void UpdateDamageLabel(float damage)
    { this.DamageInfoLabel.text = damage.ToString("N0"); }

    public void UpdateArmorLabel(int armor)
    { this.ArmorInfoLabel.text = armor.ToString("N0"); }

    public void OnPressingHealBtn()
    {
        this.HealingCooldown.fillAmount = 1f;
        Player.Instance.UseHealSkill();
    }

    #endregion Publics
}
