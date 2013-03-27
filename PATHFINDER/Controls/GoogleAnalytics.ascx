<%@ Control Language="C#" ClassName="GoogleAnalytics" %>
<%@ OutputCache Duration="100" Shared="true" VaryByParam="None" %>


<script runat="server" >
    protected override void OnLoad(EventArgs e)
    {
        this.Visible = !Context.IsDebuggingEnabled;
        
        base.OnLoad(e);
    }
</script>


<script type="text/javascript">
    var gaJsHost = (("https:" == document.location.protocol) ? "https://ssl." : "http://www."); document.write(unescape("%3Cscript src='" + gaJsHost + "google-analytics.com/ga.js' type='text/javascript'%3E%3C/script%3E"));
</script>
<script type="text/javascript">
    try
    {
        var pageTracker = _gat._getTracker('<%= System.Configuration.ConfigurationManager.AppSettings["googleAnalyticsTrackingCode"] %>');
        pageTracker._trackPageview();
    } catch (err) { }
    
</script>
