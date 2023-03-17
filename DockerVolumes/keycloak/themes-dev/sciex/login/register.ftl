<#import "template.ftl" as layout>
<@layout.registrationLayout displayInfo=social.displayInfo; section>


<div class="container" style="padding-top: 175px">
    <div class="row" style="margin-top: 25px">
      <div class="col-lg-8 col-lg-offset-2 col-sm-8 col-sm-offset-2">
        <div class="well bs-component" style="background-color: #004964;margin-bottom: 0px; color: white;">
          <h4 style="color: white">Registration</h4>
        </div>
        <div class="well bs-component">
          <div class="row">
            <div class="col-md-12">

            <form action="${url.registrationAction}" method="post">
              <div class="form-group">
                <div class="form-group">
                  <input required class="form-control string required" placeholder="First name" name="firstName" id="firstName" autofocus="true"/>
                </div>

                <div class="form-group">
                  <input required class="form-control string required" placeholder="Last name" name="lastName" id="lastName" autofocus="true"/>
                </div>

                <div class="form-group email required user_email">
                  <input required class="form-control string email required" placeholder="Email" type="email" name="email" id="email">
                </div>

                <div class="form-group">
                  <input required class="form-control string required" placeholder="User name" name="username" id="username" />
                </div>

                <div class="form-group">
                  <input required type="password" class="form-control string required" placeholder="Password" name="password" id="password" />
                </div>

                <div class="form-group">
                  <input required type="password" class="form-control string required" placeholder="Password confirmation" name="password-confirm" id="password-confirm" />
                </div>

                <label>
                  <input required label="false" class="boolean optional" type="checkbox" value="false" name="accepted-terms" id="accepted-terms">
                </label>

                <label>I agree to the <a href="" target="_blank" style="color: #019688;">end user license agreement</a>.</label>

               </div>

               <div class="form-group" style="margin-top: -15px;">
                  <input type="submit" name="commit" value="Register" class="btn btn-material-green-500" data-disable-with="Registering...">
                  <a class="btn btn-material-white-500" href="${url.loginUrl}">Cancel</a>
                </div>

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
</div>


</@layout.registrationLayout>