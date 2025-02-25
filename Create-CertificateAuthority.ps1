$c = New-SelfSignedCertificate -Subject "TernaTestCA" -KeyExportPolicy Exportable -CertStoreLocation Cert:\CurrentUser\My -NotAfter "2035-01-01" -KeyUsage CertSign,CRLSign
[IO.File]::WriteAllBytes("$pwd\ca.pfx", $c.Export([Security.Cryptography.X509Certificates.X509ContentType]::Pfx))
$c.PrivateKey.ExportRSAPrivateKeyPem() > ca.pem
$c.ExportCertificatePem() >> ca.pem
$c.ExportCertificatePem() > ca.cer
