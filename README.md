# Demo_Avatar
	Unity 换装功能
	主要参考了-[Unity中Avatar换装实现](https://blog.uwa4d.com/archives/avartar.html)这篇文章
    
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
	

	
# 相关知识

## SkinedMeshRender
	该对象负责网格绘制

主要数据成员包括	
- bones : Transform[] 骨骼
- materials : Material[] 材质
- sharedMesh : Mesh 网格

其中Mesh的主要成员是 
vertices : Vector3[] 顶点
boneWeights : BoneWeight[] 骨骼权重
boneWeights数组与vertices数组对应，表示对应下标的顶点运动受骨骼影响的权重。BoenWeight结构记录了骨骼在SkinedMeshRender.bones数组中的索引。

## 网格和材质的对应关系
	一张实际的网格只能施加一个材质。因此，当render所使用的mesh包含多个实际网格（sub mesh），它对每个sub mesh所施加的材质实际上是materials数组中对应下标的材质。

## 合并网格 CombineMeshes
	函数的第二个参数是设置是否将多个子网格合并成一张实际的网格。
	正如前面所述，一个实际的网格只能施加一个材质， 所以只有被合并的所有网格原来使用的就是同一个材质（即共享材质）时，将它们真正合并才能正确应用材质。
	否则，应该将该参数置为false，表示不实际合 并这些sub mesh，而是将它们作为被合并后Mesh对象的sub mesh。

# QAQ


###

	Combine
	// 这个函数实际上并没有将各部分的子网格合并成一张网，而只是将他们合并到
	// 同一个Mesh下作为sub mesh。因为一张网格只能用一个材质，只有所有子网格
	// 都共享同一个材质时，合并成一张网才能保证材质应用正确。
###

	两个模型在制作时是基于同一套骨骼，导出模型部位时连着该部位的骨骼一起导出，这样导入到Unity的模型就带有SkinnedMeshRenderer组件。
	