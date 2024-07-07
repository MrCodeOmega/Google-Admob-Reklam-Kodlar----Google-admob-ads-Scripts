using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
public class AdsManager : MonoBehaviour
{
#if UNITY_ANDROID
    private string _adUnitId = "gecis reklam IDniz - Intersitial ads ID";
    private string _adUnitRewardedId = "odullu reklam IDniz - Rewarded ads ID";
#elif UNITY_IPHONE
  private string _adUnitId = "iphonelar icin gecis id - for Iphones intersitial Ads ID";
  private string _adUnitRewardedId = "iphonelar icin odullu - for Iphones rewarded Ads ID";
#else
  private string _adUnitId = "unused";
  private string _adUnitRewardedId = "---------";
#endif

    private InterstitialAd _interstitialAd;
    private RewardedAd _rewardedAd;
    private void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
        });
        LoadRewardedAd();
        LoadInterstitialAd();
    }


    public void LoadInterstitialAd()
    {
        // Clean up the old ad before loading a new one.
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        Debug.Log("Loading the interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        InterstitialAd.Load(_adUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Reklam su tip seyler gercekleserek yuklendi - Ads loaded with this things: "
                          + ad.GetResponseInfo());

                _interstitialAd = ad;
                RegisterReloadHandler(_interstitialAd);
            });
    }


    public void ShowInterstitialAd()
    {
        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            Debug.Log("Reklam gosteriliyor. - Ads showing.");
            _interstitialAd.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet - Reklam henuz hazir degil.");
        }
    }


    private void RegisterReloadHandler(InterstitialAd interstitialAd)
    {
        // Raised when the ad closed full screen content.
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Reklam izlenmeden kapantildi - Ads Closed with unwatched.");

            // Reload the ad so that we can show another as soon as possible.
            LoadInterstitialAd();
        };
        // Raised when the ad failed to open full screen content.
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            LoadInterstitialAd();
        };
    }



    /////////////////////////Rewarded Ad Section //////////////////////

    public void LoadRewardedAd()
    {
        // Clean up the old ad before loading a new one.
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad - Odullu reklam yukleniyor.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(_adUnitRewardedId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                _rewardedAd = ad;
                RewardedRegisterReloadHandler(_rewardedAd);
            });
    }

    public void ShowRewardedAd()
    {


        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                Debug.Log("Reklam izlendi ve sunlar oldu : - Ads watched and these things happend :");
                // Oyuncuya odul olarak verilecek seyler buraya
            });
        }
    }


    private void RewardedRegisterReloadHandler(RewardedAd ad)
    {
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded Ad full screen content closed.");

            // Reload the ad so that we can show another as soon as possible.
            _rewardedAd.Destroy();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
            LoadRewardedAd();
        };
    }

}
