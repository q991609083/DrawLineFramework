using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DrawLine
{
    public class DrawLineManager
    {
        #region SingleInstance
        private static DrawLineManager _instance = null;
        private static object _lock = new object();
        private DrawLineManager() { }

        public static DrawLineManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance = new DrawLineManager();
                        if (_instance == null)
                        {
                            _instance = new DrawLineManager();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion
        /// <summary>
        /// 所有线的父节点
        /// </summary>
        public Transform lineGroup;

        /// <summary>
        /// 当前线的长度
        /// </summary>
        public float lineLength = 0;
        /// <summary>
        /// 刷新花费
        /// </summary>
        public int refreshCost = 100;
        /// <summary>
        /// 重置花费
        /// </summary>
        public int resetCost = 50;
        /// <summary>
        /// 墨水升级金币成长
        /// </summary>
        public int upgradeCost = 100;
        /// <summary>
        /// 墨水升级数值
        /// </summary>
        public float upgradeLineLength = 50f;
        /// <summary>
        /// 最大线的长度(墨水)
        /// </summary>
        public float maxLineLength = 100;
        /// <summary>
        /// 当前武器的id
        /// </summary>
        public int curWeaponId = 0;
        /// <summary>
        /// 刷新墨水长度
        /// </summary>
        public void RefreshLineLength()
        {
            lineLength = 0;
        }
    }

}

