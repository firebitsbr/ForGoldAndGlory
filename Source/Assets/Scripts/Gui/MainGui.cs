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
    public UILabel PlayerHaveDiedLabel;
    public GameObject FeedbackCenterAnchor;
    public GameObject BoughtItemContainer;
    public UILabel BoughtItemLabel;

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

    public void ShowPlayerHaveDied(int expLoss)
    {
        this.PlayerHaveDiedLabel.text = "YOU HAVE DIED!\nYOU LOST " + expLoss + " EXP!";

        TweenAlpha.Begin(this.PlayerHaveDiedContainer, 0.2f, 1f);
        StartCoroutine(FadeAlphaOverTime(this.PlayerHaveDiedContainer, 2f, 0f, 0.5f));
    }

    public void ShowBoughtItem(string itemName)
    {
        this.BoughtItemLabel.text = "You have bought\n" + itemName;
        TweenAlpha.Begin(this.BoughtItemContainer, 0.2f, 1f);
        StartCoroutine(FadeAlphaOverTime(this.BoughtItemContainer, 2f, 0f, 0.5f));
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