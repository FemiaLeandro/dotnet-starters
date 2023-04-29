# .NET 6 Razor Web Pages starter

This starter project should eliminate (or at least reduce to a minimum) the whole setup phase for the solution, and also adds the possibility to remove or customize whatever you need as the code is thoroughly commented, explaning each part of the configuration to the best of my ability.

Features:
- Individual accounts authentication using .NET handlers for fast and safe customizable auth without the need of external service providers (also an option if you need!)
- Unscaffolded Identity pages to customize whatever you need (also, Bootstrap and jQuery installed by default on ASP.NET projects)
- Localization support and example in Login page
- In memory database support for fast local tests or demos
- Docker and HTTPS support (debugging included, with custom self signed SSL certificate creation for local development)
- LocaLDB MS SQL support for Development deployments or standard MSSQL connections
- Entity Framework code-first approach with in-code migrations (no out-of-control database changes :D)

I hope this helps anyone in need of something similar for a quick project setup

Thanks to https://github.com/NCarlsonMSFT/CertExample for the required commands and guidance on how to generate your own self-signed certs and pointing the Docker runtime container to use those!

### Required config to run the project

Review the **'.\Create-Certs.ps1'** inside the **'.\src\web-app-with-authentication\Certs'** folder in detail.

Set the directory to **'.\src\web-app-with-authentication\Certs'** folder from an elevated PowerShell script ([tested using PowerShell 7 stable](https://github.com/PowerShell/PowerShell/tags)) and run the **'.\Create-Certs.ps1'** script.

This will generate custom self-signed certificates from the runtime .NET 6 container and trust them for local development, you will need to press **Yes** when the trust certificate popup appears.

Finally, run the project under the **Docker** configuration and check the SSL certificate for the WebApp (depicted in Chrome):

![Backend generated Certificate](readme-assets\backend-certificate-contoso-chrome.png)

As you can see, the Certificate name is different, since in the PowerShell script we trusted the test Certificate from Contoso and subsequently, all Certificates signed by the same organization are trusted locally.

Microsoft already included this functionality into the Visual Studio extension that connects into the Docker deamon, but since you probably will need to deal with this when setting up your deployment, I included these steps here for understading how a Certificate is generated, stored, and trusted.

### Removing/untrusting the test Certificate manually

The **'.\Clean-Certs.ps1'** script still does not work (I'm still working on it), but there's a workaround:

Hit the Windows key and type **mmc**, open that.

Go to File -> Add or Remove Snap-In -> Select 'Certificates' on the left list and click **Add** -> Select 'My User Account' -> Click **Finish** -> Click **OK**

In the main window on the left list, go to:   
Certificates - Current User -> Trusted Root Certification Authorities -> Certificates  
and search for the Contoso certificate:

![Contoso Certificate](readme-assets\trusted-contoso-certificate.png)

Make sure that the **Thumbprint** for the certificate matches the output from the following PowerShell command (run in elevated prompt):

 - Set-Location Cert:\CurrentUser\Root\; $oldCerts = Get-ChildItem -Recurse | Where-Object -Property Subject -EQ "O=Contoso"; $oldCerts; Set-Location C:\;

![PowerShell Output](readme-assets\powershell-output-certificate.png)

Validate against your Certificate's Thumbprint and only delete if they match. THIS IS VERY IMPORTANT TO AVOID DAMAGING ANY CERTIFICATE FUNCTIONALITY IN YOUR SYSTEM. I will continue working on updating the script to work automatically so you can avoid this step. CLARIFICATION: Thumbprints in the screenshots do not match, do not worry, this is because a different set of Certificates was used for the two screenshots, another property you can check to make sure that the Certificate you've got is the right one is checking the Issued Date, and making sure that it's the same as the date you've ran the **.\CreateScripts.ps1** script.

![Certificate Thumbprint](readme-assets\trusted-contoso-certificate-thumbprint.png)

Delete your self-signed Certificate, this will untrust any Certificate generated by your script and disable HTTPS for existing certificates, so you will need to re-generate them if you wish to continue using them. 

![Untrusted Certificate](readme-assets\ssl-untrusted.png)

### Additional comments

As you will see in the Program.cs class, the Development appsettings.json use the In Memory database for running, and that requires no migrations.

In case you get to the point of using a database, you will need to generate the migrations. Steps for that can be found below:
(I already ran these steps on this repo so you can check the generated code beforehand, but you can delete the Migrations folder and start again)

- Setup the database connection string in the appsettings file where your VS is pointing, in my case, I updated the appsettings.Development.json file
- Run the following command in the Package Manager Console **Add-Migration InitialCreate**
- That command will create your Migrations folder and your InitialCreate migration, check the results
- To update the database set in the connection string, run **Update-Database** in the Package Manager Console, and you should get all the modifications done as the output in the console

