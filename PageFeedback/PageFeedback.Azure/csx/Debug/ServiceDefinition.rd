﻿<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="PageFeedback.Azure" generation="1" functional="0" release="0" Id="f8f52a34-d0af-42a4-9fa3-526fce34c1ff" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="PageFeedback.AzureGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="PageFeedback.Web:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/PageFeedback.Azure/PageFeedback.AzureGroup/LB:PageFeedback.Web:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="PageFeedback.WebInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/PageFeedback.Azure/PageFeedback.AzureGroup/MapPageFeedback.WebInstances" />
          </maps>
        </aCS>
        <aCS name="PageFeedback.WorkerInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/PageFeedback.Azure/PageFeedback.AzureGroup/MapPageFeedback.WorkerInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:PageFeedback.Web:Endpoint1">
          <toPorts>
            <inPortMoniker name="/PageFeedback.Azure/PageFeedback.AzureGroup/PageFeedback.Web/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapPageFeedback.WebInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/PageFeedback.Azure/PageFeedback.AzureGroup/PageFeedback.WebInstances" />
          </setting>
        </map>
        <map name="MapPageFeedback.WorkerInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/PageFeedback.Azure/PageFeedback.AzureGroup/PageFeedback.WorkerInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="PageFeedback.Web" generation="1" functional="0" release="0" software="D:\projects\PageFeedback\PageFeedback\PageFeedback.Azure\csx\Debug\roles\PageFeedback.Web" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="-1" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;PageFeedback.Web&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;PageFeedback.Web&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;PageFeedback.Worker&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/PageFeedback.Azure/PageFeedback.AzureGroup/PageFeedback.WebInstances" />
            <sCSPolicyUpdateDomainMoniker name="/PageFeedback.Azure/PageFeedback.AzureGroup/PageFeedback.WebUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/PageFeedback.Azure/PageFeedback.AzureGroup/PageFeedback.WebFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
        <groupHascomponents>
          <role name="PageFeedback.Worker" generation="1" functional="0" release="0" software="D:\projects\PageFeedback\PageFeedback\PageFeedback.Azure\csx\Debug\roles\PageFeedback.Worker" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="-1" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <settings>
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;PageFeedback.Worker&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;PageFeedback.Web&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;PageFeedback.Worker&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/PageFeedback.Azure/PageFeedback.AzureGroup/PageFeedback.WorkerInstances" />
            <sCSPolicyUpdateDomainMoniker name="/PageFeedback.Azure/PageFeedback.AzureGroup/PageFeedback.WorkerUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/PageFeedback.Azure/PageFeedback.AzureGroup/PageFeedback.WorkerFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="PageFeedback.WebUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyUpdateDomain name="PageFeedback.WorkerUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="PageFeedback.WebFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyFaultDomain name="PageFeedback.WorkerFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="PageFeedback.WebInstances" defaultPolicy="[1,1,1]" />
        <sCSPolicyID name="PageFeedback.WorkerInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="89866bce-9e35-4ac7-bf54-3cf3e692c4ab" ref="Microsoft.RedDog.Contract\ServiceContract\PageFeedback.AzureContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="2b5888b8-ba6c-4b5e-aff9-0de3a0753fac" ref="Microsoft.RedDog.Contract\Interface\PageFeedback.Web:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/PageFeedback.Azure/PageFeedback.AzureGroup/PageFeedback.Web:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>