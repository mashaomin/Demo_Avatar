﻿
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
using Summer;
public class Main : MonoBehaviour
{

    #region 属性

    public GameObject pfb_skeleton;
    public List<AvatarPartInfo> avatar_parts;
    public BaseAvatar _base_avater;
    public bool flag = false;

    public CombineType _type;

    #endregion

    #region MONO Override

    // Use this for initialization
    void Start()
    {
        if (_type == CombineType.combine_mesh)
            _base_avater = new CombineMeshAvatar();
        else if (_type == CombineType.share_skeleton)
            _base_avater = new SkeletonAvatar();
        else if (_type == CombineType.combine_mesh_material_1)
            _base_avater = new CombineMeshAvatar();
        else if (_type == CombineType.combine_mesh_material_2)
            _base_avater = new Material2Avatar();

        _base_avater.Init(pfb_skeleton);
    }

    // Update is called once per frame
    void Update()
    {
        if (flag)
        {
            flag = false;
            _excute();
        }
    }

    #endregion

    #region Public


    #endregion

    #region Private Methods


    public void _excute()
    {

        for (int i = 0; i < avatar_parts.Count; i++)
            _base_avater.ReplaceAvatar(avatar_parts[i].part, avatar_parts[i].pfb_go);

        _base_avater.ResetAvatar();

        Animation anim = _base_avater.GetSkeleton().GetComponent<Animation>();
        AnimationClip anim_clip = anim.GetClip("walk");
        anim_clip.wrapMode = WrapMode.Loop;
        anim.wrapMode = WrapMode.Loop;
        anim.Play(anim_clip.name);
    }

    #endregion
}

[System.Serializable]
public class AvatarPartInfo
{
    public E_AvatarPart part;
    public GameObject pfb_go;
}


public enum CombineType
{
    share_skeleton,
    combine_mesh,
    combine_mesh_material_1,
    combine_mesh_material_2,
}
