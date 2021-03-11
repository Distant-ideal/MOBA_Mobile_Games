using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	public Tower tower;
	private GameObject target;
	// Use this for initialization

	public float speed = 20f;

	void Start () {
		tower = GetComponentInParent<Tower> (); //箭塔对于子弹来说相当于父物体
		Destroy (this.gameObject, 1f);
	}
	 
	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "Soldier") {
			//销毁小兵
			Health hp = col.GetComponent<Health>();
			if (hp) {
				hp.TakeDamage (0.5f);
				if (hp.hp.Value <= 0) {
					tower.listSoldier.Remove (col.gameObject);
					Destroy (col.gameObject);
				}
				Destroy (this.gameObject);
			}
		} else if(col.gameObject.tag == "player"){ //防止触发箭塔条件
			//销毁英雄
			Health hp = col.GetComponent<Health>();
			if (hp) {
				hp.TakeDamage (0.2f);
				if (hp.hp.Value <= 0) {
					tower.listHero.Remove (col.gameObject);
					Destroy (col.gameObject);
				}
				Destroy (this.gameObject);
			}
		}
	}

	// Update is called once per frame
	void Update () {
		if(target) {
			Vector3 dir = target.transform.position - transform.position;//三维向量
			GetComponent<Rigidbody>().velocity = dir.normalized * speed;
		} else {
			Destroy (this.gameObject);
		}
	}

	public void SetTarget(GameObject target) {
		this.target = target;
	}
}
