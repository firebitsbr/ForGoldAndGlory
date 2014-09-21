#region Using

using UnityEngine;
using System.Collections;

#endregion Using

public class TravelGui : MonoBehaviour
{
    #region Members

    public UIScrollBar Scrollbar;

    #endregion Members

    #region UnityFunctions

    void Update()
    { UpdateScroll(); }

    #endregion UnityFunctions

    #region Publics

    public void OnPressingPlanetOne()
    { GameMaster.Instance.SwitchPlanet(0); }

    public void OnPressingPlanetTwo()
    { GameMaster.Instance.SwitchPlanet(1); }

    #endregion Publics

    #region Privates

    private void UpdateScroll()
    { this.Scrollbar.value -= Input.GetAxis("Mouse ScrollWheel"); }

    #endregion Privates
}