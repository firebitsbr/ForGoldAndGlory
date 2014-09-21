#region Using

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#endregion Using

public class GameMaster : MonoBehaviour
{
    #region Statics

    public static GameMaster Instance
    { 
        get 
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("GameMaster");
                instance = obj.AddComponent<GameMaster>();
            }
            return instance;
        } 
    }

    private static GameMaster instance;

    #endregion Statics

    #region Members

    private List<GameObject> listOfListeners = new List<GameObject>();

    #endregion Members

    #region Publics

    public void AddListener(GameObject obj)
    { this.listOfListeners.Add(obj); }

    public void SwitchArea(int areaId)
    { SendMsg("OnSwitchingArea", areaId); }

    #endregion Publics

    #region Privates

    private void SendMsg(string message, object obj)
    {
        for (int i = 0; i < this.listOfListeners.Count; i++)
        { this.listOfListeners[i].SendMessage(message, obj, SendMessageOptions.DontRequireReceiver); }
    }

    #endregion Privates
}
