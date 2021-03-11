using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSlider : MonoBehaviour {

	[SerializeField]
	private Transform front;
	public float m_value;

	public float Value
	{
		get
		{
			return m_value;
		}
		set
		{
			m_value = value;
			front.localScale = new Vector3(value, 1, 1);
			front.localPosition = new Vector3((1 - m_value) * -0.8f, 0);//这个式子可以自己设置数值关观察下

		}
	}
}
