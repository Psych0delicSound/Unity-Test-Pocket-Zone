using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
	Slider slider;

	void Start()
	{
		slider = GetComponent<Slider>();
	}
	
	public void ChangeValue(float value)
	{
		slider.value = value;
	}
}