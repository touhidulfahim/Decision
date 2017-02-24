﻿using System;

namespace NTierMvcFramework.Common.MvcToolkit.Security.HiddenField
{
    public interface IRijndaelStringEncrypter : IDisposable
    {
        string Encrypt(string value);
        string Decrypt(string value);
    }
}