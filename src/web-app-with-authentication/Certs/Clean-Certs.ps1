# Remove all Root certificates from Contoso
$ConfirmPreference = 'None';

Set-Location Cert:\CurrentUser\Root\;

$oldCerts = Get-ChildItem -Recurse |
Where-Object -Property Subject -EQ "O=Contoso";
$oldCerts;
$oldCerts | Remove-Item;
