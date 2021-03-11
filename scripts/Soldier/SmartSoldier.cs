using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SmartSoldier : MonoBehaviour {


	public Transform target;
	private Animation ani;
	private NavMeshAgent nav;
	public Transform[] towers;
	public int type=0;
	private float distance;//小兵与箭塔的距离
	//敌人进入小兵攻击范围，就加载列表里面
	List<Transform> enemylist=new List<Transform>();

	private Health hp;
	// Use this for initialization
	void Start ()
	{
		hp = GetComponent<Health>();
		nav = GetComponent<NavMeshAgent>();
		ani = GetComponent<Animation>();
		target = GetTarget();
	}

	// Update is called once per frame
	void Update ()
	{
		SoliderMove();
	}

	void SoliderMove()
	{

		if (target==null)
		{
			//目标点为空，需要寻找一下目标点
			target = GetTarget();
			return;
		}

		ani.CrossFade("Run");
		nav.SetDestination(target.position);
		//通过距离判断小兵是否都攻击箭塔

		distance = Vector3.Distance(transform.position, target.position);
		if (distance > 5)
		{
			nav.speed = 3.5f;//保持原速度前进
		}
		else
		{
			nav.speed = 0;//小兵停止前进
			Vector3 tarpos = target.position;
			Vector3 lookpos=new Vector3(tarpos.x,transform.position.y,tarpos.z);
			transform.LookAt(lookpos);//小兵看向炮塔，箭塔比较高，所以看向高度是小兵的高度
			ani.CrossFade("Attack1");
		}
	}

	/// 敌人进入小兵攻击范围,确定小兵攻击目标
	private void OnTriggerEnter(Collider other)
	{
		//判断敌人是英雄还是小兵
		if(other.gameObject.tag=="player"&&this.type==1)//针对于操作者玩家而言，敌方小兵会攻打玩家，玩家相对于敌方小兵是敌人
		{
			this.enemylist.Add(other.transform);
			Transform temp = enemylist[0];//取出列表的第一个作为攻击目标
			if (target==null||temp!=target)
			{
				target = temp;

			}

		}
		else 
		{
			SmartSoldier soldier = other.GetComponent<SmartSoldier>();
			if (soldier!=null
				&&soldier.type!=this.type //确定是敌方
				&&!enemylist.Contains(other.transform))//检查敌人是否已经包含在敌人列表里，防止重复加入
			{
				enemylist.Add(other.transform);
				Transform temp = enemylist[0];
				if (target==null||temp!=target)
				{
					target = temp;
				}
			}

		}

	}

	// 敌人离开小兵攻击范围，从列表中移除
	private void OnTriggerExit(Collider other)
	{
		if (this.enemylist.Contains(other.transform))
		{
			this.enemylist.Remove (other.transform);
			target = GetTarget();
		}
	}
	/// <summary>
	/// 小兵受伤害值
	/// </summary>
	public void Takedamage(float damaga)
	{
		hp.hp.Value -= damaga;
		if (hp.hp.Value<=0)
		{
			Destroy(gameObject);
		}
	}

	//小兵开始攻击播放攻击动画，监听攻击事件
	public void Attack()
	{

		if (target==null)
		{
			target = GetTarget();
			return;
		}

		Health heal = target.GetComponent<Health>();
		if (heal!=null)
		{
			float damp = Random.Range(0.1f, 0.6f);
			heal.TakeDamage(damp);//攻击目标血量减少
			if (heal.hp.Value<=0)//当被攻击对象血量为0时
			{
				Destroy(target.gameObject);
				//移除目标对象，断这个target是兵还是炮塔，并移除
				if (this.enemylist.Contains(target)) 
				{
					this.enemylist.Remove(target);
				}
				else
				{
					DestroyTowerInList(target);
				}
				//上一个目标消灭掉，开始获取下一个目标
				target = GetTarget();
				if (target)
				{
					nav.SetDestination(target.transform.position);
					nav.speed=3.5f;
					ani.CrossFade("Run");

				}
			}
		}
	}
		
	/// 当塔消失时，塔从列表中清除
	void DestroyTowerInList(Transform detroyTower)
	{
		for (int i = 0; i <towers.Length; i++)
		{
			if (towers[i]==detroyTower)
			{
				towers[i] = null;
				return;
			}
		}
	}
	//假如小兵跑向的目标点是塔
	Transform GetTarget()
	{
		this.enemylist.RemoveAll(t=>t==null);//移除掉列表里所有为空的元素
		if (enemylist.Count > 0) //判断数组里是否有值
		{
			//直接拿第一个
			return enemylist[0];
		}
		for (int i=0;i<towers.Length;i++)
		{

			if (towers[i]!=null)
			{
				return towers[i];
			}
		}
		return null;
	}

	//设置小兵行走的路
	public void SetRoad(int road)
	{

		nav = GetComponent<NavMeshAgent>();
		nav.areaMask = road;
	}
}