<#import "template.ftl" as layout>
<@layout.registrationLayout displayMessage=false; section>
    <#if section = "header">
        <#if messageHeader??>
        ${messageHeader}
        <#else>
        ${message.summary}
        </#if>
    <#elseif section = "form">
        <div class="" style="display: flex; height: 65.7vh; justify-content: center; align-items: center;">
            <div style="width: 90%; min-width: 400px; max-width: 750px;" id="COS-ErrorPanel">
                <div class="well bs-component" style="background-color: #004964;margin-bottom: 0px; color: white;">
                    <h4 style="color: white">OneOmics</h4>
                </div>
                <div class="well bs-component">
                    <div class="row">
                        <div class="col-md-12">
                            <p class="instruction">${message.summary}<#if requiredActions??><#list requiredActions>: <b><#items as reqActionItem>${msg("requiredAction.${reqActionItem}")}<#sep>, </#items></b></#list><#else></#if></p>
                            <#if skipLink??>
                            <#else>
                                <#if pageRedirectUri??>
                                    <p><a href="${pageRedirectUri}" class="btn btn-raised" style="background-color: #00afdb; color: #000EEE; font-weight: 500;">${kcSanitize(msg("backToApplication"))?no_esc}</a></p>
                                <#elseif actionUri??>
                                    <p><a href="${actionUri}" class="btn btn-raised" style="background-color: #00afdb; color: #000EEE; font-weight: 500;">${kcSanitize(msg("proceedWithAction"))?no_esc}</a></p>
                                <#elseif client.baseUrl??>
                                    <p><a href="${client.baseUrl}" class="btn btn-raised" style="background-color: #00afdb; color: #000EEE; font-weight: 500;">${kcSanitize(msg("backToApplication"))?no_esc}</a></p>
                                </#if>
                            </#if>
                        </div>
                    </div>
                    <div class="row" style="border-top: 1px solid #ccc;margin-top: 20px; padding-top: 10px">
                        <div class="col-md-6">
                        <a href="http://www.sciex.com">
                            <img src="${url.resourcesPath}/img/logo.png" alt="logo">
                        </a>
                        </div>
                        <div class="bottom-align-text col-md-6" style="text-align: right; color: #bbb; padding-top: 20px">

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </#if>
</@layout.registrationLayout>