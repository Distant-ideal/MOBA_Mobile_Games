using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatSoldier : MonoBehaviour {
	[SerializeField]
	private GameObject soldierPrefab;

	[SerializeField]
	private Transform soldierParent;
	//是否生成小兵默认为true
	bool isCreatSoldier = true;
	//生成小兵的数量
	public int soldierCount = 2;

	//虽然目标点对象不能直接托给预制体，但是我们可以在创建预制体时给他赋值
	[SerializeField]
	private Transform[] middleTowers;
	[SerializeField]
	private Transform[] LeftTowers;
	[SerializeField]
	private Transform[] RightTowers;
	[SerializeField]
	private Transform[] middleEnemyTowers;
	[SerializeField]
	private Transform[] LeftEnemyTowers;
	[SerializeField]
	private Transform[] RightEnemyTowers;

	//生成点数组
	[SerializeField]
	private Transform[] Start1;
	[SerializeField]
	private Transform[] Start2;



	void Start() {
		StartCoroutine (Creat (0, 1, 5));
	}

	//生成一个小兵
	//层级表示的时2的几次方所以时int型的
	void CreatSmartSoldier(SoldierType soldierType, Transform startTran, Transform[] towers, int road) {
		GameObject obj = Instantiate(soldierPrefab, startTran.position, Quaternion.identity) as GameObject;
		//设置父物体
		obj.transform.parent = soldierParent; 

		SmartSoldier soldier = obj.GetComponent<SmartSoldier> (); //获取脚本
		soldier.towers = towers; //指定目标防御塔
		soldier.SetRoad(road);
		soldier.type = (int)soldierType; //SoldierType类型转换为int型
	}

	//协程生成一波一波小兵
	//time游戏开始后几秒开始生成士兵
	//delyTime同一波内两个小兵生成的间隔
	//spwanTime下一波小兵生成的时间间隔
	private IEnumerator Creat(float time, float delyTime, float spwanTime) {
		yield return new WaitForSeconds(time); //几秒后开始生成小兵
		while(isCreatSoldier) {
			//一个for循环代表一波小兵
			for(int i = 0; i < soldierCount; i++) {
				CreatSmartSoldier(SoldierType.soldier1, Start1[0], middleEnemyTowers, 1 << 3); //2的3次方
				CreatSmartSoldier(SoldierType.soldier2, Start2[0], middleTowers, 1 << 3);	//设置我方敌方小兵类型

				CreatSmartSoldier(SoldierType.soldier1, Start1[1], LeftEnemyTowers, 1 << 4);
				CreatSmartSoldier(SoldierType.soldier2, Start2[1], LeftTowers, 1 << 4);

				CreatSmartSoldier(SoldierType.soldier1, Start1[2], RightEnemyTowers, 1 << 5);
				CreatSmartSoldier(SoldierType.soldier2, Start2[2], RightTowers, 1 << 5);


				yield return new WaitForSeconds(delyTime);//生成下一个小兵的时间间隔
			}
			//等待下一波小兵生成的时间
			yield return new WaitForSeconds(spwanTime); //生成下一波的时间间隔
		}
	}
}
