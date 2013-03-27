/// <reference name="MicrosoftAjax.js"/>
/// <reference path="~/content/scripts/jquery-1.3.2-vsdoc.js"/>
/// <reference path="~/content/scripts/clientmanager-vsdoc.js"/>

		var windowHeight = $(window);
		var divHeight = windowHeight.height(); 
		var divWidth = windowHeight.width();


		$(document).ready(pageReady);
		Sys.WebForms.PageRequestManager.getInstance().add_endRequest(pageReady);

		function pageReady() 
		{
		    //Prevent spaces in username
		    $("#ctl00_main_loginCtrl_Login1_UserName").keydown(function(e)
		    {
		        if (e.keyCode == 32)
		            return false;
		    }).change(function(e)
		    {
		        $(this).val(function(i, v) { return v.replace(/ /g, ""); });
		    }); 
            //
		    		    
		    $("body").addClass("login");
		    $(".header").hide();
		    if($("form").attr("action") != "reauthenticate.aspx")
		        $("body.login").css({ paddingTop: divHeight / 4 });
		    $("html").css({ margin: "0px" });
		    $(".textBoxReadOnly").parent().parent().parent().addClass("readOnlyBox");

		    var userBox = $("#ctl00_main_Login1_UserName").attr("value");

		    if (userBox)
		    {
		        $("#ctl00_main_Login1_Password").focus();
		    } else
		    {
		        $("#ctl00_main_Login1_UserName").focus();
		    }
		    if (userBox)
		    {
		        $("#ctl00_main_Login1_LabelUserName").hide();
		    } else
		    {
		        $("#ctl00_main_Login1_LabelUserName").show();

		    }
		}

 function resetUserName()
 {
     $($(".textBoxReadOnly")).attr("readOnly", false).attr("class", "textBox userName").focus().select();
        $(".textBox").parent().parent().parent().removeClass("readOnlyBox");
        $("#ctl00_main_Login1_UserName").attr("value", "");
        $("#ctl00_main_Login1_UserName").focus();
        $("#ctl00_main_Login1_LabelUserName").show();

 }
 

 function forgotPassword()
 {
     $openWindow("content/forgotpassword.aspx", null, null, 500, 200);
 }

 //Check if async request already in progress - if so cancel this request 
 Sys.WebForms.PageRequestManager.getInstance().add_initializeRequest(initializeRequest);
 function initializeRequest(o, a)
 {
     $("#spanProgress").css("visibility","visible");
 }

 //Clean up droplists in grid after partial page render otherwise page takes an extremely long time to load    
 Sys.Net.WebRequestManager.add_completedRequest(completeRequest);
 function completeRequest(o, a)
 {
     $("#spanProgress").css("visibility","hidden");
 }    