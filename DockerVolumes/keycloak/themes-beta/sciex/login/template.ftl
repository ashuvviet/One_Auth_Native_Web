<#macro registrationLayout bodyClass="" displayInfo=false displayMessage=true>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"  "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
  <title>OneOmics</title>
  <#--  <link rel="shortcut icon" href="/favicon.ico" type="image/x-icon">  -->
  <#--  <link rel="icon" href="/favicon.ico" type="image/x-icon">  -->
  <meta name="google" value="notranslate">
  <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.2/css/bootstrap.min.css">
  <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-material-design/0.3.0/css/material-fullpalette.min.css" />
  <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-material-design/0.3.0/css/ripples.min.css" />
  <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-material-design/0.3.0/css/roboto.min.css" />
  <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-material-design/0.3.0/js/material.min.js" />
  <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-material-design/0.3.0/js/ripples.min.js" />
  <meta http-equiv="Cache-Control" content="no-cache, no-store, must-revalidate" />
  <meta http-equiv="Pragma" content="no-cache" />
  <meta http-equiv="Expires" content="0" />
	<#if properties.styles?has_content>
			<#list properties.styles?split(' ') as style>
					<link href="${url.resourcesPath}/${style}" rel="stylesheet" />
			</#list>
	</#if>
</head>
<body>

       <div class="navbar navbar-inverse">
            <div class="container-fluid">
              <div class="navbar-header">
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="navbar-inverse-collapse">
                  <span class="icon-bar"></span>
                  <span class="icon-bar"></span>
                  <span class="icon-bar"></span>
                </button>
                <a class="navbar-brand" href="https://multiomics.beta.sciex.com"><img class="logo" height="30px" src="${url.resourcesPath}/img/logo-white.png" alt="SCIEX"/></a>
              </div>
              <div class="navbar-collapse collapse navbar-inverse-collapse">

              </div>
            </div>
          </div>

          <h1>
            SCIEX Cloud
          </h1>

					<div class="alert-top">
						<div class="row">
							<#if displayMessage && message?has_content>
              	<div id="COS-Notification-messageText" class="alert alert-dismissable alert-${message.type}">
								<#list message.summary?split("<br>") as x>
									<#if x?has_next>
										${x},
									<#else>
										${x}
									</#if>
								</#list>
                </div>
              </#if>
						</div>
					</div>

					<#nested "form">

	      <script src="https://code.jquery.com/jquery-1.10.2.min.js"></script>
        <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.2/js/bootstrap.min.js"></script>
</body>
</html>
</#macro>