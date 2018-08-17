
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

using UnityEditor;
using UnityEngine;

namespace SummerEditor
{
    public class EPrefabHelper
    {
        public static void CreatePrefab(string pfb_path, GameObject go, bool destroy = true)
        {
            //string pfb_path = pfb_dir_asset_path + "/" + go.name.ToLower() + EAvatarConst.PREAFAB_SUFFIX;
            PrefabUtility.CreatePrefab(pfb_path, go);
            if (destroy)
                Object.DestroyImmediate(go);
        }
    }
}