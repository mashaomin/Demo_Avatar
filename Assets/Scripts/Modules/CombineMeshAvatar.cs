
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
using Object = UnityEngine.Object;

namespace Summer
{
    public class CombineMeshAvatar : BaseAvatar
    {
        #region 属性

        public List<Transform> _skeleton_trans = new List<Transform>();
        public List<CombineInstance> _combines = new List<CombineInstance>();
        public List<Material> _mats = new List<Material>();
        #endregion

        #region Override 

        public override void ResetAvatar()
        {

            foreach (var info in _replace_part_map)
            {
                DestroyPart(info.Key);
                GameObject go = AddPart(info.Key, info.Value);
                go.SetActive(false);
            }
            _replace_part_map.Clear();
            _combines.Clear();
            _mats.Clear();
            _skeleton_trans.Clear();
            foreach (var info in _avatar_part_map)
            {
                CollectionAvatarPartInfo(info.Key, info.Value);
            }

            CombineSkinnedMesh();
        }

        #endregion

        #region private 

        public void CollectionAvatarPartInfo(E_AvatarPart part, GameObject go)
        {
            SkinnedMeshRenderer skinned = go.GetComponentInChildren<SkinnedMeshRenderer>();
            _mats.AddRange(skinned.materials);

            int length = skinned.sharedMesh.subMeshCount;
            for (int sub = 0; sub < length; sub++)
            {
                CombineInstance ci = new CombineInstance();
                ci.mesh = skinned.sharedMesh;
                ci.subMeshIndex = sub;
                ci.transform = skinned.transform.localToWorldMatrix;
                //ci.transform = _skeleton.transform.worldToLocalMatrix * skinned.transform.localToWorldMatrix;
                _combines.Add(ci);
            }

            length = skinned.bones.Length;
            for (int i = 0; i < length; i++)
            {
                GameObject bone = skinned.bones[i].gameObject;
                _skeleton_trans.Add(FindBones(bone.name));
            }
        }

        public void CombineSkinnedMesh()
        {
            TransformHelper.DestroyComponent<SkinnedMeshRenderer>(_skeleton_go);

            SkinnedMeshRenderer skinned_mesh = _skeleton_go.AddComponent<SkinnedMeshRenderer>();
            skinned_mesh.sharedMesh = new Mesh();
            skinned_mesh.sharedMesh.CombineMeshes(_combines.ToArray(), false, false);
            skinned_mesh.bones = _skeleton_trans.ToArray();
            skinned_mesh.materials = _mats.ToArray();

            //_combines.Clear();
            //_mats.Clear();
            //_skeleton_trans.Clear();
        }
        #endregion

    }
}