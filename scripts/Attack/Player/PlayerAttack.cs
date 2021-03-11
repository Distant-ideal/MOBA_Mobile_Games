using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

	Animator ani;
	[SerializeField]
	private ParticleSystem fire1;
	[SerializeField]
	private ParticleSystem fire2;

	private int type;
	//玩家攻击对象之所以放进列表是因为，有的招数技能攻打的是一个区域内的小兵，并不一定是一个
	List<GameObject> enemyList=new List<GameObject>();

	// Use this for initialization
	void Start () {
		ani = GetComponent<Animator>();
		//设定敌方英雄，我方英雄类型
		if (this.tag=="player")
		{
			type = 0;  
		}
		else
		{
			type = 1;
		}

	}
	void OnTriggerExit(Collider col)
	{      
		//print ("移除队列");
		enemyList.Remove(col.gameObject);
		enemyList.RemoveAll(t=>t==null);//移除列表里所有为空的元素（括号里是泛型用法）
	}
	void OnTriggerEnter(Collider col)
	{
		if (!this.enemyList.Contains (col.gameObject)) {
			//print ("进入队列");
			enemyList.Add (col.gameObject);  
		}
	}  

	public void Atk1()
	{
		ani.SetInteger("state", AnimState.ATTACK1);
		if (enemyList.Count <= 0) return;    
		for (int i = 0; i < enemyList.Count; i++)
		{
			if (enemyList.Count > 0 && !enemyList[i].name.Contains("Tower"))
			{
				SmartSoldier soldier = enemyList[i].GetComponent<SmartSoldier>();
				if (soldier&&soldier.type!=this.type)//是小兵就攻击小兵
				{                   
					Health hp=soldier.GetComponent<Health>();
					hp.TakeDamage(0.5f);
					if (hp.hp.Value<=0)
					{
						enemyList.Remove(soldier.gameObject);
						Destroy(soldier.gameObject);
					}
				}               
			}         
		}      

	}
	public void Atk2()
	{
		ani.SetInteger("state",AnimState.ATTACK1);
		if (enemyList.Count <= 0) return;    
		for (int i = 0; i < enemyList.Count; i++)
		{
			if (enemyList.Count > 0 && !enemyList[i].name.Contains("Tower"))
			{
				SmartSoldier soldier = enemyList[i].GetComponent<SmartSoldier>();
				if (soldier&&soldier.type!=this.type)//是小兵就攻击小兵
				{                   
					Health hp=soldier.GetComponent<Health>();
					hp.TakeDamage(1f);
					if (hp.hp.Value<=0)
					{
						enemyList.Remove(soldier.gameObject);
						Destroy(soldier.gameObject);
					}
				}               
			}         
		}

	}
	public void Dance()
	{
		//用dance代替第三次攻击
		ani.SetInteger("state", AnimState.DANCE);

	}
	//动画监听事件，攻击动画播放完播放攻击特效
	public void EffectPlay1()
	{
		fire1.Play();
	}
	public void EffectPlay2()
	{
		fire2.Play();
	}
	//攻击动画播放完转换成Idle状态
	public void ResetIdle()
	{
		ani.SetInteger("state", AnimState.IDLE);
	}
}