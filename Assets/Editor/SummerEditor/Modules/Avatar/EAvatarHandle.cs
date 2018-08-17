
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
using System.Collections.Generic;
using System.IO;
using Summer;

namespace SummerEditor
{
    /// <summary>
    /// 根据模型的创建骨骼Pfb,和不同类型的Avatar Pfb
    /// </summary>
    public class EAvatarHandle
    {
        public static Dictionary<string, GameObject> _fbxs = new Dictionary<string, GameObject>();

        public static void ExcuteAvater()
        {
            // 1.收集fbx
            CollectionFbx();
            // 2.删除老的
            EDirectoryHelper.DeleteDirectory(EAvatarConst.PREFAB_ASSET_PATH);
            CreateAllPrefab();
        }


        #region 创建骨骼

        public static void CreateAllPrefab()
        {
            foreach (var info in _fbxs)
            {
                string path = info.Key;
                string directory = Path.GetDirectoryName(path);
                if (string.IsNullOrEmpty(directory)) continue;

                string[] splitdirs = directory.Split('/');
                CreateSkeletonPrefab(info.Value, directory, splitdirs[splitdirs.Length - 1]);
                CreateAvatarPrefab(info.Value, directory, splitdirs[splitdirs.Length - 1]);
            }
        }

        public static void CreateSkeletonPrefab(GameObject pfb_go, string dir, string middir)
        {
            string pfb_dir_asset_path = EAvatarConst.PREFAB_ASSET_PATH + "/" + middir + "/";

            EDirectoryHelper.CreateDirectory(pfb_dir_asset_path);
            // 实例化并且格式化
            GameObject go = Object.Instantiate<GameObject>(pfb_go);
            go.name = middir + EAvatarConst.SKELETON_SUFFIX;
            TransformHelper.Normalizle(go);

            // 剔除SkinnedMeshRenderer
            foreach (SkinnedMeshRenderer smr in go.GetComponentsInChildren<SkinnedMeshRenderer>())
                Object.DestroyImmediate(smr.gameObject);

            // 创建Prefab
            EPrefabHelper.CreatePrefab(pfb_dir_asset_path + "/" + go.name.ToLower() + EAvatarConst.PREAFAB_SUFFIX, go);
        }

        #endregion

        #region 创建Avatar部件

        public static void CreateAvatarPrefab(GameObject pfb_go, string dir, string middir)
        {
            string pfb_dir_asset_path = EAvatarConst.PREFAB_ASSET_PATH + "/" + middir + "/";
            string material_dir_path = EAvatarConst.MAT_ASSET_PATH + middir + "/";
            EDirectoryHelper.CreateDirectory(pfb_dir_asset_path);
            // 实例化并且格式化
            GameObject go = Object.Instantiate<GameObject>(pfb_go);

            // 创建子部件
            List<Material> materials = EDirectoryHelper.CollectAll<Material>(material_dir_path);
            SkinnedMeshRenderer[] skinneds = go.GetComponentsInChildren<SkinnedMeshRenderer>(true);



            /* Dictionary<string, SkinnedMeshRenderer> skinned_map = new Dictionary<string, SkinnedMeshRenderer>();
             for (int i = 0; i < skinneds.Length; i++)
                 skinned_map.Add(skinneds[i].gameObject.name, skinneds[i]);*/
            /*for (int i = 0; i < materials.Count; i++)
            {
                Material mat = materials[i];

                for (int j = 0; j < skinneds.Length; j++)
                {
                    if (mat == null) continue;
                    if (!mat.name.Contains(obj.name))continue;
                }

            }*/
            foreach (SkinnedMeshRenderer smr in skinneds)
            {
                GameObject new_part_go = Object.Instantiate<GameObject>(smr.gameObject);

                Material mat = FindMaterialByName(materials, smr.gameObject.name);
                TransformHelper.Normalizle(new_part_go);

                SkinnedMeshRenderer newrenderer = new_part_go.GetComponentInChildren<SkinnedMeshRenderer>();
                newrenderer.material = mat;


                EPrefabHelper.CreatePrefab(pfb_dir_asset_path + "/" + new_part_go.name.ToLower() + EAvatarConst.PREAFAB_SUFFIX, new_part_go);
            }
        }

        #endregion

        #region private

        // 收集fbx
        public static void CollectionFbx()
        {
            _fbxs.Clear();
            Object[] gos = Selection.GetFiltered(typeof(GameObject), SelectionMode.DeepAssets);

            int length = gos.Length;
            for (int i = 0; i < length; i++)
            {
                GameObject obj = gos[i] as GameObject;
                if (!obj) continue;

                if (obj.name.Contains(EAvatarConst.FILTER_FBX)) continue;
                string path = AssetDatabase.GetAssetPath(obj);
                _fbxs.Add(path, obj);
            }
        }

        public static Material FindMaterialByName(List<Material> materials, string mat_name)
        {
            foreach (var material in materials)
            {
                if (material == null) continue;

                if (!material.name.Contains(mat_name)) continue;
                return material;
            }
            return null;
        }
        #endregion
    }
}