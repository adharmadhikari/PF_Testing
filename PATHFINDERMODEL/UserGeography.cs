using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

/// <summary>
/// Data container for information about geographic regions.
/// </summary>
[DataContract]
public class UserGeography
{
	public UserGeography()
	{
	}

    public static UserGeography Create(double MinX, double MinY, double MaxX, double MaxY)
    {
        UserGeography userGeog = new UserGeography();

        double lat = (MaxY + MinY) / 2;
        double lon = (MaxX + MinX) / 2;
        double height = MaxY - MinY;
        double width = MaxX - MinX;

        //make dimensions of region a rectangle in the same scale as US
        if ( height > width )
            width = height * 2.324;
        else if ( width * .43 > height )
            height = width * .43;
        else
            width = height * 2.324;

        return new UserGeography { Area = height * width, CenterX = lon, CenterY = lat, RegionID = null };
    }

    /// <summary>
    /// Longitude of the center point of a region.
    /// </summary>
    [DataMember]
    public double CenterX { get; set; }
    /// <summary>
    /// Latitude of the center point of a region.
    /// </summary>
    [DataMember]
    public double CenterY { get; set; }
    /// <summary>
    /// Area the geographic region covers.  It is calculated to be the size required to show the entire region and is scaled to fit in the dashboard map.  This means if the region is taller than it is wide the width is recalculated so it is scaled up to match the height.  There are is then calculated based on the larger width.  A similar adjustment is applied if the region is too wide compaired to its height.  In that case the height is adjusted to match the width.
    /// </summary>
    [DataMember]
    public double Area { get; set; }
    /// <summary>
    /// Optional ID of a region.  This value is only set if the UserGeography instance represents a user's aligned region and they are only aligned to one state.
    /// </summary>
    [DataMember]
    public string RegionID { get; set; }
    string _regions = null;
    /// <summary>
    /// Optional comma separated list of regions that create a larger region.  This is usually a list of states and will be set when the UserGeography instance represents an entry in the Region selection list in the dashbaord map.
    /// </summary>
    [DataMember]
    public string Regions
    {
        get { return _regions; }
        set 
        { 
            _regions = value;
            RegionsAsList();
        }
    }
    
    Dictionary<string, string> _regionList = new Dictionary<string,string>();
    /// <summary>
    /// Constructs a dictionary of regions based on the Regions property.
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, string> RegionsAsList()
    {
        if ( !string.IsNullOrEmpty(Regions) )
            _regionList = Regions.Split(',').ToDictionary<string, string>(r => r.ToLower());
        else
            _regionList = new Dictionary<string, string>();

        return _regionList;
    }

    /// <summary>
    /// Determines if a specified GeographyID (ex State Abbr) is contained in the list of regions stored in the Regions property.  If the Regions property is null or empty True is always returned meaning the UserGeography instance must represent the nation.
    /// </summary>
    /// <param name="GeographyID"></param>
    /// <returns></returns>
    public bool HasRegion(string GeographyID)
    {
        if ( _regionList == null || _regionList.Count == 0 )
            return true;
        else 
            return _regionList.ContainsKey(GeographyID.ToLower());
    }
}
