#region Using

using UnityEngine;
using System.Collections;

#endregion Using

public class SplashGui : MonoBehaviour
{
    #region Publics

    public void GotoMainScene()
    { StartCoroutine(WaitAndGotoMainScene()); }

    #endregion Publics

    #region Privates

    private IEnumerator WaitAndGotoMainScene()
    {
        yield return new WaitForSeconds(1f);
        Application.LoadLevel("MainScene");
    }

    #endregion Privates
}
