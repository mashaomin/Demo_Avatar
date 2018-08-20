/*

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
    /// <summary>
    /// 写烂之后不想再写的一块东西，本意是想合并哪些可以合并的
    /// </summary>
    public class CombineMaterialAvatar
    {
        #region 属性

        public string shader_name;
        public List<SkinnedMeshRenderer> _meshs = new List<SkinnedMeshRenderer>();
        public List<E_AvatarPart> _contain_parts = new List<E_AvatarPart>();

        public BaseAvatar _base_avatar;

        #region combine


        protected const int COMBINE_TEXTURE_MAX = 512;
        protected const string COMBINE_DIFFUSE_TEXTURE = "_MainTex";
        protected const string DEFAULT_MATERIAL_NAME = "Mobile/Diffuse";

        public List<Material> _mats = new List<Material>();
        public List<CombineInstance> _combines = new List<CombineInstance>();
        public List<Transform> _skeleton_trans = new List<Transform>();

        #endregion

        #endregion

        #region public

        public CombineMaterialAvatar(BaseAvatar base_avatar)
        {
            _base_avatar = base_avatar;
        }

        public void AddPart(E_AvatarPart part)
        {
            _contain_parts.Add(part);
        }

        public bool AddMesh(E_AvatarPart part, SkinnedMeshRenderer mesh)
        {
            if (!_contain_parts.Contains(part)) return false;
            _meshs.Add(mesh);
            return true;
        }

        public void Excute()
        {

        }

        #endregion

        #region

        public void CollectionAllSkinnedMesh()
        {
            int length = _meshs.Count;
            for (int i = 0; i < length; i++)
            {
                CollectionSkinnedMesh(_meshs[i]);
            }
        }

        public void CollectionSkinnedMesh(SkinnedMeshRenderer skinned)
        {
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
                _skeleton_trans.Add(_base_avatar.FindBones(bone.name));
            }
        }

        public List<Vector2[]> _old_uv = new List<Vector2[]>();
        public List<Texture2D> _megre_texs = new List<Texture2D>();
        public Material _new_material = new Material(Shader.Find(DEFAULT_MATERIAL_NAME));
        public Texture2D new_diffuse_tex;
        public void CombineTexture(string tex_name)
        {
            _old_uv.Clear();
            _megre_texs.Clear();
            for (int i = 0; i < _mats.Count; i++)
            {
                _megre_texs.Add(_mats[i].GetTexture(tex_name) as Texture2D);
            }
            if (new_diffuse_tex == null)
                new_diffuse_tex = new Texture2D(COMBINE_TEXTURE_MAX, COMBINE_TEXTURE_MAX, TextureFormat.RGBA32, true);

            Rect[] uvs = new_diffuse_tex.PackTextures(_megre_texs.ToArray(), 0);
            new_diffuse_tex.Apply();
            _new_material.SetTexture(tex_name, new_diffuse_tex);
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

        #endregion
    }
}

*/
