<#import "template.ftl" as layout>
<@layout.registrationLayout displayMessage=false; section>
    <#if section = "title">
        ${msg("errorTitle")?no_esc}
    <#elseif section = "header">
        ${msg("errorTitleHtml")?no_esc}
    <#elseif section = "form">
        <div class="" style="display: flex; height: 65.7vh; justify-content: center; align-items: center;">
            <div style="width: 90%; min-width: 400px; max-width: 750px;" id="COS-ErrorPanel">
                <div class="well bs-component" style="background-color: #004964;margin-bottom: 0px; color: white;">
                    <h4 style="color: white">OneOmics</h4>
                </div>
                <div class="well bs-component">
                    <div class="row">
                        <div class="col-md-12">
                            <h3>${message.summary}</h3>
                            <#if client?? && client.baseUrl?has_content>
                                <a id="backToApplication" href="${client.baseUrl}" class="btn btn-raised" style="background-color: #00afdb; color: #fff; font-weight: 500;">Go to OneOmics</a>
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