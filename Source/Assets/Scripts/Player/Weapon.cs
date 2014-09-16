#region Using

using UnityEngine;
using System.Collections;

#endregion Using

public class Weapon : Item
{
    #region Members

    public float AttackSpeed = 1f;
    public float Damage = 10f;
    public GameObject BulletObj;

    private Transform spawnedBullet;

    #endregion Members

    #region Publics

    public void UseWeapon(Enemy enemy)
    {
        GameObject go = Instantiate(this.BulletObj, Vector3.zero, Quaternion.identity) as GameObject;
        go.GetComponent<Bullet>().Initialize(Player.Instance.gameObject, Player.Instance.Position, enemy.AimSpot.position);
    }

    #endregion Publics
}