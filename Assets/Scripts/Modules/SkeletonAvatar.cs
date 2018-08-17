
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

using System.Collections.Generic;
using UnityEngine;

namespace Summer
{
    public class SkeletonAvatar : BaseAvatar
    {
        public override void ResetAvatar()
        {
            foreach (var info in _replace_part_map)
            {
                DestroyPart(info.Key);
                GameObject go = AddPart(info.Key, info.Value);
                ShareSkeletonInstanceWith(go.GetComponentInChildren<SkinnedMeshRenderer>(), _skeleton_go);
            }
            _replace_part_map.Clear();
        }

        public void ShareSkeletonInstanceWith(SkinnedMeshRenderer skin, GameObject target)
        {
            Transform[] new_bones = new Transform[skin.bones.Length];
            int length = skin.bones.Length;
            for (int i = 0; i < length; i++)
            {
                GameObject bone = skin.bones[i].gameObject;
                new_bones[i] = FindBones(bone.name);
            }
            skin.bones = new_bones;
        }
    }
}
