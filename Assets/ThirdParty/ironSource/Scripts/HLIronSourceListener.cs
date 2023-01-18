/*//————————————————————————————————————————//
	Company Name : Sompom Game Studio 
	Developer Name 	: Mr.Puthear San
		Unity Developer
//————————————————————————————————————————//*/

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class HLIronSourceListener : MonoBehaviour
{
    public static Action OnVideoComplete;
	public static Action OnVideoFailed;
	
	public static Action OnVideoCompleteFreeBoostIcon;
	
	private static List<VideoCallBack> ListVideoCallBack = new List<VideoCallBack>();
	
	private void Awake()
	{
		HLIronSourceAdapter.OnPlayVideoComplete += OnAdmobVideoComplete;
		HLIronSourceAdapter.OnVideoRewarded += OnVideoRewarded;
	}

	private void RequestVideoAgain()
	{
		gameObject.GetComponent<HLIronSourceVideoRequestService>().RequestRewardedVideo();
	}
	
	//====== Admob Video ===============
	private void OnAdmobVideoComplete(bool isComplete)
	{
		if(isComplete)
		{
			PlayVideoSuccess();
		}
		else
		{
			PlayVideoFailed();
		}
	}

	private void OnVideoRewarded ()
	{
		int count = ListVideoCallBack.Count;
		if(count == 0)
		{
			return;
		}
		VideoCallBack callBack = ListVideoCallBack[count-1];
		if (callBack.OnVideoRewarded != null)
		{
			callBack.OnVideoRewarded.Invoke();
		}
		// Debug.Log("Call back to give rewarded only!");
	}
	private void PlayVideoSuccess()
	{
		Debug.Log("Play video success !!!");
		if (OnVideoCompleteFreeBoostIcon != null)
		{
			OnVideoCompleteFreeBoostIcon.Invoke();
			OnVideoCompleteFreeBoostIcon = null;
		}
		
		InvokeVideoCallBack(true);
		
		ResetGameSound();
		
		RequestVideoAgain();
	}
	
	public void PlayVideoFailed()
	{
		OnVideoCompleteFreeBoostIcon = null;
		
		InvokeVideoCallBack(false);
		
		ResetGameSound();
		
		RequestVideoAgain();
	}
	
	private void ResetGameSound()
	{
		StartCoroutine(Delay(()=>{
			// if(AudioManager.Instance != null)
			// {
			// 	AudioManager.Instance.Pua(SMPGameData.Instance.IsSound);
			// }
			// if(SMPSoundManager.instance != null)
			// {
			// 	SMPSoundManager.instance.PlayMusic();
			// }
		}));
	}
	
	private void InvokeVideoCallBack(bool isSuccess)
	{
		//Debug.Log("Start Invok Call back: "+isSuccess);
		//Debug.Log("OnCOmplete " + (OnVideoComplete != null));
		//Debug.Log("OnFialed " + (OnVideoFailed!=null));
		if (isSuccess && OnVideoComplete != null)
		{
			OnVideoComplete.Invoke();
		}
		else if(!isSuccess && OnVideoFailed != null)
		{
			OnVideoFailed.Invoke();
		}
		
		int count = ListVideoCallBack.Count;
		if(count == 0)
		{
			return;
		}
		
		VideoCallBack callBack = ListVideoCallBack[count-1];
		// Debug.Log("V Callback "+callBack+" isSuccess "+isSuccess);
		if(isSuccess)
		{
			if(callBack.OnVideoSuccess != null)
			{
				callBack.OnVideoSuccess.Invoke();
			}
		}
		else
		{
			if(callBack.OnVideoFailed != null)
			{
				callBack.OnVideoFailed.Invoke();
			}
		}
	}
	
	public static void AddVideoCallBack(Action onSuccess,Action onFailed, Action onVideoRewarded = null)
	{
		bool isExist = false;
		foreach(VideoCallBack cb in ListVideoCallBack)
		{
			if(cb.OnVideoSuccess == onSuccess && cb.OnVideoFailed == onFailed && cb.OnVideoRewarded == onVideoRewarded)
			{
				isExist = true;
				break;
			}
		}
		if(isExist == false)
		{
			VideoCallBack callBack = new VideoCallBack();
			callBack.OnVideoSuccess = onSuccess;
			callBack.OnVideoFailed = onFailed;
			callBack.OnVideoRewarded = onVideoRewarded;
			ListVideoCallBack.Add(callBack);
		}
		// Debug.Log("V Add Callback "+ListVideoCallBack.Count+" : "+onSuccess);
	}
	
	public static void RemoveVideoCallBack(Action onSuccess, Action onFailed, Action onVideoRewarded = null)
	{
		VideoCallBack callBack = null;
		foreach(VideoCallBack cb in ListVideoCallBack)
		{
			if(cb.OnVideoSuccess == onSuccess && cb.OnVideoFailed == onFailed && cb.OnVideoRewarded == onVideoRewarded)
			{
				callBack = cb;
				break;
			}
		}
		
		if(callBack != null)
		{
			ListVideoCallBack.Remove(callBack);
		}
		// Debug.Log("V Remove Callback "+ListVideoCallBack.Count);
	}

	private IEnumerator Delay(Action action)
	{
		yield return new WaitForSeconds(1);
		action.Invoke();
	}
	
	private class VideoCallBack
	{
		public Action OnVideoSuccess;
		public Action OnVideoRewarded;
		public Action OnVideoFailed;
	}
}
