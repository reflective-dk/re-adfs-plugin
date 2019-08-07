# Installation

## Dependencies

Add to c:\Windows\ADFS
ReflectiveAttributeStore.dll
RestSharp.dll
Newtonsoft.Json.dll

*NB when updating the DLL's you need to stop the service, because it will not allow overriding files.

Windows Identity Foundation 3.5 feature must be installed:
check or install here:
Open 'Server Manager'
Select menu 'Manage' -> 'Add roles and features'
Skip forward to step 'Features'
Make sure Windows Identity Foundation 3.5 is installed or install it.

Afterwards you need to restart the server.

## Add custom Attribute Store

Open 'Server Manager', select tools -> 'AD FS management'
Service -> Attribute Stores -> 
Add Custom Attribute Store...
Add Attribute Store
Set display name to ReflectiveAttributeStore
In class name input: ReflectiveAttributeStore.MainClass, ReflectiveAttributeStore

Then in optional parameters, input:
ReflectiveUrl , example https://test.reflective.dk
Username , example myname
Password
Context, example {"domain": "hjertekoebing"}

## Verify correct installation
To check for correct instantiation, open 'Event Viewer'
For Active Directory Federated Service check for log item: "Attribute store 'ReflectiveStoreAttribute' is loaded successfully"

In order to issue claim as a shortname, add a Claim Description,
Example:
Claim Identifier = http://reflective.dk/cprnumber
Displayname = cprnumber
shortname = cpr

## Logging
The log is present at 'c:/Logs/reflective/adfs-plugin.txt'
If debug is 'true', more info will be available here