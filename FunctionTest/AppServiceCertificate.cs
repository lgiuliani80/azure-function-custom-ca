using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FunctionTest
{
    public static class AppServiceCertificate
    {
        public static X509Certificate2? GetCertificate(string thumbprint)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                using X509Store store = new(StoreName.My, StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly);
                return store.Certificates
                    .Find(X509FindType.FindByThumbprint, thumbprint, false)
                    .FirstOrDefault();
            }
            else
            {
                var certPath = $"/var/ssl/certs/{thumbprint.ToUpper()}.der";
                if (File.Exists(certPath))
                    return new X509Certificate2(certPath);

                var pfxPath = $"/var/ssl/certs/{thumbprint.ToUpper()}.pfx";
                if (File.Exists(pfxPath))
                    return new X509Certificate2(pfxPath);

                return null;
            }
        }
    }
}
