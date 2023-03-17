<#import "template.ftl" as layout>
<@layout.registrationLayout displayInfo=social.displayInfo; section>



<div class="" style="display: flex; height: 65.7vh; justify-content: center; align-items: center;">
      <div style="width: 90%; min-width: 400px; max-width: 750px;" id="COS-ForgotPasswordPanel">
        <div class="well bs-component" style="background-color: #004964;margin-bottom: 0px; color: white;">
          <h4 style="color: white">Forgot Password</h4>
        </div>
        <div class="well bs-component">
          <div class="row">
            <div class="col-md-12">
              <form class="form-horizontal" id="new_user" action="${url.loginAction}" method="post" accept-charset="UTF-8">
                  <fieldset>
                <div class="form-group">
                    <div class="col-lg-12">
                        <input type="text" name="username" id="username" value="" class="form-control" placeholder="Email" required="required">
                    </div>
                </div>
                <div class="form-group">
                  <div class="col-lg-12">
                  <input type="submit" name="commit" value="Submit" class="btn btn-material-green-500" data-disable-with="Submit">
                  <a class="btn btn-material-white-500" href="${url.loginUrl}">Cancel</a>
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