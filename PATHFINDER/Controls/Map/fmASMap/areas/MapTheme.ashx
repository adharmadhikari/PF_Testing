<%@ WebHandler Language="C#" Class="MapTheme" %>

using System;
using System.Web;

public class MapTheme : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {        
        context.Response.Clear();
        context.Response.Cache.SetNoStore();

        //If d query string param set then just load Default xml file - don't care about client state param s
        string s = context.Request.QueryString["s"];
        string areasXml = !string.IsNullOrEmpty(s) && string.IsNullOrEmpty(context.Request.QueryString["d"]) ? string.Format("areas/mapdata.ashx?s={0}", s) : "areas/areas.xml";
        
        context.Response.Write("<theme id='us'>");
        context.Response.Write("<map file='areas/fm-us.swf' borderColor='0xCCCCCC'></map>");
        context.Response.Write(string.Format("<areas xmlAreas='{0}' xmlCategories='areas/area_categories.xml'></areas>", areasXml));
        context.Response.Write("<pois xmlPOIs='pois/pois.xml' xmlCategories='pois/poi_categories.xml' />");
        context.Response.Write("</theme>");
    }

    public bool IsReusable
    {
        get
        {
            return true;
        }
    }

}