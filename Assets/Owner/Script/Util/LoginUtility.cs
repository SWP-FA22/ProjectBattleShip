using Assets.Owner.Script.Network.HttpRequests;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class LoginUtility
{
    public string Token { get; private set; }

    public async Task<bool> Login(string username, string password)
    {
        LoginRequest request = new LoginRequest();

        var result = await request.Login(username, password);

        if (result)
        {
            Token = request.Token;
        }
        else
        {
            Token = "";
        }

        return result;
    }

    public async Task<bool> Verfiy()
    {
        LoginRequest request = new LoginRequest(Token);
        
        return await request.VerifyToken();
    }

    public void SaveTokenToFile(string filename)
    {
        if (Token?.Length > 0)
        {
            File.WriteAllText(filename, Token);
        }
    }

    public void LoadTokenFromFile(string filename)
    {
        string token = File.ReadAllText(filename);

        if (token?.Length > 0)
        {
            Token = token;
        }
    }
}
