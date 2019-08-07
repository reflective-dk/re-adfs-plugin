# Installation

## Dependencies

Add to c:\Windows\ADFS

ReflectiveAttributeStore.dll
RestSharp.dll
Newtonsoft.Json.dll

Compiled to WIndows 2012 server [ReflectiveAttributeStore/bin/Release](here).

*NB when updating the DLL's you need to stop the service, because it will not allow overriding files.

Windows Identity Foundation 3.5 feature must be installed:
check or install here:
1. Open 'Server Manager'
2. Select menu 'Manage' -> 'Add roles and features'
3. Skip forward to step 'Features'
4. Make sure Windows Identity Foundation 3.5 is installed or install it.

## Add custom Attribute Store

1. Open 'Server Manager'
2. select tools -> 'AD FS management'
3. Service -> Attribute Stores -> Add Custom Attribute Store...
4. Add Attribute Store

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

## Example of claim rules using plugin
Example rules [example](here).

In power shell add like so:
```sh
Set-AdfsRelyingPartyTrust -TargetName "Reflective" -IssuanceTransformRulesFile ./<name of file>.txt
```
