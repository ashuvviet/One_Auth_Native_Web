<#import "template.ftl" as layout>
<@layout.mainLayout active='password' bodyClass='password'; section>

    <div class="container" style="padding-top: 175px">
    <div class="row" style="margin-top: 25px">
      <div class="col-lg-8 col-lg-offset-2 col-sm-8 col-sm-offset-2">
        <div class="well bs-component" style="background-color: #004964;margin-bottom: 0px; color: white;">
          <h4 style="color: white">Change Password</h4>
        </div>
        <div class="well bs-component">
          <div class="row">
            <div class="col-md-12">
              <form action="${url.passwordUrl}" class="form-horizontal" method="post">
                <input type="hidden" id="stateChecker" name="stateChecker" value="${stateChecker}">
                <div class="form-group">
                    <div class="col-lg-12">
                        <input type="password" name="password" class="form-control" id="COS-PasswordDialog-currentPasswordField" placeholder="Password" />
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-lg-12">
                        <input type="password" name="password-new" class="form-control"  id="COS-PasswordDialog-newPasswordField" placeholder="New Password" />
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-lg-12">
                        <input type="password" name="password-confirm" class="form-control"  id="COS-PasswordDialog-confirm-newPasswordField" placeholder="Confirm New Password" />
                    </div>
                </div>
                <div class="form-group">
                  <div class="col-lg-12">
                    <button type="submit" class="btn btn-material-green-500" id="COS-PasswordDialog-change-passwordBtn">Change Password</button>
                    <a  class="btn btn-material-white-500" href="${properties.dashboardLink}" id="COS-PasswordDialog-cancelBtn">Cancel</a>
                  </div>
                </div>
                <div class="form-group">
                    <div class="col-lg-12">
                      <label id="COS-ChangePassword-policy">Please ensure that your password is at least 8 characters long with at least 1 uppercase letter, 1 lowercase letter, 1 special character and 1 digit.  Password cannot match any of your 3 most recent passwords.</label>
                    </div>
                </div>
              <form>
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
</div>


</@layout.mainLayout>
