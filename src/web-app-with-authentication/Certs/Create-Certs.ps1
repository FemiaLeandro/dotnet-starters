# WARNING: All this logic is to test locally generating the certs, not needed when going to production

[string]$Password = [Guid]::NewGuid().ToString("N");
Set-Content -Path "${PSScriptRoot}\Password.txt" -Value $Password -NoNewline;

# Run the CreateCerts.sh script on the runtime container to generate webapp and test Certificates from that machine
docker run --rm --entrypoint="/bin/bash" -v "${PSScriptRoot}:/Certs" -w="/Certs" mcr.microsoft.com/dotnet/aspnet:6.0.16 "/Certs/CreateCerts.sh";

# Set the generated Certificate environment variables (password) on the webapp root directory for the container to use
$envTemplate = Get-Content -Path "${PSScriptRoot}\ContainerCerts.env.template";
$envWPassword = $envTemplate.Replace("`$Password", $Password);
$backEndEnv = $envWPassword.Replace("`$ProjectName", "backend");
Set-Content -Path "${PSScriptRoot}\..\Templates.DockerWebAppWithAuth\ContainerCerts.env" -Value $backEndEnv;

# Add the test certificate to your local store to prompt for trust
$testCaCert = New-Object -TypeName "System.Security.Cryptography.X509Certificates.X509Certificate2" @("${PSScriptRoot}\test-ca.crt", $null);

$storeName = [System.Security.Cryptography.X509Certificates.StoreName]::Root;
$storeLocation = [System.Security.Cryptography.X509Certificates.StoreLocation]::CurrentUser
$store = New-Object System.Security.Cryptography.X509Certificates.X509Store($storeName, $storeLocation)
$store.Open(([System.Security.Cryptography.X509Certificates.OpenFlags]::ReadWrite))
try
{
    $store.Add($testCaCert)
}
finally
{
    $store.Close()
    $store.Dispose()
}
