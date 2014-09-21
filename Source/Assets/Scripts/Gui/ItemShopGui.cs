#region Using

using UnityEngine;
using System.Collections;

#endregion Using

public class ItemShopGui : MonoBehaviour
{
    #region Members

    public UILabel NameLabel;
    public UILabel PriceLabel;
    public UILabel InfoLabel;
    public UISprite InfoIcon;

    private ShopGui owner;
    private Item item;
    private EquipmentManager.ItemCategory itemCategory;

    #endregion Members

    #region Publics

    public void SetItemInfo(ShopGui owner, Item item, EquipmentManager.ItemCategory itemCategory)
    {
        this.NameLabel.text = item.ItemName;
        this.PriceLabel.text = item.Cost.ToString("N0");
        if (itemCategory == EquipmentManager.ItemCategory.Weapon)
        { 
            this.InfoLabel.text = (item as Weapon).Damage.ToString("N0");
            this.InfoIcon.spriteName = "DamageIcon";
        }
        else
        { 
            this.InfoLabel.text = (item as Armor).ArmorValue.ToString("N0");
            this.InfoIcon.spriteName = "ArmorIcon";
        }

        this.owner = owner;
        this.item = item;
        this.itemCategory = itemCategory;
    }

    public void BuyItem()
    {
        if (!Player.Instance.HasEnoughGold(this.item.Cost))
        { return; }

        Player.Instance.ModifyGold(-this.item.Cost);
        Player.Instance.UpdateItem(this.item, this.itemCategory);
        MainGui.Instance.ShowBoughtItem(this.item.ItemName);

        this.owner.ShowToolTip(this.item);
    }

    public void ShowToolTips()
    { this.owner.ShowToolTip(this.item); }

    public void HideToolTips()
    { this.owner.HideToolTip(); }

    #endregion Publics
}
