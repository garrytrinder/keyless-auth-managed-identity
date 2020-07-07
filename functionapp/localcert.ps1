$cert = New-SelfSignedCertificate -CertStoreLocation "cert:\CurrentUser\My" -Subject "CN=KeylessAuthLocalDev" -KeySpec KeyExchange
$keyValue = [System.Convert]::ToBase64String($cert.GetRawCertData())
$sp = New-AzADServicePrincipal -DisplayName KeylessAuthLocalDev -CertValue $keyValue -EndDate $cert.NotAfter  -StartDate $cert.NotBefore
$tenant = Get-AzTenant
Write-Host "AppId: $($sp.ApplicationId)"
Write-Host "Tenant Id: $($tenant.Id)"
Write-Host "Thumbprint: $($cert.Thumbprint)"