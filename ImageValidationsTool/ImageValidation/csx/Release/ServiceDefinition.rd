<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="ImageValidation" generation="1" functional="0" release="0" Id="298be618-29f5-4e07-8b48-b9f29cf6bde6" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="ImageValidationGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="ImageValidation.Service:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/ImageValidation/ImageValidationGroup/LB:ImageValidation.Service:Endpoint1" />
          </inToChannel>
        </inPort>
        <inPort name="ImageValidation.Service:Endpoint2" protocol="https">
          <inToChannel>
            <lBChannelMoniker name="/ImageValidation/ImageValidationGroup/LB:ImageValidation.Service:Endpoint2" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="Certificate|ImageValidation.Service:tools.imagevalidate.com" defaultValue="">
          <maps>
            <mapMoniker name="/ImageValidation/ImageValidationGroup/MapCertificate|ImageValidation.Service:tools.imagevalidate.com" />
          </maps>
        </aCS>
        <aCS name="ImageValidation.Service:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/ImageValidation/ImageValidationGroup/MapImageValidation.Service:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="ImageValidation.Service:StorageConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/ImageValidation/ImageValidationGroup/MapImageValidation.Service:StorageConnectionString" />
          </maps>
        </aCS>
        <aCS name="ImageValidation.ServiceInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/ImageValidation/ImageValidationGroup/MapImageValidation.ServiceInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:ImageValidation.Service:Endpoint1">
          <toPorts>
            <inPortMoniker name="/ImageValidation/ImageValidationGroup/ImageValidation.Service/Endpoint1" />
          </toPorts>
        </lBChannel>
        <lBChannel name="LB:ImageValidation.Service:Endpoint2">
          <toPorts>
            <inPortMoniker name="/ImageValidation/ImageValidationGroup/ImageValidation.Service/Endpoint2" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapCertificate|ImageValidation.Service:tools.imagevalidate.com" kind="Identity">
          <certificate>
            <certificateMoniker name="/ImageValidation/ImageValidationGroup/ImageValidation.Service/tools.imagevalidate.com" />
          </certificate>
        </map>
        <map name="MapImageValidation.Service:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/ImageValidation/ImageValidationGroup/ImageValidation.Service/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapImageValidation.Service:StorageConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/ImageValidation/ImageValidationGroup/ImageValidation.Service/StorageConnectionString" />
          </setting>
        </map>
        <map name="MapImageValidation.ServiceInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/ImageValidation/ImageValidationGroup/ImageValidation.ServiceInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="ImageValidation.Service" generation="1" functional="0" release="0" software="D:\Project\New-ImageValidation\New\ImageValidation\csx\Release\roles\ImageValidation.Service" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
              <inPort name="Endpoint2" protocol="https" portRanges="443">
                <certificate>
                  <certificateMoniker name="/ImageValidation/ImageValidationGroup/ImageValidation.Service/tools.imagevalidate.com" />
                </certificate>
              </inPort>
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="StorageConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;ImageValidation.Service&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;ImageValidation.Service&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;e name=&quot;Endpoint2&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="ImageValidation.Service.svclog" defaultAmount="[1000,1000,1000]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
            <storedcertificates>
              <storedCertificate name="Stored0tools.imagevalidate.com" certificateStore="My" certificateLocation="User">
                <certificate>
                  <certificateMoniker name="/ImageValidation/ImageValidationGroup/ImageValidation.Service/tools.imagevalidate.com" />
                </certificate>
              </storedCertificate>
            </storedcertificates>
            <certificates>
              <certificate name="tools.imagevalidate.com" />
            </certificates>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/ImageValidation/ImageValidationGroup/ImageValidation.ServiceInstances" />
            <sCSPolicyUpdateDomainMoniker name="/ImageValidation/ImageValidationGroup/ImageValidation.ServiceUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/ImageValidation/ImageValidationGroup/ImageValidation.ServiceFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="ImageValidation.ServiceUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="ImageValidation.ServiceFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="ImageValidation.ServiceInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="7f3a0a96-5b3f-4308-baa6-74ed09b98ca6" ref="Microsoft.RedDog.Contract\ServiceContract\ImageValidationContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="8a66f27c-68fe-41a2-ad7d-13e1f9cea467" ref="Microsoft.RedDog.Contract\Interface\ImageValidation.Service:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/ImageValidation/ImageValidationGroup/ImageValidation.Service:Endpoint1" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="69810d06-b4c4-4be6-8569-08da47bb95dd" ref="Microsoft.RedDog.Contract\Interface\ImageValidation.Service:Endpoint2@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/ImageValidation/ImageValidationGroup/ImageValidation.Service:Endpoint2" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>