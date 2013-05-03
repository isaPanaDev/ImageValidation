<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="AzureAdminWebPortal" generation="1" functional="0" release="0" Id="e9634e15-8fe9-430c-a72b-6b3e109e5c33" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="AzureAdminWebPortalGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="AdminWebPortal:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/AzureAdminWebPortal/AzureAdminWebPortalGroup/LB:AdminWebPortal:Endpoint1" />
          </inToChannel>
        </inPort>
        <inPort name="AdminWebPortal:Endpoint2" protocol="https">
          <inToChannel>
            <lBChannelMoniker name="/AzureAdminWebPortal/AzureAdminWebPortalGroup/LB:AdminWebPortal:Endpoint2" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="AdminWebPortal:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/AzureAdminWebPortal/AzureAdminWebPortalGroup/MapAdminWebPortal:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="AdminWebPortalInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/AzureAdminWebPortal/AzureAdminWebPortalGroup/MapAdminWebPortalInstances" />
          </maps>
        </aCS>
        <aCS name="Certificate|AdminWebPortal:imagevalidate.com" defaultValue="">
          <maps>
            <mapMoniker name="/AzureAdminWebPortal/AzureAdminWebPortalGroup/MapCertificate|AdminWebPortal:imagevalidate.com" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:AdminWebPortal:Endpoint1">
          <toPorts>
            <inPortMoniker name="/AzureAdminWebPortal/AzureAdminWebPortalGroup/AdminWebPortal/Endpoint1" />
          </toPorts>
        </lBChannel>
        <lBChannel name="LB:AdminWebPortal:Endpoint2">
          <toPorts>
            <inPortMoniker name="/AzureAdminWebPortal/AzureAdminWebPortalGroup/AdminWebPortal/Endpoint2" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapAdminWebPortal:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/AzureAdminWebPortal/AzureAdminWebPortalGroup/AdminWebPortal/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapAdminWebPortalInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/AzureAdminWebPortal/AzureAdminWebPortalGroup/AdminWebPortalInstances" />
          </setting>
        </map>
        <map name="MapCertificate|AdminWebPortal:imagevalidate.com" kind="Identity">
          <certificate>
            <certificateMoniker name="/AzureAdminWebPortal/AzureAdminWebPortalGroup/AdminWebPortal/imagevalidate.com" />
          </certificate>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="AdminWebPortal" generation="1" functional="0" release="0" software="D:\Project\AdminWebPanel\AzureAdminWebPortal\csx\Release\roles\AdminWebPortal" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
              <inPort name="Endpoint2" protocol="https" portRanges="443">
                <certificate>
                  <certificateMoniker name="/AzureAdminWebPortal/AzureAdminWebPortalGroup/AdminWebPortal/imagevalidate.com" />
                </certificate>
              </inPort>
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;AdminWebPortal&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;AdminWebPortal&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;e name=&quot;Endpoint2&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
            <storedcertificates>
              <storedCertificate name="Stored0imagevalidate.com" certificateStore="My" certificateLocation="User">
                <certificate>
                  <certificateMoniker name="/AzureAdminWebPortal/AzureAdminWebPortalGroup/AdminWebPortal/imagevalidate.com" />
                </certificate>
              </storedCertificate>
            </storedcertificates>
            <certificates>
              <certificate name="imagevalidate.com" />
            </certificates>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/AzureAdminWebPortal/AzureAdminWebPortalGroup/AdminWebPortalInstances" />
            <sCSPolicyUpdateDomainMoniker name="/AzureAdminWebPortal/AzureAdminWebPortalGroup/AdminWebPortalUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/AzureAdminWebPortal/AzureAdminWebPortalGroup/AdminWebPortalFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="AdminWebPortalUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="AdminWebPortalFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="AdminWebPortalInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="c2ddc210-99c3-4c68-8935-ab196c1ef640" ref="Microsoft.RedDog.Contract\ServiceContract\AzureAdminWebPortalContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="cbce226c-8617-41b0-ac5a-7858ad4ae6d4" ref="Microsoft.RedDog.Contract\Interface\AdminWebPortal:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/AzureAdminWebPortal/AzureAdminWebPortalGroup/AdminWebPortal:Endpoint1" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="05f739df-28cf-4e1d-a667-7a34df4243fd" ref="Microsoft.RedDog.Contract\Interface\AdminWebPortal:Endpoint2@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/AzureAdminWebPortal/AzureAdminWebPortalGroup/AdminWebPortal:Endpoint2" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>