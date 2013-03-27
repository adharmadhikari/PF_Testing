using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

public abstract class DynamicTemplateBase : System.Web.UI.UserControl 
{
    public string ContextKey
    {
        get { return string.Format("{0}_DynamicTemplate", this.Parent.Parent.UniqueID); }
    }

    public string DataField1 { get; set; }
    public string DataField2 { get; set; }
    public string DataField3 { get; set; }
    public string DataField4 { get; set; }
    public string DataField5 { get; set; }

    public abstract string DataFieldFormat1 { get; }
    public abstract string DataFieldFormat2 { get; }
    public abstract string DataFieldFormat3 { get; }
    public abstract string DataFieldFormat4 { get; }
    string _dataFieldFormat5 = "";
    public virtual string DataFieldFormat5  
    { 
        get{ return _dataFieldFormat5; }
    }

    protected override void OnInit(EventArgs e)
    {
        string key = ContextKey;

        object index = Context.Items[key];
        int idx = 0;

        if ( index != null )
        {
            idx = Convert.ToInt32(index) + 1;
        }

        Context.Items[key] = idx;

        DataField1 = string.Format(DataFieldFormat1, idx);
        DataField2 = string.Format(DataFieldFormat2, idx);
        DataField3 = string.Format(DataFieldFormat3, idx);
        DataField4 = string.Format(DataFieldFormat4, idx);
        DataField5 = string.Format(DataFieldFormat5, idx);
        base.OnInit(e);
    }
}
