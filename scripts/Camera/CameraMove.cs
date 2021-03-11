using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Transform _player;
    private Vector3 _vec;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").transform; //读取玩家位置
        _vec = _player.position - transform.position; //计算偏移量
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _player.position - _vec; //用玩家位置减去偏移量
    }
}
