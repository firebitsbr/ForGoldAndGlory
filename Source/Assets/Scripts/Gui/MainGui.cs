#region Using

using UnityEngine;
using System.Collections;

#endregion Using

public class MainGui : MonoBehaviour
{
    #region Enums

    private enum GuiState
    { 
        PlayerInfo, // 0
        Shop,       // 1
        Travel      // 2
    }

    #endregion Enums

    #region Statics

    public static MainGui Instance
    { get { return instance; } }
    private static MainGui instance;

    #endregion Statics

    #region Members

    public GameObject PlayerInfoContainer;
    public GameObject ShopContainer;
    public GameObject TravelContainer;
    public GameObject LevelupContainer;
    public GameObject PlayerHaveDiedContainer;
    public GameObject FeedbackCenterAnchor;

    private GuiState currentState = GuiState.PlayerInfo;

    #endregion Members

    #region InitAndDestruction

    void Awake()
    { instance = this; }

    void OnDestroy()
    { instance = null; }

    #endregion InitAndDestruction

    #region Publics

    public void OnPressingPlayerInfo()
    { SwitchState(GuiState.PlayerInfo); }

    public void OnPressingShop()
    { SwitchState(GuiState.Shop); }

    public void OnPressingTravel()
    { SwitchState(GuiState.Travel); }

    public void ShowLevelUp()
    {
        TweenAlpha.Begin(this.LevelupContainer, 0.2f, 1f);
        StartCoroutine(FadeAlphaOverTime(this.LevelupContainer, 1f, 0f, 0.5f));
    }

    public void ShowPlayerHaveDied()
    {
        TweenAlpha.Begin(this.PlayerHaveDiedContainer, 0.2f, 1f);
        StartCoroutine(FadeAlphaOverTime(this.PlayerHaveDiedContainer, 1f, 0f, 0.5f));
    }

    public GameObject SpawnFollowObj(GameObject obj, Transform objToFollow, Vector3 offset)
    {
        GameObject spawnedObj = NGUITools.AddChild(this.FeedbackCenterAnchor, obj);
        UIFollowTarget target  = spawnedObj.GetComponent<UIFollowTarget>();
        target.uiCamera = UICamera.currentCamera;
        target.gameCamera = Camera.main;
        target.target = objToFollow;
        target.Offset = offset;

        return spawnedObj;
    }

    #endregion Publics

    #region Privates

    private void SwitchState(GuiState newState)
    {
        if (newState == this.currentState)
        { return; }

        this.currentState = newState;

        this.PlayerInfoContainer.SetActive(this.currentState == GuiState.PlayerInfo);
        this.ShopContainer.SetActive(this.currentState == GuiState.Shop);
        this.TravelContainer.SetActive(this.currentState == GuiState.Travel);
    }

    private IEnumerator FadeAlphaOverTime(GameObject obj, float waitingTime, float alpha, float speed)
    {
        yield return new WaitForSeconds(waitingTime);
        TweenAlpha.Begin(obj, speed, alpha);
    }

    #endregion Privates
}