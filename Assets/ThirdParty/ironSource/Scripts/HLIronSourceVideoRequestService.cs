/*//————————————————————————————————————————//
	Company Name : Sompom Game Studio 
	Developer Name 	: Mr.Puthear San
		Unity Developer
//————————————————————————————————————————//*/

using UnityEngine;
using System.Collections;

public class HLIronSourceVideoRequestService : MonoBehaviour
{
	[SerializeField]
    private HLIronSourceAdapter admobMediatorVideo;

	private bool ads = false;

	public bool isVideoReward
	{
		get
		{
			return admobMediatorVideo.IsVideoRewardAvailable();
		}
	}
	public bool isInterstitialVideo
	{
		get
		{
			return admobMediatorVideo.IsInterstitialReady();
		}
	}
	
	private void Awake()
	{
		HLIronSourceAdapter.OnVideoLoad += OnVideoLoaded;
	}

	private void Start()
	{
		gameObject.AddComponent<HLIronSourceListener>();
		RequestRewardedVideo();
		RequestInterstitialVideo();
	}
	
	public void ShowRewardedVideoAds()
	{
#if UNITY_IOS || UNITY_ANDROID
		Debug.Log("call show video");
		ads = false;
		admobMediatorVideo.ShowVideoReward();
		#endif
	}
	public void ShowInterstitialVideoAds()
	{
#if UNITY_IOS || UNITY_ANDROID
		Debug.Log("call show video");
		ads = false;
		admobMediatorVideo.ShowIniterstial();
		#endif
	}
	public void RequestInterstitialVideo()
	{
		#if UNITY_IOS || UNITY_ANDROID
		ads = admobMediatorVideo.IsInterstitialReady();
		if(!ads)
		{
			admobMediatorVideo.RequestInterstitialVideo();
		}
		#endif
	}
	public void RequestRewardedVideo()
	{
		#if UNITY_IOS || UNITY_ANDROID
		ads = admobMediatorVideo.IsVideoRewardAvailable();
		if(!ads)
		{
			admobMediatorVideo.RequestRewardVideo();
		}
		#endif
	}
	
	private void OnVideoLoaded(bool isLoad)
	{
		#if UNITY_IOS || UNITY_ANDROID
		// Debug.Log("Request is Loaded : "+isLoad+" "+admobMediatorVideo.IsVideoLoaded());
		if(admobMediatorVideo.IsVideoRewardAvailable())
		{
			ads = true;
		}
		else
		{
			ads = false;
			StartCoroutine(DelayNextVideoRequest());
		}
		#endif
	}
	
	private IEnumerator DelayNextVideoRequest()
	{
		yield return new WaitForSeconds(10);
		#if UNITY_IOS || UNITY_ANDROID
		if(Application.internetReachability != NetworkReachability.NotReachable)
		{
			if(admobMediatorVideo.IsVideoRewardAvailable())
			{
				ads = true;
			}
			else
			{
				ads = false;
				admobMediatorVideo.RequestRewardVideo();
			}

			if(!admobMediatorVideo.IsInterstitialReady())
			{
				admobMediatorVideo.RequestInterstitialVideo();
			}
		}
		#endif
	}
	
	private IEnumerator WaitInSecond(float delay)
	{
		delay = Time.timeSinceLevelLoad + delay;
		while(Time.timeSinceLevelLoad < delay)
		{
			yield return null;
		}
	}
}
