using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

	public List <GameObject> listSoldier = new List<GameObject> (); //小兵攻击箭塔队列
	public List <GameObject> listHero = new List<GameObject> (); //英雄攻击箭塔队列
	public int towerType;
	[SerializeField]
	private GameObject bulletPrefab;
	[SerializeField]
	private Transform bulletStart;
	[SerializeField]
	private Transform parent;

	// Use this for initialization
	void Start () {
		if (this.gameObject.tag.Equals ("Tower")) { //读取标签看是否是地方箭塔 //为了区分我方箭塔和小兵
			towerType = 0; //为了和小兵进行匹配所以使用int型
		} else {
			towerType = 1;
		}
		InvokeRepeating ("CreatBullet", 0.1f, 1.3f);
	}


	public void CreatBullet() {
		if(listHero.Count == 0 && listSoldier.Count == 0) return;
		//否则生成子弹
		GameObject bullet = (GameObject)Instantiate(bulletPrefab,bulletStart.position,Quaternion.identity);
		bullet.transform.parent = parent;
		BulletTarget(bullet);//设置子弹攻击目标
	}

	public void BulletTarget(GameObject bullet) { //先从小兵集合中取出目标之后再从英雄中取出
		if (listSoldier.Count > 0) {
			bullet.GetComponent<Bullet>().SetTarget(listSoldier[0]);
		} else {
			bullet.GetComponent<Bullet>().SetTarget(listHero[0]);
		}
	}
		

	private void OnTriggerEnter(Collider col) { //触发函数
		if (col.gameObject.tag == "player" ) {
			listHero.Add (col.gameObject);
		} else {
			SmartSoldier soldier = col.GetComponent<SmartSoldier> ();
			if (soldier && soldier.type != towerType) { //判断小兵是否不存在或者类型不匹配
				//print(listSoldier.Count);
				listSoldier.Add (col.gameObject); //插入到队列中
			}
		}
	}

	//人物退出范围，移除列表
	private void OnTriggerExit(Collider col) {
		if(col.gameObject.tag == "player") {
			listHero.Remove (col.gameObject);
		} else {
			listSoldier.Remove (col.gameObject); //删除出队列
		}
	}
} 
