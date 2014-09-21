#region Using

using UnityEngine;
using System.Collections;

#endregion Using

public class EquipmentManager : MonoBehaviour
{
    #region Enums

    public enum ItemCategory
    {
        Weapon,     // 0
        Armor,      // 1
        Boots,      // 2
        Helmet      // 3
    }

    #endregion Enums

    #region Statics

    public static EquipmentManager Instance
    { get { return instance; } }
    private static EquipmentManager instance;

    #endregion Statics

    #region Members

    public Weapon[] ArrayOfWeapons;
    public Armor[] ArrayOfArmors;
    public Armor[] ArrayOfBoots;
    public Armor[] ArrayOfHelmets;

    #endregion Members

    #region InitAndDestruction

    void Awake()
    { instance = this; }

    void OnDestroy()
    { instance = null; }

    #endregion InitAndDestruction

    #region UnityFunctions

    void Start()
    { LoadItemCosts(); }

    #endregion UnityFunctions

    #region Privates

    private void LoadItemCosts()
    {
        SetItemCost(this.ArrayOfWeapons);
        SetItemCost(this.ArrayOfArmors);
        SetItemCost(this.ArrayOfHelmets);
        SetItemCost(this.ArrayOfBoots);
    }

    private void SetItemCost(Item[] arrayOfItems)
    {
        arrayOfItems[0].Cost = 100;
        arrayOfItems[1].Cost = 500;
        for (int i = 2; i < arrayOfItems.Length; i++)
        { arrayOfItems[i].Cost = (arrayOfItems[i - 1].Cost * 12); }    
    }

    #endregion Privates
}
