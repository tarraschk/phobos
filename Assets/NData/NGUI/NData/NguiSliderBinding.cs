using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Slider Binding")]
public class NguiSliderBinding : NguiPollingCustomBoundsNumericBinding
{
	private UISlider _UiSliderReceiver;
	
	public override void Awake()
	{
		base.Awake();
		_UiSliderReceiver = gameObject.GetComponent<UISlider>();
	}
	
	protected override double GetValue()
	{
		return (_UiSliderReceiver != null) ? _UiSliderReceiver.sliderValue : 0;
	}
	
	protected override void SetValue(double val)
	{
		if (_UiSliderReceiver != null)
			_UiSliderReceiver.sliderValue = (float)val;
	}
}
