using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

/// <summary>
/// Summary description for JSONHeaderBuilder
/// </summary>
public class JSONHeaderBuilder
{
    StringBuilder _stringBuilder;
    string _format;

    public JSONHeaderBuilder(string varName)
    {
        _format = string.Format("var {0}=[{1}0{2}];", varName, "{", "}");
        _stringBuilder = new StringBuilder();
    }

    public JSONHeaderBuilder AddColumn(string Text)
    {
        return AddColumn(Text, 1);
    }

    public JSONHeaderBuilder AddColumn(string Text, int ColumnSpan)
    {
        return AddColumn(Text, ColumnSpan,""); 
    }

    public JSONHeaderBuilder AddColumn(string Text, int ColumnSpan, String cssClass)
    {
        if ( _stringBuilder.Length > 0 )
            _stringBuilder.Append(",");
        _stringBuilder.AppendFormat("{0}span:{1}, text:\"{2}\", cssClass:\"{3}\"{4}", "{", ColumnSpan, Text, cssClass, "}");

        return this;
    }

    public override string ToString()
    {
        return string.Format(_format, _stringBuilder.ToString());
    }
}
