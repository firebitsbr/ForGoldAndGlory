#region Using

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#endregion Using

public class ShopGui : MonoBehaviour
{
    #region Members

    public GameObject ItemObj;
    public Transform ItemContainer;
    public UIWidget TooltipContainer;
    public UILabel TooltipHeader;
    public UILabel TooltipInfo;
    public UISprite TooltipIcon;

    private EquipmentManager.ItemCategory currentCategory = EquipmentManager.ItemCategory.Armor;
    private List<GameObject> listOfWeapons = new List<GameObject>();
    private List<GameObject> listOfArmors = new List<GameObject>();
    private List<GameObject> listOfBoots = new List<GameObject>();

    #endregion Members

    #region UnityFunctions

    void Start()
    { 
        SpawnItems();
        SwitchItemCategory(EquipmentManager.ItemCategory.Weapon);
    }

    #endregion UnityFunctions

    #region Publics

    public void ShowToolTip()
    {
        this.TooltipContainer.alpha = 1f;
        if (this.currentCategory == EquipmentManager.ItemCategory.Weapon)
        {
            this.TooltipHeader.text = Player.Instance.CurrentWeapon.ItemName;
            this.TooltipInfo.text = Player.Instance.CurrentWeapon.Damage.ToString("N0");
            this.TooltipIcon.spriteName = "DamageIcon";
        }
        else if (this.currentCategory == EquipmentManager.ItemCategory.Armor)
        {
            this.TooltipHeader.text = Player.Instance.CurrentArmor.ItemName;
            this.TooltipInfo.text = Player.Instance.CurrentArmor.ArmorValue.ToString("N0");
            this.TooltipIcon.spriteName = "ArmorIcon";
        }
        else if (this.currentCategory == EquipmentManager.ItemCategory.Boots)
        {
            this.TooltipHeader.text = Player.Instance.CurrentBoots.ItemName;
            this.TooltipInfo.text = Player.Instance.CurrentBoots.ArmorValue.ToString("N0");
            this.TooltipIcon.spriteName = "ArmorIcon";
        }
    }

    public void HideToolTip()
    { this.TooltipContainer.alpha = 0f; }

    public void OnPressingWeaponBtn()
    { SwitchItemCategory(EquipmentManager.ItemCategory.Weapon); }

    public void OnPressingArmorBtn()
    { SwitchItemCategory(EquipmentManager.ItemCategory.Armor); }

    public void OnPressingBootsBtn()
    { SwitchItemCategory(EquipmentManager.ItemCategory.Boots); }

    #endregion Publics

    #region Privates

    private void SpawnItems()
    {
        Weapon[] weapons = EquipmentManager.Instance.ArrayOfWeapons;
        for (int i = 0; i < weapons.Length; i++)
        { this.listOfWeapons.Add(SpawnItem(i, weapons[i], EquipmentManager.ItemCategory.Weapon)); }

        Armor[] armors = EquipmentManager.Instance.ArrayOfArmors;
        for (int i = 0; i < armors.Length; i++)
        { this.listOfArmors.Add(SpawnItem(i, armors[i], EquipmentManager.ItemCategory.Armor)); }

        Armor[] boots = EquipmentManager.Instance.ArrayOfBoots;
        for (int i = 0; i < boots.Length; i++)
        { this.listOfBoots.Add(SpawnItem(i, boots[i], EquipmentManager.ItemCategory.Boots)); }
    }

    private GameObject SpawnItem(int counter, Item item, EquipmentManager.ItemCategory itemCategory)
    {
        Vector3 spawnPos = Vector3.up * ((counter * -49) - 25f);
        GameObject go = Instantiate(this.ItemObj, Vector3.zero, Quaternion.identity) as GameObject;
        go.GetComponent<ItemShopGui>().SetItemInfo(this, item, itemCategory);
        go.transform.parent = this.ItemContainer;
        go.transform.localPosition = spawnPos;
        go.transform.localScale = Vector3.one;

        return go;
    }

    private void SwitchItemCategory(EquipmentManager.ItemCategory newCategory)
    {
        if (this.currentCategory == newCategory)
        { return; }

        this.currentCategory = newCategory;

        for (int i = 0; i < this.listOfWeapons.Count; i++)
        { this.listOfWeapons[i].SetActive(this.currentCategory == EquipmentManager.ItemCategory.Weapon); }
        for (int i = 0; i < this.listOfArmors.Count; i++)
        { this.listOfArmors[i].SetActive(this.currentCategory == EquipmentManager.ItemCategory.Armor); }
        for (int i = 0; i < this.listOfBoots.Count; i++)
        { this.listOfBoots[i].SetActive(this.currentCategory == EquipmentManager.ItemCategory.Boots); }
    }

    #endregion Privates
}