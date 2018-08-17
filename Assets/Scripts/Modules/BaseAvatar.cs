
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
    public abstract class BaseAvatar
    {
        #region 属性

        protected GameObject _skeleton_go;                             // 骨骼GameObject
        protected Dictionary<string, Transform> _skeleton_map
            = new Dictionary<string, Transform>();                      // 骨骼节点

        public Dictionary<E_AvatarPart, GameObject> _avatar_part_map
            = new Dictionary<E_AvatarPart, GameObject>();               // Avatar部位 存在的部分

        public Dictionary<E_AvatarPart, GameObject> _replace_part_map
            = new Dictionary<E_AvatarPart, GameObject>();               // Avatar部位 更改的部分

        #endregion

        #region public 

        public GameObject GetSkeleton() { return _skeleton_go; }

        #endregion

        #region virtual

        public virtual void Init(GameObject pfb_skeleton)
        {
            _skeleton_go = Object.Instantiate(pfb_skeleton); ;
            TransformHelper.CollectionTrans(_skeleton_go, _skeleton_map);
        }

        public virtual void ReplaceAvatar(E_AvatarPart part, GameObject pfb_go)
        {
            if (_replace_part_map.ContainsKey(part))
                _replace_part_map.Remove(part);
            _replace_part_map.Add(part, pfb_go);
        }

        public abstract void ResetAvatar();

        #endregion

        #region protected

        protected Transform FindBones(string bone_name)
        {
            if (_skeleton_map.ContainsKey(bone_name))
            {
                return _skeleton_map[bone_name];
            }
            else
            {
                Debug.LogFormat("找不到对应的骨骼:[{0}]", bone_name);
                return null;
            }
        }

        protected bool DestroyPart(E_AvatarPart part)
        {
            if (!_avatar_part_map.ContainsKey(part)) return false;
            GameObject old_go = _avatar_part_map[part];
            _avatar_part_map.Remove(part);
            Object.DestroyImmediate(old_go);
            old_go = null;
            return true;
        }

        protected GameObject AddPart(E_AvatarPart part, GameObject pfb_go)
        {
            GameObject go = Object.Instantiate(pfb_go);
            TransformHelper.Reset(go, _skeleton_go);
            _avatar_part_map.Add(part, go);
            return go;
        }

        #endregion

    }
}