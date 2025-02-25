param(
    [string]$Subject
)

$ca = [Security.Cryptography.X509Certificates.X509Certificate2]::new([IO.File]::ReadAllBytes("$pwd\ca.pfx"))
$c = New-SelfSignedCertificate -Subject "mysite1.terna" -KeyExportPolicy Exportable -CertStoreLocation Cert:\CurrentUser\My -NotAfter "2027-01-01" -Signer $ca
$c.PrivateKey.ExportRSAPrivateKeyPem() > "$Subject.pem"
$c.ExportCertificatePem() >> "$Subject.pem"
$c.ExportCertificatePem() > "$Subject.cer"
