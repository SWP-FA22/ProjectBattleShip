using System.Collections;
using System.Collections.Generic;
using Assets.Owner.Script.Util;
using Owner.Script.GameData;
using Owner.Script.GameData.HandleData;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using TMPro;

public class CurentRankingManage : MonoBehaviour
{
    public Image RankImage;

    public TextMeshProUGUI scoreText;
    private int ScoreRank;
    PlayerData playerData;

    string addressableOfRank;
    // Start is called before the first frame update
    async void  Start()
    {
        this.playerData = await PlayerUtility.GetMyPlayerData();
        if(this.playerData.Rank<1600){
            this.addressableOfRank = "bonze";
        } else if(this.playerData.Rank<2000){
            this.addressableOfRank = "sliver";
        } else if(this.playerData.Rank<2400){
            this.addressableOfRank = "gold";
        } else {
            this.addressableOfRank = "master";
        }
        Addressables.LoadAssetAsync<Sprite>(addressableOfRank).Completed += (rank) => { this.RankImage.sprite = rank.Result; };
        this.scoreText.text = this.playerData.Rank.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
