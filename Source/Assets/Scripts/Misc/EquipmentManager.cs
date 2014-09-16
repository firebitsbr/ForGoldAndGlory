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
}
