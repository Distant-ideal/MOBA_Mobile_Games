using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

	public SpriteSlider hp;
	private uint value=1;//uint 表示值不可能为负

	// Use this for initialization
	void Start () {
		hp=GetComponentInChildren<SpriteSlider>();//代码找到组建，不用一个个拖拽
		Init();
	}

	public void Init()
	{
		hp.Value = value;//血条初始化值为1
	}
	//减血的方法
	public void TakeDamage(float damage)
	{

		hp.Value -= damage;
	}
}
