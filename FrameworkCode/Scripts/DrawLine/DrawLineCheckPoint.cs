using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DrawLine
{
    public class DrawLineCheckPoint : MonoBehaviour
    {
        [HideInInspector]
        public bool hasCheck = false;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.tag == "Drawing")
            {
                hasCheck = true;
                Debug.Log("check");
            }
        }
    }
}

