using Assets.Owner.Script.Network.HttpRequests;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public static class LoginUtility
{
    private const string TOKEN_FILE = ".login-data";
    
    public static string GLOBAL_TOKEN { get; private set; } = "";
    
    public static async Task<bool> Login(string username, string password)
    {
        LoginRequest request = new LoginRequest();

        var result = await request.Login(username, password);

        if (result)
        {
            GLOBAL_TOKEN = request.Token;
        }
        else
        {
            GLOBAL_TOKEN = "";
        }

        return result;
    }

    public static async Task<bool> Verfiy()
    {
        LoginRequest request = new LoginRequest(GLOBAL_TOKEN);
        return await request.VerifyToken();
    }

    public static void SaveTokenToFile()
    {
        if (GLOBAL_TOKEN?.Length > 0)
        {
            File.WriteAllText(TOKEN_FILE, GLOBAL_TOKEN);
        }
    }

    public static void LoadTokenFromFile()
    {
        try
        {
            string token = File.ReadAllText(TOKEN_FILE);
            Debug.Log(token);
            if (token?.Length > 0)
            {
                GLOBAL_TOKEN = token;
            }
            else
            {
                GLOBAL_TOKEN = "";
            }
        }
        catch (System.Exception)
        {
            GLOBAL_TOKEN = "";
        }
    }
}
