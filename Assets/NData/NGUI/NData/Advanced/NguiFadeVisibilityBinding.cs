using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class NguiFadeVisibilityBinding : NguiVisibilityBinding
{
	public float fadeInDuration = 0.3f;
	public float fadeOutDuration = 0.3f;
	public bool dynamicContent = false;
	
	public delegate void FadeFinish(bool visible);
	public event FadeFinish OnFadeFinish;
	
	private void InvokeOnFadeFinish(bool visible)
	{
		var callback = OnFadeFinish;
		if (callback != null)
			callback(visible);
	}
	
	private enum Status
	{
		None,
		FadeIn,
		FadeOut,
	};
	
	private bool _inited;
	private bool _screenVisible;
	private Status _status = Status.None;
	private float _fade;
	private float _lastRealTime;
	private UIWidget [] _widgets;
	
	public override void Awake()
	{
		base.Awake();
		_widgets = GetComponentsInChildren<UIWidget>();
	}
	
	private void SetAlpha(float a)
	{
		foreach(var w in _widgets)
		{
			w.alpha = a;
		}
	}
	
	protected override void ApplyNewValue(bool newValue)
	{
		if (!_inited && !newValue)
		{
			base.ApplyNewValue(newValue);
			_inited = true;
			_screenVisible = newValue;
			return;
		}
		_inited = true;
		
		if (!newValue && _screenVisible)
		{
			_status = Status.FadeOut;
			_fade = 0;
		}
		if (newValue && !_screenVisible)
		{
			base.ApplyNewValue(newValue);
			InvokeOnFadeFinish(newValue);
			_status = Status.FadeIn;
			_fade = 0;
		}
		
		_screenVisible = newValue;
	}
	
	public override void Start()
	{
		base.Start();
		InvokeOnFadeFinish(_screenVisible);
		_lastRealTime = Time.realtimeSinceStartup;
	}
	
	void Update()
	{
		var dt = Time.realtimeSinceStartup - _lastRealTime;
		_lastRealTime = Time.realtimeSinceStartup;
		
		if (_status == Status.None)
			return;
		
		if (dynamicContent)
			_widgets = GetComponentsInChildren<UIWidget>();
	
		var fadeDuration = (_status == Status.FadeIn) ? fadeInDuration : fadeOutDuration;
		var instantFade = fadeDuration <= 0.001f;
		var fadeSpeed = instantFade ? 1000000.0f : (1 / fadeDuration);
		_fade = instantFade ? 1 : Mathf.Clamp01(_fade + fadeSpeed * dt);
		
		var a = 1.0f;
		if (_status == Status.FadeIn)
			a = _fade;
		else if (_status == Status.FadeOut)
			a = 1 - _fade;
		a = Mathf.Clamp01(a);
		SetAlpha(a);
		
		if (_fade < 1)
			return;
		
		if (!_screenVisible)
		{
			base.ApplyNewValue(_screenVisible);
			InvokeOnFadeFinish(_screenVisible);
		}
		_status = Status.None;
	}
}
