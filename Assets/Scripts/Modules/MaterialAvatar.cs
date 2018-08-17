
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
    public class MaterialAvatar : BaseAvatar
    {
        protected const int COMBINE_TEXTURE_MAX = 512;
        protected const string COMBINE_DIFFUSE_TEXTURE = "_MainTex";
        protected const string DEFAULT_MATERIAL_NAME = "Mobile/Diffuse";

        public List<Material> _mats = new List<Material>();
        public List<CombineInstance> _combines = new List<CombineInstance>();
        public List<Transform> _skeleton_trans = new List<Transform>();
        public override void ResetAvatar()
        {
            // 1.从网格中收集到相关信息
            // 2.合并材质球， 强制把纹理合并，并且记录纹理UV
            // 3.合并网格，并且制定网格的UV坐标
            // 4.创建SkinnedMeshRender

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
            CombineMaterial();
            CreateSkinnedMesh();
        }

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

        public List<Vector2[]> _old_uv = new List<Vector2[]>();
        public List<Texture2D> _megre_texs = new List<Texture2D>();
        public Material _new_material = new Material(Shader.Find(DEFAULT_MATERIAL_NAME));
        public Texture2D new_diffuse_tex;
        public void CombineMaterial()
        {
            _old_uv.Clear();
            _megre_texs.Clear();
            for (int i = 0; i < _mats.Count; i++)
            {
                _megre_texs.Add(_mats[i].GetTexture(COMBINE_DIFFUSE_TEXTURE) as Texture2D);
            }
            if (new_diffuse_tex == null)
                new_diffuse_tex = new Texture2D(COMBINE_TEXTURE_MAX, COMBINE_TEXTURE_MAX, TextureFormat.RGBA32, true);

            Rect[] uvs = new_diffuse_tex.PackTextures(_megre_texs.ToArray(), 0);
            new_diffuse_tex.Apply();
            _new_material.mainTexture = new_diffuse_tex;

            // reset uv
            Vector2[] uva, uvb;
            for (int j = 0; j < _combines.Count; j++)
            {
                uva = (_combines[j].mesh.uv);
                uvb = new Vector2[uva.Length];
                for (int k = 0; k < uva.Length; k++)
                {
                    uvb[k] = new Vector2((uva[k].x * uvs[j].width) + uvs[j].x, (uva[k].y * uvs[j].height) + uvs[j].y);
                }
                _old_uv.Add(_combines[j].mesh.uv);
                _combines[j].mesh.uv = uvb;
            }
        }

        public void CreateSkinnedMesh()
        {
            TransformHelper.DestroyComponent<SkinnedMeshRenderer>(_skeleton_go);

            SkinnedMeshRenderer skinned = _skeleton_go.AddComponent<SkinnedMeshRenderer>();
            skinned.sharedMesh = new Mesh();
            skinned.sharedMesh.CombineMeshes(_combines.ToArray(), true, false);
            skinned.bones = _skeleton_trans.ToArray();
            skinned.material = _new_material;

            for (int i = 0; i < _combines.Count; i++)
            {
                _combines[i].mesh.uv = _old_uv[i];
            }
        }
        #endregion

    }
}