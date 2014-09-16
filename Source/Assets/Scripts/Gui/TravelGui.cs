#region Using

using UnityEngine;
using System.Collections;

#endregion Using

public class TravelGui : MonoBehaviour
{
    #region Publics

    public void OnPressingPlanetOne()
    { GameMaster.Instance.SwitchPlanet(0); }

    public void OnPressingPlanetTwo()
    { GameMaster.Instance.SwitchPlanet(1); }

    #endregion Publics
}