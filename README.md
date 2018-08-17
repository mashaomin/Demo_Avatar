# Demo_Avatar
	Unity 换装功能
	主要参考了(Unity中Avatar换装实现)[https://blog.uwa4d.com/archives/avartar.html]这篇文章

包含了几部分
- 骨骼，影响动作
- Mesh 影响蒙皮显示
- 材质球和纹理，显示蒙皮显示
		
		
- 替换蒙皮网格		
- 刷新骨骼
- 替换材质

		
## 资源准备

		
## 共享骨骼替换蒙皮网格	
	What
		
	How
		
	
	直接替换模型换装部位的GameObject，并且重新指定SkinnedMeshRender的Bones	
		
## 合并网格，材质球多个

## 合并Mesh/合并材质球/

	为了降低游戏的Draw Call会合并模型的网格，这就需要我们重新计算UV，还要合并贴图和材质