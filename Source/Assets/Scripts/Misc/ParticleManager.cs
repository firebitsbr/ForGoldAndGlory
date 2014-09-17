#region Using

using UnityEngine;
using System.Collections;

#endregion Using

public class ParticleManager : MonoBehaviour
{
    #region Enums

    public enum Particle
    { 
        Blood,      // 0
        Slime       // 1
    }

    #endregion Enums

    #region Statics

    public static ParticleManager Instance
    { get { return instance; } }
    private static ParticleManager instance;

    #endregion Statics

    #region Members

    public GameObject[] Particles;

    #endregion Members

    #region InitAndDestruction

    void Awake()
    { instance = this; }

    void OnDestroy()
    { instance = null; }

    #endregion InitAndDestruction

    #region Publics

    public void SpawnParticle(Particle particle, Vector3 position, float ttl)
    { 
        GameObject go = Instantiate(this.Particles[(int)particle], position, Quaternion.identity) as GameObject;
        Destroy(go, ttl);
        go.GetComponent<Renderer>().sortingOrder = 10;
    }

    #endregion Publics
}
