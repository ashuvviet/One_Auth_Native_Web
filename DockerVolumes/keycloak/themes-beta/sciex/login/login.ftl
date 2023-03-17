<#import "template.ftl" as layout>
<@layout.registrationLayout displayInfo=social.displayInfo; section>
<div class="container" style="padding-top: 175px;>
    <div class="row" style="margin-top: 25px">
      <div class="col-lg-8 col-lg-offset-2 col-sm-8 col-sm-offset-2">
        <div class="well bs-component" style="background-color: #004964;margin-bottom: 0px; color: white;">
          <h4 style="color: white">${msg("welcome")}</h4>
        </div>
        <div class="well bs-component">
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
                <div class="form-group">
                  <div class="col-lg-12">
                    <button type="submit" class="btn btn-material-green-500" id="COS-LoginDialog-signInBtn">Sign in</button>
                    <a href="${url.loginResetCredentialsUrl}" class="btn btn-material-white-500" id="COS-LoginDialog-forgotPasswordBtn">Forgot Password</a>
                  </div>
                </div>
              </form>
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
