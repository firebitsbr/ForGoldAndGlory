#region Using

using UnityEngine;
using System.Collections;

#endregion Using

public class EnemyHealthbar : MonoBehaviour
{
    #region Members

    public UISprite Healthbar;

    private UIWidget mainWidget;
    private float barwidth = 0f;

    #endregion Members

    #region InitAndDestruction

    void Awake()
    { this.mainWidget = GetComponent<UIWidget>(); }

    #endregion InitAndDestruction

    #region UnityFunctions

    void Start()
    { this.barwidth = this.Healthbar.width; }

    #endregion UnityFunctions

    #region Publics

    public void UpdateBar(float health, float maxHealth)
    { 
        this.Healthbar.width = Mathf.RoundToInt((health / maxHealth) * this.barwidth);
        if (this.mainWidget.alpha == 0f)
        { this.mainWidget.alpha = 1f; }
    }

    #endregion Publics
}
