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
    public UIScrollBar Scrollbar;

    private EquipmentManager.ItemCategory currentCategory = EquipmentManager.ItemCategory.Armor;
    private List<GameObject> listOfWeapons = new List<GameObject>();
    private List<GameObject> listOfArmors = new List<GameObject>();
    private List<GameObject> listOfBoots = new List<GameObject>();
    private List<GameObject> listOfHelmets = new List<GameObject>();
    private Item hoveredItem;

    #endregion Members

    #region UnityFunctions

    void Start()
    { 
        SpawnItems();
        SwitchItemCategory(EquipmentManager.ItemCategory.Weapon);
    }

    void Update()
    { UpdateScroll(); }

    #endregion UnityFunctions

    #region Publics

    public void ShowToolTip(Item item)
    {
        this.hoveredItem = item;

        this.TooltipContainer.alpha = 1f;
        if (this.currentCategory == EquipmentManager.ItemCategory.Weapon)
        {
            this.TooltipHeader.text = Player.Instance.CurrentWeapon.ItemName;
            this.TooltipIcon.spriteName = "DamageIcon";
            SetTooltipInfo(Player.Instance.CurrentWeapon.Damage, (this.hoveredItem as Weapon).Damage);
        }
        else if (this.currentCategory == EquipmentManager.ItemCategory.Armor)
        {
            this.TooltipHeader.text = Player.Instance.CurrentArmor.ItemName;
            this.TooltipIcon.spriteName = "ArmorIcon";
            SetTooltipInfo(Player.Instance.CurrentArmor.ArmorValue, (this.hoveredItem as Armor).ArmorValue);
        }
        else if (this.currentCategory == EquipmentManager.ItemCategory.Boots)
        {
            this.TooltipHeader.text = Player.Instance.CurrentBoots.ItemName;
            this.TooltipIcon.spriteName = "ArmorIcon";
            SetTooltipInfo(Player.Instance.CurrentBoots.ArmorValue, (this.hoveredItem as Armor).ArmorValue);
        }
        else if (this.currentCategory == EquipmentManager.ItemCategory.Helmet)
        {
            this.TooltipHeader.text = Player.Instance.CurrentHelmet.ItemName;
            this.TooltipIcon.spriteName = "ArmorIcon";
            SetTooltipInfo(Player.Instance.CurrentHelmet.ArmorValue, (this.hoveredItem as Armor).ArmorValue);
        }
    }

    public void HideToolTip()
    { this.TooltipContainer.alpha = 0f; }

    public void OnPressingWeaponBtn()
    { SwitchItemCategory(EquipmentManager.ItemCategory.Weapon); }

    public void OnPressingHelmetBtn()
    { SwitchItemCategory(EquipmentManager.ItemCategory.Helmet); }

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

        Armor[] helmets = EquipmentManager.Instance.ArrayOfHelmets;
        for (int i = 0; i < helmets.Length; i++)
        { this.listOfHelmets.Add(SpawnItem(i, helmets[i], EquipmentManager.ItemCategory.Helmet)); }
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

    private void UpdateScroll()
    { this.Scrollbar.value -= Input.GetAxis("Mouse ScrollWheel"); }

    private void SwitchItemCategory(EquipmentManager.ItemCategory newCategory)
    {
        if (this.currentCategory == newCategory)
        { return; }

        this.currentCategory = newCategory;
        this.Scrollbar.value = 0f;

        for (int i = 0; i < this.listOfWeapons.Count; i++)
        { this.listOfWeapons[i].SetActive(this.currentCategory == EquipmentManager.ItemCategory.Weapon); }
        for (int i = 0; i < this.listOfArmors.Count; i++)
        { this.listOfArmors[i].SetActive(this.currentCategory == EquipmentManager.ItemCategory.Armor); }
        for (int i = 0; i < this.listOfBoots.Count; i++)
        { this.listOfBoots[i].SetActive(this.currentCategory == EquipmentManager.ItemCategory.Boots); }
        for (int i = 0; i < this.listOfBoots.Count; i++)
        { this.listOfHelmets[i].SetActive(this.currentCategory == EquipmentManager.ItemCategory.Helmet); }
    }

    private void SetTooltipInfo(float current, float hovered)
    {
        float difference = hovered - current;

        if (difference > 0f)
        { this.TooltipInfo.text = current.ToString("N0") + " [00FF00]+" + difference + "[-]"; }
        else if (difference < 0f)
        { this.TooltipInfo.text = current.ToString("N0") + " [FF0000]" + difference + "[-]"; }
        else
        { this.TooltipInfo.text = current.ToString("N0"); }       
    }

    #endregion Privates
}