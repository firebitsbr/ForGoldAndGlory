#region Using

using UnityEngine;
using System.Collections;

#endregion Using

public class Bullet : MonoBehaviour
{
    #region Members

    private Transform localTransform;
    private GameObject reportObj;

    private bool isInitialized = false;
    private Vector3 startPos;
    private Vector3 endPos;
    private float startTime;
    private float speed = 2f;

    #endregion Members

    #region InitAndDestruction

    void Awake()
    { this.localTransform = transform; }

    void OnDestroy()
    { this.localTransform = null; }

    #endregion InitAndDestruction

    #region UnityFunctions

    void Update()
    { UpdatePosition(); }

    #endregion UnityFunctions

    #region Publics

    public void Initialize(GameObject reportObj, Vector3 startPos, Vector3 endPos)
    {
        this.startPos = startPos;
        this.endPos = endPos;
        this.startTime = Time.time;
        this.reportObj = reportObj;

        this.localTransform.position = this.startPos;
        this.isInitialized = true;
    }

    #endregion Publics

    #region Privates

    private void UpdatePosition()
    {
        if (!this.isInitialized)
        { return; }

        this.localTransform.position = Vector3.Lerp(this.startPos, this.endPos, (Time.time - this.startTime) * this.speed);
        this.localTransform.LookAt(this.endPos);

        if (this.reportObj == null)
        {
            DestroyObject();
            return;
        }

        if (this.localTransform.position == this.endPos)
        {
            this.reportObj.SendMessage("BulletHasReachedTarget", SendMessageOptions.DontRequireReceiver);
            DestroyObject();
        }
    }

    private void DestroyObject()
    { Destroy(gameObject); }

    #endregion Privates
}
