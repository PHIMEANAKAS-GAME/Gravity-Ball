using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HLIronSourceAdapter : MonoBehaviour
{
    public static HLIronSourceAdapter instance;

    [SerializeField]
    private string appKey = "";

    public static Action<bool> OnPlayVideoComplete; //is just to tell that we should resume the game
	public static Action OnVideoRewarded; //when we give reward, so we always give reward in OnVideoRewarded
    public static Action<bool> OnVideoLoad;

    private bool isVideoReceivedRewarded = false;

    private bool isBannerCallLoaded;
    private bool isBannerLoaded;

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        UnSubscribe();
    }

    private void Awake()
    {
        instance = this;
        Initialize();
    }

    private void Start()
    {
        LoadBanner();
        RequestInterstitialVideo();
    }

    private void Initialize()
    {
        IronSource.Agent.validateIntegration();
        IronSource.Agent.init(appKey);

        //
    }

    private void Subscribe()
    {
        //Add AdInfo Rewarded Video Events
        IronSourceRewardedVideoEvents.onAdOpenedEvent += ReardedVideoOnAdOpenedEvent;
        IronSourceRewardedVideoEvents.onAdClosedEvent += ReardedVideoOnAdClosedEvent;
        IronSourceRewardedVideoEvents.onAdAvailableEvent += ReardedVideoOnAdAvailable;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent += ReardedVideoOnAdUnavailable;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent += ReardedVideoOnAdShowFailedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent += ReardedVideoOnAdRewardedEvent;
        IronSourceRewardedVideoEvents.onAdClickedEvent += ReardedVideoOnAdClickedEvent;

        //Add AdInfo Banner Events
        IronSourceBannerEvents.onAdLoadedEvent += BannerOnAdLoadedEvent;
        IronSourceBannerEvents.onAdLoadFailedEvent += BannerOnAdLoadFailedEvent;
        IronSourceBannerEvents.onAdClickedEvent += BannerOnAdClickedEvent;
        IronSourceBannerEvents.onAdScreenPresentedEvent += BannerOnAdScreenPresentedEvent;
        IronSourceBannerEvents.onAdScreenDismissedEvent += BannerOnAdScreenDismissedEvent;
        IronSourceBannerEvents.onAdLeftApplicationEvent += BannerOnAdLeftApplicationEvent;
    }

    private void UnSubscribe()
    {
        //Add AdInfo Rewarded Video Events
        IronSourceRewardedVideoEvents.onAdOpenedEvent += ReardedVideoOnAdOpenedEvent;
        IronSourceRewardedVideoEvents.onAdClosedEvent += ReardedVideoOnAdClosedEvent;
        IronSourceRewardedVideoEvents.onAdAvailableEvent += ReardedVideoOnAdAvailable;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent += ReardedVideoOnAdUnavailable;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent += ReardedVideoOnAdShowFailedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent += ReardedVideoOnAdRewardedEvent;
        IronSourceRewardedVideoEvents.onAdClickedEvent += ReardedVideoOnAdClickedEvent;

        //Add AdInfo Banner Events
        IronSourceBannerEvents.onAdLoadedEvent -= BannerOnAdLoadedEvent;
        IronSourceBannerEvents.onAdLoadFailedEvent -= BannerOnAdLoadFailedEvent;
        IronSourceBannerEvents.onAdClickedEvent -= BannerOnAdClickedEvent;
        IronSourceBannerEvents.onAdScreenPresentedEvent -= BannerOnAdScreenPresentedEvent;
        IronSourceBannerEvents.onAdScreenDismissedEvent -= BannerOnAdScreenDismissedEvent;
        IronSourceBannerEvents.onAdLeftApplicationEvent -= BannerOnAdLeftApplicationEvent;
    }

    public void RequestRewardVideo()
    {
        IronSource.Agent.loadRewardedVideo();
    }

    public void ShowVideoReward()
    {
        if (IsVideoRewardAvailable())
        {
            isVideoReceivedRewarded = false;
            IronSource.Agent.showRewardedVideo();
        }
    }

    public bool IsVideoRewardAvailable()
    {
        return IronSource.Agent.isRewardedVideoAvailable();
    }

    public void RequestInterstitialVideo()
    {
        IronSource.Agent.loadInterstitial();
    }

    public bool IsInterstitialReady()
    {
        return IronSource.Agent.isInterstitialReady();
    }

    public void ShowIniterstial()
    {
        if(IsInterstitialReady())
        {
            IronSource.Agent.showInterstitial();
        }
        else
        {
            RequestInterstitialVideo();
        }
    }

    public void LoadBanner()
    {
        isBannerCallLoaded = true;
        IronSource.Agent.loadBanner(IronSourceBannerSize.SMART, IronSourceBannerPosition.BOTTOM);
    }

    public bool IsBannerReady()
    {
        return isBannerLoaded;
    }

    public void ShowBanner(bool isShow)
    {
        if(isShow)
        {
            if(IsBannerReady())
            {
                IronSource.Agent.displayBanner();
            }
            else if(!isBannerCallLoaded)
            {
                LoadBanner();
            }
        }
        else
        {
            IronSource.Agent.hideBanner();
        }
    }

    public void DestroyBanner()
    {
        IronSource.Agent.destroyBanner();
        BannerNeedReset();
    }

    #region AdInfo Rewarded Video
    void ReardedVideoOnAdOpenedEvent(IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got ReardedVideoOnAdOpenedEvent With AdInfo " + adInfo.ToString());
    }
    void ReardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got ReardedVideoOnAdClosedEvent With AdInfo " + adInfo.ToString());
        InvokeVideoRewardCallback(isVideoReceivedRewarded);
    }
    void ReardedVideoOnAdAvailable(IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got ReardedVideoOnAdAvailable With AdInfo " + adInfo.ToString());
        OnVideoLoad?.Invoke(true);
    }
    void ReardedVideoOnAdUnavailable()
    {
        Debug.Log("unity-script: I got ReardedVideoOnAdUnavailable");
        OnVideoLoad?.Invoke(false);
    }
    void ReardedVideoOnAdShowFailedEvent(IronSourceError ironSourceError, IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got RewardedVideoAdOpenedEvent With Error" + ironSourceError.ToString() + "And AdInfo " + adInfo.ToString());
        InvokeVideoRewardCallback(false);
    }
    void ReardedVideoOnAdRewardedEvent(IronSourcePlacement ironSourcePlacement, IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got ReardedVideoOnAdRewardedEvent With Placement" + ironSourcePlacement.ToString() + "And AdInfo " + adInfo.ToString());
        isVideoReceivedRewarded = true;
        OnVideoRewarded?.Invoke();
    }
    void ReardedVideoOnAdClickedEvent(IronSourcePlacement ironSourcePlacement, IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got ReardedVideoOnAdClickedEvent With Placement" + ironSourcePlacement.ToString() + "And AdInfo " + adInfo.ToString());
    }
    private void InvokeVideoRewardCallback(bool isReward)
    {
        if (OnPlayVideoComplete != null)
        {
            OnPlayVideoComplete.Invoke(isReward);
        }
    }

    #endregion

    #region Banner AdInfo

    void BannerOnAdLoadedEvent(IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got BannerOnAdLoadedEvent With AdInfo " + adInfo.ToString());
        isBannerLoaded = true;
        ShowBanner(true);
    }

    void BannerOnAdLoadFailedEvent(IronSourceError ironSourceError)
    {
        Debug.Log("unity-script: I got BannerOnAdLoadFailedEvent With Error " + ironSourceError.ToString());
        BannerNeedReset();
    }

    void BannerOnAdClickedEvent(IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got BannerOnAdClickedEvent With AdInfo " + adInfo.ToString());
    }

    void BannerOnAdScreenPresentedEvent(IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got BannerOnAdScreenPresentedEvent With AdInfo " + adInfo.ToString());
    }

    void BannerOnAdScreenDismissedEvent(IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got BannerOnAdScreenDismissedEvent With AdInfo " + adInfo.ToString());
        BannerNeedReset();
    }

    void BannerOnAdLeftApplicationEvent(IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got BannerOnAdLeftApplicationEvent With AdInfo " + adInfo.ToString());
        BannerNeedReset();
    }
    private void BannerNeedReset()
    {
        isBannerLoaded = false;
        isBannerCallLoaded = false;
    }
    #endregion
}
