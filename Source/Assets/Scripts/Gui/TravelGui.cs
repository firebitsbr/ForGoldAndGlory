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

    public void OnPressingAreaOne()
    { GameMaster.Instance.SwitchArea(0); }

    public void OnPressingAreaTwo()
    { GameMaster.Instance.SwitchArea(1); }

    public void OnPressingAreaThree()
    { GameMaster.Instance.SwitchArea(2); }

    public void OnPressingAreaFour()
    { GameMaster.Instance.SwitchArea(3); }

    public void OnPressingAreaFive()
    { GameMaster.Instance.SwitchArea(4); }

    public void OnPressingAreaSix()
    { GameMaster.Instance.SwitchArea(5); }

    public void OnPressingAreaSeven()
    { GameMaster.Instance.SwitchArea(6); }

    public void OnPressingAreaEight()
    { GameMaster.Instance.SwitchArea(7); }

    public void OnPressingAreaNine()
    { GameMaster.Instance.SwitchArea(8); }

    public void OnPressingAreaTen()
    { GameMaster.Instance.SwitchArea(9); }

    #endregion Publics

    #region Privates

    private void UpdateScroll()
    { this.Scrollbar.value -= Input.GetAxis("Mouse ScrollWheel"); }

    #endregion Privates
}