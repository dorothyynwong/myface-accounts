using System;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

public class KeyStore
{
    public RsaSecurityKey RsaSecurityKey { get; private set; }

    public KeyStore()
    {
        RsaSecurityKey = new RsaSecurityKey(ConfigureRsaKey());
    }

    private RSA ConfigureRsaKey()
    {
        // Check if the RSA key exists
        var privateKeyXml = Environment.GetEnvironmentVariable("RSA_PRIVATE_KEY");

        RSA rsa;
        if (string.IsNullOrEmpty(privateKeyXml))
        {
            // If the key doesn't exist, generate a new one
            rsa = RSA.Create(2048);
            privateKeyXml = rsa.ToXmlString(true); // Export the private key
            Environment.SetEnvironmentVariable("RSA_PRIVATE_KEY", privateKeyXml);

            // Optionally, save the key in a more secure storage, like AWS Secrets Manager
        }
        else
        {
            // Load the existing private key
            rsa = RSA.Create();
            rsa.FromXmlString(privateKeyXml);
        }

        return rsa;
    }
}
