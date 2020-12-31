using Microsoft.AspNetCore.Mvc;
using NBitcoin;
using System;

namespace Stratis.STOPlatform.Core
{
    public static class Utility
    {
        public static string ControllerName<TController>() where TController : Controller
        {
            var name = typeof(TController).Name;
            return name.Substring(0, name.Length - "Controller".Length);
        }

        public static bool ValidateAddress(string sender, Network network)
        {
            try
            {
                BitcoinPubKeyAddress.IsValid(sender, network);
                return true;
            }
            catch { }

            return false;
        }
    }
}
