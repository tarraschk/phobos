using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("NGUI/NData/FillAmout Binding")]
public class NguiFillAmountBinding : NguiCustomBoundsNumericBinding
{
	private float _prevValue = -1.0f;
	
	private UISprite _UiSpriteReceiver;
	
	public override void Awake()
	{
		base.Awake();
		
		_UiSpriteReceiver = gameObject.GetComponent<UISprite>();
	}
	
	void Update()
	{
		if (_UiSpriteReceiver != null &&
			_prevValue != _UiSpriteReceiver.fillAmount)
		{
			_prevValue = _UiSpriteReceiver.fillAmount;
			SetCustomBoundsNumericValue(_UiSpriteReceiver.fillAmount);
		}
	}
	
	protected override void ApplyNewCustomBoundsValue(double val)
	{
		if (_UiSpriteReceiver == null)
			return;
		
		_UiSpriteReceiver.fillAmount = (float)val;
	}
}
