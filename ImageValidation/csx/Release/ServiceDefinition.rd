<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="ImageValidation" generation="1" functional="0" release="0" Id="81193a23-b24f-4466-8f20-ec04dd12f245" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="ImageValidationGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="ImageValidation.Service:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/ImageValidation/ImageValidationGroup/LB:ImageValidation.Service:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="ImageValidation.Service:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/ImageValidation/ImageValidationGroup/MapImageValidation.Service:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
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
      </channels>
      <maps>
        <map name="MapImageValidation.Service:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/ImageValidation/ImageValidationGroup/ImageValidation.Service/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
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
          <role name="ImageValidation.Service" generation="1" functional="0" release="0" software="D:\Project\ImageValidation-ValidatorToolAndClientTool\ImageValidation\csx\Release\roles\ImageValidation.Service" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;ImageValidation.Service&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;ImageValidation.Service&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="ImageValidation.Service.svclog" defaultAmount="[1000,1000,1000]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
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
    <implementation Id="969a67c5-dc95-4712-bb8e-8630d0a1af99" ref="Microsoft.RedDog.Contract\ServiceContract\ImageValidationContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="4847dfc3-1258-43b3-9555-85f4998b249d" ref="Microsoft.RedDog.Contract\Interface\ImageValidation.Service:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/ImageValidation/ImageValidationGroup/ImageValidation.Service:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>