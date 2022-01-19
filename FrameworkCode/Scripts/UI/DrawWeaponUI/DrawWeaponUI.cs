using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DrawLine;

public class DrawWeaponUI : MonoBehaviour
{
    private Slider inkSlider;

    private Text inkText;
    /// <summary>
    /// 检测点的父物体
    /// </summary>
    private Transform checkNode;
    /// <summary>
    /// 检测点的List
    /// </summary>
    private DrawLineCheckPoint[] checkList = null;

    private Transform _doneBtn;
    private Transform _refreshBtn;
    private Transform _resetBtn;
    private Text _coinText;
    private Transform _adUpgradeTrans;
    private Transform _coinUpgradeTrans;
    
    private Text _textRefreshTrans;
    private Transform _adRefreshTrans;
    private Transform _coinRefreshTrans;
    
    private Text _textResetTrans;
    private Transform _adResetTrans;
    private Transform _coinResetTrans;
    
    private Text _LevelText;

    private int perSetId;
    /// <summary>
    /// 监测点预制体路径
    /// </summary>
    private readonly string checkNodePath = "Prefabs/Weapons/CheckNodes/Check";
    public void OnClose()
    {
        if (DrawLineManager.Instance.lineLength <= 1)
        {
            return;
        }
        
        CheckDrawResult();
        RemoveLineCollider();
        DrawLineManager.Instance.lineGroup.localScale = Vector3.one * 0.04f;
        
    }

    public void OnRefresh()
    {
        OnRefreshSuccess();
    }

    private void OnRefreshSuccess()
    {
        for(int i = DrawLineManager.Instance.lineGroup.childCount - 1; i >= 0; i--)
        {
            Destroy(DrawLineManager.Instance.lineGroup.GetChild(i).gameObject);
        }
        
        InitWeaponImage();
        DrawLineManager.Instance.RefreshLineLength();
    }

    public void OnReset()
    {
        OnResetSuccess();
    }

    private void OnResetSuccess()
    {
        for (int i = DrawLineManager.Instance.lineGroup.childCount - 1; i >= 0; i--)
        {
            Destroy(DrawLineManager.Instance.lineGroup.GetChild(i).gameObject);
        }

        DrawLineManager.Instance.RefreshLineLength();
    }
    
    /// <summary>
    /// 检测绘制结果
    /// </summary>
    private void CheckDrawResult()
    {
        int checkPoint = 0;
        for(int i = 0; i < checkList.Length; i++)
        {
            if (checkList[i].hasCheck)
            {
                checkPoint += 1;
            }
        }
        if(checkPoint >= checkList.Length / 2)
        {
            DrawLineManager.Instance.curWeaponId = perSetId;
        }
        else
        {
            //画线失败逻辑
            OnDrawFailed();
        }
    }
    /// <summary>
    /// 画线失败
    /// </summary>
    private void OnDrawFailed()
    {

    }
    /// <summary>
    /// 移除画线的2D碰撞器
    /// </summary>
    private void RemoveLineCollider()
    {
        Transform lineGroup = DrawLineManager.Instance.lineGroup;
        for (int i = 0; i < lineGroup.childCount; i++)
        {
            lineGroup.GetChild(i).GetComponent<LineRenderer>().startWidth =
                lineGroup.GetChild(i).GetComponent<LineRenderer>().endWidth = 0.1f;
            lineGroup.GetChild(i).gameObject.layer = LayerMask.NameToLayer("Default");
        }
        EdgeCollider2D[] colliders = lineGroup.GetComponentsInChildren<EdgeCollider2D>();
        for (int i = 0; i < colliders.Length; i++)
        {
            Destroy(colliders[i]);
        }
        Rigidbody2D[] rbs = lineGroup.GetComponentsInChildren<Rigidbody2D>();
        for (int i = 0; i < rbs.Length; i++)
        {
            Destroy(rbs[i]);
        }
    }
    /// <summary>
    /// 刷新墨水的显示
    /// </summary>
    private void RefreshInkShow()
    {
        float sub = Mathf.Clamp(DrawLineManager.Instance.maxLineLength - DrawLineManager.Instance.lineLength, 0, DrawLineManager.Instance.maxLineLength);
        inkSlider.value = sub / DrawLineManager.Instance.maxLineLength;
        inkText.text = (int)sub + "/" + (int)DrawLineManager.Instance.maxLineLength;
    }
    /// <summary>
    /// 初始化武器图片
    /// </summary>
    private void InitWeaponImage()
    {
        ///这里需要修改预设ID
        var id = perSetId = 5001;
        for (int i = checkNode.childCount - 1; i >= 0; i--)
        {
            Destroy(checkNode.GetChild(i).gameObject);
        }
        ///这里需要设置检测点预制体路径
        GameObject pfb = Resources.Load<GameObject>(checkNodePath + id);
        GameObject check = Instantiate(pfb, checkNode, false);
        check.transform.localPosition = Vector3.zero;
        checkList = check.GetComponentsInChildren<DrawLineCheckPoint>();
    }

    private void FindProperty()
    {
        inkSlider = transform.Find("InkNode").GetComponent<Slider>();
        inkText = transform.Find("InkNode/Number").GetComponent<Text>();
        checkNode = transform.Find("DrawArea/CheckNode");

        _doneBtn = transform.Find("Btns/Done");
        _refreshBtn = transform.Find("Btns/Refresh");
        _resetBtn = transform.Find("Btns/Reset");
        _coinText = transform.Find("Coin/Text").GetComponent<Text>();
        
        _adUpgradeTrans = transform.Find("DrawArea/UpgradeInk/Ad");
        _coinUpgradeTrans = transform.Find("DrawArea/UpgradeInk/Coin");

        _textRefreshTrans = transform.Find("Btns/Refresh/Text").GetComponent<Text>();
        _adRefreshTrans = transform.Find("Btns/Refresh/Ad");
        _coinRefreshTrans = transform.Find("Btns/Refresh/Coin");
    
        _textResetTrans = transform.Find("Btns/Reset/Text").GetComponent<Text>();
        _adResetTrans = transform.Find("Btns/Reset/Ad");
        _coinResetTrans = transform.Find("Btns/Reset/Coin");
        
        _LevelText = transform.Find("DrawArea/UpgradeInk/Level").GetComponent<Text>();
    }
    private void Awake()
    {
        FindProperty();
        if(DrawLineManager.Instance.lineGroup == null)
        {
            GameObject lineGroup = new GameObject("LineGroup");
            DrawLineManager.Instance.lineGroup = lineGroup.transform;
        }
    }

    private void OnEnable()
    {

    }

    private void Start()
    {
        InitWeaponImage();
        DrawLineManager.Instance.RefreshLineLength();
    }

    private void Update()
    {
        RefreshInkShow();
    }

    public void OpenWeaponUI()
    {
        OnResetSuccess();
    }

    public void UpgradeInk()
    {
        UpgradeSuccess();
    }

    private void UpgradeSuccess()
    {
        DrawLineManager.Instance.maxLineLength += DrawLineManager.Instance.upgradeLineLength;
    }

}

