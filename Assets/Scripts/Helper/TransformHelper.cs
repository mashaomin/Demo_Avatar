
//
//                            _ooOoo_
//                           o8888888o
//                           88" . "88
//                           (| -_- |)
//                           O\  =  /O
//                        ____/`---'\____
//                      .'  \\|     |//  `.
//                     /  \\|||  :  |||//  \
//                    /  _||||| -:- |||||-  \
//                    |   | \\\  -  /// |   |
//                    | \_|  ''\---/''  |   |
//                    \  .-\__  `-`  ___/-. /
//                  ___`. .'  /--.--\  `. . __
//               ."" '<  `.___\_<|>_/___.'  >'"".
//              | | :  `- \`.;`\ _ /`;.`/ - ` : | |
//              \  \ `-.   \_ __\ /__ _/   .-` /  /
//         ======`-.____`-.___\_____/___.-`____.-'======
//                            `=---='
//        ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
//                 			 佛祖 保佑             

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Summer
{
    public class TransformHelper
    {
        public static void CollectionTrans(GameObject go, Dictionary<string, Transform> tran_map)
        {
            tran_map.Clear();
            Transform[] trans = go.GetComponentsInChildren<Transform>(true);
            int length = trans.Length;
            for (int i = 0; i < length; i++)
            {
                tran_map.Add(trans[i].name, trans[i]);
            }
        }

        public static void DestroyComponent<T>(GameObject go) where T : UnityEngine.Object
        {
            T t = go.GetComponent<T>();
            if (t != null)
            {
                UnityEngine.Object.DestroyImmediate(t);
                t = null;
            }
        }

        public static void Normalizle(GameObject go)
        {
            go.transform.position = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
        }

        public static void Reset(GameObject go, GameObject parent)
        {
            go.transform.parent = parent.transform;
            Normalizle(go);
        }
    }
}

