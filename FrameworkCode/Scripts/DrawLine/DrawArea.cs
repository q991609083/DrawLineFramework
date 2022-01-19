using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DrawLine
{
    public class DrawArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        /// <summary>
        /// 当前画线的索引
        /// </summary>
        private Transform currentDrawObj;
        /// <summary>
        /// 画线的线的预制体
        /// </summary>
        private GameObject drawPfb;
        /// <summary>
        /// 当前画的索引
        /// </summary>
        private int drawIndex = 0;
        /// <summary>
        /// 标志位，用来判断当前从手指按下到抬起的过程中，是否执行的画线逻辑
        /// </summary>
        private bool isDraw = true;
        /// <summary>
        /// 用来发出射线的相机
        /// </summary>
        public Camera uiCamera;
        private Ray ray;
        private RaycastHit hit;
        public void OnDrag(PointerEventData eventData)
        {
            if (DrawLineManager.Instance.lineLength >= DrawLineManager.Instance.maxLineLength)
            {
                return;
            }
            isDraw = true;
            CreateRayByPosition(eventData.position);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (DrawLineManager.Instance.lineLength >= DrawLineManager.Instance.maxLineLength)
            {
                return;
            }
            InitDrawObj();
            isDraw = false;
            drawIndex = -1;
            currentDrawObj.GetComponent<LineRenderer>().positionCount = 0;
            CreateRayByPosition(eventData.position);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (DrawLineManager.Instance.lineLength >= DrawLineManager.Instance.maxLineLength)
            {
                return;
            }
            if (!isDraw)
            {
                Destroy(currentDrawObj.gameObject);
            }
            currentDrawObj = null;
        }
        /// <summary>
        /// 根据手指滑动的位置，画线
        /// </summary>
        /// <param name="pos"></param>
        private void DrawLineByPos(Vector3 pos)
        {
            LineRenderer line = currentDrawObj.GetComponent<LineRenderer>();
            line.positionCount += 1;
            drawIndex += 1;
            line.SetPosition(drawIndex, pos);
            if (drawIndex >= 1)
            {
                DrawLineManager.Instance.lineLength += Vector3.Distance(line.GetPosition(drawIndex - 1), pos);
            }
            Vector3[] vecs = new Vector3[line.positionCount];
            line.GetPositions(vecs);

            EdgeCollider2D collider = currentDrawObj.GetComponent<EdgeCollider2D>();
            Vector2[] vec2s = new Vector2[vecs.Length];
            for (int i = 0; i < vecs.Length; i++)
            {
                vec2s[i] = new Vector2(vecs[i].x, vecs[i].y);
            }
            collider.points = vec2s;
        }
        /// <summary>
        /// 初始化一个画线的物体
        /// </summary>
        private void InitDrawObj()
        {
            GameObject drawObj = Instantiate(drawPfb);
            drawObj.transform.SetParent(DrawLineManager.Instance.lineGroup, false);
            drawObj.transform.position = new Vector3(0,0,transform.position.z);
            currentDrawObj = drawObj.transform;
        }

        /// <summary>
        /// 根据给定触碰点的位置，从摄像机发射一条射线
        /// </summary>
        /// <param name="pos"></param>
        private void CreateRayByPosition(Vector3 pos)
        {
            ray = uiCamera.ScreenPointToRay(pos);
            if (Physics.Raycast(ray, out hit, 50, 1 << LayerMask.NameToLayer("UI")))
            {
                if(hit.collider.tag == "RecvRay")
                {
                    DrawLineByPos(new Vector3(hit.point.x, hit.point.y, -2));
                }
                
            }
        }

        private void Awake()
        {
            drawPfb = Resources.Load<GameObject>("Prefabs/DrawWeapon/DrawLine");
        }
    }

}
