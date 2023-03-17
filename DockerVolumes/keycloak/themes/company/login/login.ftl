<#import "template.ftl" as layout>
<@layout.registrationLayout displayInfo=social.displayInfo; section>
<div class="container" style="padding-top: 175px;>
    <div class="row" style="margin-top: 25px">
      <div class="col-lg-8 col-lg-offset-2 col-sm-8 col-sm-offset-2">
        <div class="well bs-component" style="background-color: #004964;margin-bottom: 0px; color: white;">
          <h4 style="color: white">${msg("welcome")}™ Suite</h4>
        </div>
        <div>
          <div class="sciex-bar" style="background-color: #b1b1b1;"></div>
          <div class="sciex-bar" style="background-color: #007da4;"></div>
          <div class="sciex-bar" style="background-color: #80ba27;"></div>
          <div class="sciex-bar" style="background-color: #0b4661;"></div>
        </div>
        <div class="well bs-component" style="padding-bottom: 5px">
          <div class="row login-container">
            <div class="col-md-6" style="border-right: 1px solid #ccc;">
              <form class="form-horizontal" action="${url.loginAction}" method="post">
                <h4>Log in</h4>
                <div class="form-group">
                    <div class="col-lg-12">
                      <input name="username" type="text" class="form-control" placeholder="Email" id="COS-LoginDialog-userNameField"/>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-lg-12">
                        <input type="password" class="form-control" placeholder="Password" id="COS-LoginDialog-passwordField" name="password"/>
                    </div>
                </div>

                <br/>

                <div class="form-group">
                  <div class="col-lg-12">
                    <button type="submit" class="btn btn-raised btn-cta" id="COS-LoginDialog-signInBtn">Sign in</button>
                    <a href="${url.loginResetCredentialsUrl}" class="btn btn-material-white-500" id="COS-LoginDialog-forgotPasswordBtn">Forgot Password</a>
                  </div>
                </div>
              </form>
            </div>
			<div>
			    <#if realm.password && social.providers??>
					<div id="kc-social-providers" class="${properties.kcFormSocialAccountContentClass!} ${properties.kcFormSocialAccountClass!}">
						<ul class="${properties.kcFormSocialAccountListClass!} <#if social.providers?size gt 4>${properties.kcFormSocialAccountDoubleListClass!}</#if>">
							<#list social.providers as p>
								<li class="${properties.kcFormSocialAccountListLinkClass!}"><a href="${p.loginUrl}" id="zocial-${p.alias}" class="zocial ${p.providerId}"> <span>${p.displayName}</span></a></li>
							</#list>
						</ul>
					</div>
				</#if>
			</div>
            <div class="col-md-6 login-right text-right">
                <#--  <div class="login-chrome-text">
                  <img src="/assets/chrome-logo.svg" alt="Google Chrome Logo" width="24" height="24" /> <p>Built for <a href="https://www.google.com/chrome/">Google Chrome</a>.</p>
                </div>  -->
            </div>
          </div>
          <div class="row" style="border-top: 1px solid #ccc;margin-top: 20px; padding-top: 10px">
            <div class="col-md-3">
              <a href="http://www.sciex.com">
                <img src="${url.resourcesPath}/img/logo.png" alt="logo">
              </a>
            </div>
            <div class="bottom-align-text col-md-9" style="text-align: right;  padding-top: 20px">
              <#--  NOTE: This may have to be changed. I noticed in staging there is a bug fixes message.   -->
              <p class="copyright">&copy; ${msg("copyright", "${.now?string('yyyy')}")}</p>
            </div>
          </div>
        </div>
      </div>
    </div>
</div>
<script>
   window.localStorage.removeItem('oneomics.auth');
</script>

</@layout.registrationLayout>
