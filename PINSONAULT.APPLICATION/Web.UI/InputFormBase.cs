using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;

public class InputFormResult
{
    public bool Success { get; set; }
    //public string CommandName { get; set; }

    List<string> _messages = new List<string>();
    public List<string> Messages
    {
        get { return _messages; }
    }

}

/// <summary>
/// Summary description for InputFormBase
/// </summary>
public abstract class InputFormBase : PageBase
{
	public InputFormBase()
	{
        PostBackResult = new InputFormResult();
	}

    protected override void OnPreInit(EventArgs e)
    {
        this.Response.Cache.SetNoStore();    
        base.OnPreInit(e);
    }

    protected override void OnInit(EventArgs e)
    {
        this.EnableViewState = true;
        base.OnInit(e);
    }

    protected InputFormResult PostBackResult { get; private set; }

    protected string InvalidPageUrl
    {
        get { return "invalidpage.aspx"; }
    }

    protected override void OnLoad(EventArgs e)
    {
        if ( IsRequestValid() )
        {
            //string target = Request.Form["__EVENTTARGET"];
            //if ( !string.IsNullOrEmpty(target) )
            //{
            //    IButtonControl control = FindControl(target) as IButtonControl;
            //    if ( control != null )
            //        PostBackResult.CommandName = control.CommandName;

            //}
            Page.ClientScript.RegisterHiddenField("__RESPONSETYPE", Request.Form["__RESPONSETYPE"]);
            base.OnLoad(e);
        }
        else
        {
            Server.Transfer(InvalidPageUrl, false);
        }
    }

    protected abstract bool IsRequestValid();

    public string ResponseType
    {
        get
        {
            return Request.Form["__RESPONSETYPE"];
        }
    }

    protected override void Render(System.Web.UI.HtmlTextWriter writer)
    {
        if ( !Page.IsPostBack || string.Compare(ResponseType, "html", true) == 0 )
            base.Render(writer);
        else
        {
            Response.Clear();
            Response.ContentType = "application/json";

            writer.Write(serializeResult(PostBackResult));
        }
    }

    //protected sealed override void OnError(EventArgs e)
    //{
    //    if ( HttpContext.Current.IsDebuggingEnabled && Page.IsPostBack )//FOR DEBUGGING ONLY
    //    {
    //        PostBackResult.Success = false;
    //        PostBackResult.Messages.Clear();
    //        if ( HttpContext.Current.Error != null )
    //        {
    //            PostBackResult.Messages.Add(HttpContext.Current.Error.Message);
    //            HttpContext.Current.ClearError();
    //        }
    //    }

    //    base.OnError(e);
    //}

    string serializeResult(InputFormResult data)
    {
        string result = null;

        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(InputFormResult));

        using ( MemoryStream ms = new MemoryStream() )
        {
            serializer.WriteObject(ms, data);
            result = UTF8Encoding.UTF8.GetString(ms.ToArray());
        }

        return result;
    }
}

