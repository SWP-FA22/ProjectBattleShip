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
        } catch{
            Debug.Log("cannot save data");
        }
        
    }

    public T LoadData<T>(string key) {
       T resultData;
       try{
            resultData = (T)JsonConvert.DeserializeObject(PlayerPrefs.GetString(key));
            return resultData;
       }catch{
            Debug.Log("cannot saveload data");
       }
       return default(T);
    }
}
