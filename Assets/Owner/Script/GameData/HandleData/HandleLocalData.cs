using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class HandleLocalData 
{
    public void SaveData<T>(string key,T data){
        if(data==null){
            return;
        }
        try{
            PlayerPrefs.SetString(key,JsonConvert.SerializeObject(data));
            Debug.Log(PlayerPrefs.GetString(key));
            Debug.Log("save successfully");
        } catch{
            Debug.Log("cannot save data");
        }
        
    }

    public T LoadData<T>(string key) {
       T resultData;
       try{
            resultData = JsonConvert.DeserializeObject<T>(PlayerPrefs.GetString(key));
            Debug.Log(resultData);
            return resultData;
       }catch{
            Debug.Log("cannot load data");
       }
       return default(T);
    }
}
