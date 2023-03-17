<#import "template.ftl" as layout>
<@layout.registrationLayout displayInfo=social.displayInfo; section>



<div class="" style="display: flex; height: 65.7vh; justify-content: center; align-items: center;">
  <div style="width: 90%; min-width: 400px; max-width: 750px;">
    <div class="well bs-component" style="background-color: #004964;margin-bottom: 0px; color: white;">
      <h4 style="color: white">Set Password</h4>
    </div>
    <div class="well bs-component">
      <div class="row">
        <div class="col-md-12">
          <form class="form-horizontal" id="kc-passwd-update-form" action="${url.loginAction}" method="post" accept-charset="UTF-8">
            <fieldset>
              <div class="form-group">
                <div class="col-lg-12">
                    <input type="password" name="password-new" id="password-new" value="" class="form-control" placeholder="New Password" required="required">
                </div>
              </div>
              <div class="form-group">
                <div class="col-lg-12">
                    <input type="password" name="password-confirm" id="password-confirm" value="" class="form-control" placeholder="Confirm Password" required="required">
                </div>
              </div>
              <div class="form-group">
                <div class="col-lg-12">
                  <input type="submit" name="commit" value="Submit" class="btn btn-material-green-500" data-disable-with="Submit">
                  <a class="btn btn-material-white-500" href="${url.loginUrl}">Cancel</a>
                </div>
              </div>
              <div class="form-group">
                  <div class="col-lg-12">
                    <label id="COS-ChangePassword-policy">Please ensure that your password is at least 8 characters long with at least 1 uppercase letter, 1 lowercase letter, 1 special character and 1 digit.  Password cannot match any of your 3 most recent passwords.</label>
                  </div>
              </div>
            </fieldset>
          </form>
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


</@layout.registrationLayout>