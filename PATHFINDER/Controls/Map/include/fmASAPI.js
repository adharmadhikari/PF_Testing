// **********************************
// **  Flashmaps AreaSelector 3.4  **
// **     JavaScript Functions     **
// **********************************
// ** (c)2009 Flashmaps Geospatial **
// **   http://www.flashmaps.com   **
// **********************************

// *********************
// ** THEME functions **
// *********************

	fmASMcPath 	= "";

function fmThemeLoad(theme_xml, area_str) {
//do: load a new theme into the AS

	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideTheme", theme_xml);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideArea", area_str);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "themeLoad");
	document.getElementById("fmASEngine").TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");
}

function fmThemeReloadAreas(areas_xml) {
//do: reload the areas of a theme

	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAreasXML", areas_xml);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "themeReloadAreas");
	document.getElementById("fmASEngine").TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");

}

function fmThemeReloadPOIs(pois_xml, bremove) {
//do: reload the pois of a theme
	
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideRemove", bremove);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsidePOIsXML", pois_xml);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "themeReloadPOIs");
	document.getElementById("fmASEngine").TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");
}

function fmThemeReloadPolylines(lines_xml) {
//do: reload the polylines of a theme

	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsidePolylinesXML", lines_xml);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "themeReloadPolylines");
	document.getElementById("fmASEngine").TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");
}

function fmThemeAddPOIs(pois_xml) {
//do: reload the pois of a theme

	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsidePOIsXML", pois_xml);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "themeAddPOIs");
	document.getElementById("fmASEngine").TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");
}

function fmThemeRemovePOIs(category_str) {
//do: reload the pois of a theme

	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideCategory", category_str);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "themeRemovePOIs");
	document.getElementById("fmASEngine").TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");
}

// *******************
// ** MAP functions **
// *******************

function fmInitialView() {
//do: return the map to initial view

	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "themeInitialView");
	document.getElementById("fmASEngine").TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");
}

function fmMapBackLevel() {
//do: return the map one level back
	
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "themeBackLevel");
	document.getElementById("fmASEngine").TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");
}

// ************************
// ** MAP MODE functions **
// ************************

function fmMapModeZoom() {
//do: change the map mode to zoom mode

	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "modeZoom");
	document.getElementById("fmASEngine").TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");
}

function fmMapModeSelect() {
//do: change the map mode to select mode

	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "modeSelect");
	document.getElementById("fmASEngine").TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");
}

function fmMapModeAddArea(level, id, id_parent) {
//do: add an area to select mode

	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideLevel", level);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideId", id);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideIdParent", id_parent);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "modeAddArea");
	document.getElementById("fmASEngine").TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");
}

function fmMapModeRemoveArea(level, id, id_parent) {
//do: remove an area to select mode

	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideLevel", level);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideId", id);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideIdParent", id_parent);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "modeRemoveArea");
	document.getElementById("fmASEngine").TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");
}

function fmMapModeExportListAreas() {
//do: return the list of areas selected

	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "modeExport");
	document.getElementById("fmASEngine").TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");
	return document.getElementById("fmASEngine").GetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAreasSelected");
}

function fmMapModeCleanAreas() {
//do: clean the list of areas selected

	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "modeClean");
	document.getElementById("fmASEngine").TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");
}

// *********************
// ** AREAS functions **
// *********************

function fmAreaCenter(area_str) {
//do: center the map into an area

	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideArea", area_str);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "areaCenter");
	document.getElementById("fmASEngine").TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");
}

function fmAreaBackAndCenter(area_str) {
//do: back a level and center the map into an area

	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideArea", area_str);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "areaBackAndCenter");
	document.getElementById("fmASEngine").TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");
}

function fmAreaCenterLatLon(area_str, lat, lon, scale, load_pois) {
//do: center the map into a latitude/longitude

	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideArea", area_str);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideLat", lat);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideLon", lon);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideScale", scale);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideLoadPOIs", load_pois);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "areaCenterLatLon");
	document.getElementById("fmASEngine").TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");
}

function fmAreaZoomIn(areas_array) {
//do: zoom in into the map (areas_str array)
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAreas", areas_array);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "areaZoomIn");
	document.getElementById("fmASEngine").TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");
}

function fmAreaEnabled(area_str, enabled_str) {
//do: enabled / disabled an area

	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideArea", area_str);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideEnabled", enabled_str);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "areaEnabled");
	document.getElementById("fmASEngine").TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");
}

function fmAreaColor(area_str, colorNormal, colorOver, colorPress, colorText) {
//do: change the color of an area

	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideArea", area_str);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideColorNormal", colorNormal);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideColorOver", colorOver);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideColorPress", colorPress);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideColorText", colorText);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "areaColor");
	document.getElementById("fmASEngine").TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");
}

//********************
//** POIS FUNCTIONS **
//********************

function fmPOIsShowCategory(category_str) {
//do: show all pois of a category (* for all categories)
	
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideCategory", category_str);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "POIsShowCategory");
	document.getElementById("fmASEngine").TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");
}

function fmPOIsHideCategory(category_str) {
//do: hide all pois of a category (* for all categories)
	
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideCategory", category_str);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "POIsHideCategory");
	document.getElementById("fmASEngine").TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");
}

function fmPOIAddEvent(event_str, target_str, url_str, id_str) {
//do: add an event to a POI

	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideEvent", event_str);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideTarget", target_str);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideUrl", url_str);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideId", id_str);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "POIEvent");
	document.getElementById("fmASEngine").TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");
}

function fmPOIRollOver(id_str, icon_str) {
//do: rollover over a POI
	
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideId", id_str);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideIcon", icon_str);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "POIRollOver");
	document.getElementById("fmASEngine").TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");
}

function fmPOIRollOut(id_str) {
//do: rollout over a POI
	
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideId", id_str);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "POIRollOut");
	document.getElementById("fmASEngine").TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");
}

function fmPOIHighlight(id_str, icon_str) {
//do: rollover over a POI
	
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideId", id_str);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideIcon", icon_str);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "POIHighlight");
	document.getElementById("fmASEngine").TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");
}

function fmPOIUnhighlight() {
//do: rollout over a POI
	
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "POIUnhighlight");
	document.getElementById("fmASEngine").TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");
}


//************************
//** POLYLINE FUNCTIONS **
//************************

function fmPolylinesShowCategory(category_str) {
//do: show all polylines of a category (* for all categories)
	
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideCategory", category_str);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "PolylineShowCategory");
	document.getElementById("fmASEngine").TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");
}

function fmPolylinesHideCategory(category_str) {
//do: hide all polylines of a category (* for all categories)
	
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideCategory", category_str);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "PolylineHideCategory");
	document.getElementById("fmASEngine").TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");
}


//*********************
//** IMAGE FUNCTIONS **
//*********************

function fmImageLoad(imageXml_str) {
//do: load an image
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideUrl", imageXml_str);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "ImageLoad");
	document.getElementById("fmASEngine").TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");
}

function fmImagesShow() {
//do: show images
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "ImagesShow");
	document.getElementById("fmASEngine").TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");
}

function fmImagesHide() {
//do: hide images
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "ImagesHide");
	document.getElementById("fmASEngine").TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");
}

function fmImagesRemove() {
//do: remove images
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "ImagesRemove");
	document.getElementById("fmASEngine").TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");
}


//****************************
//** ALERT WINDOW FUNCTIONS **
//****************************

function fmShowAlert(title_str, text_str) {
//do: show an alert window
	
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideTitle", title_str);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideText", text_str);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "showAlert");
	document.getElementById("fmASEngine").TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");
}

function fmHideAlert() {
//do: hide an alert window
	
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "hideAlert");
	document.getElementById("fmASEngine").TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");
}

//****************************
//** ALERT WINDOW FUNCTIONS **
//****************************

function fmShowCrossHair(lat, lon, scale) {
//do: show an alert window
	
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideLat", lat);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideLon", lon);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideScale", scale);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "showCrossHair");
	document.getElementById("fmASEngine").TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");
}

function fmHideCrossHair() {
//do: hide the cross hair
	
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "hideCrossHair");
	document.getElementById("fmASEngine").TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");
}

//************************
//*** OBJECT FUNCTIONS ***
//************************

function fmObjectShow(id_str) {
//do: show an object
	
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideObject", id_str);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "objectShow");
	document.getElementById("fmASEngine").TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");
}

function fmObjectHide(id_str) {
//do: hide an object
	
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideObject", id_str);
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "objectHide");
	document.getElementById("fmASEngine").TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");
}

//***********************
//** POITEXT FUNCTIONS **
//***********************

function fmShowPOIText() {
//do: show poitext over the map
	
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "showPOIText");
	document.getElementById("fmASEngine").TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");
}

function fmHidePOIText() {
//do: hide poitext over the map
	
	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "hidePOIText");
	document.getElementById("fmASEngine").TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");
}

//*********************
//** PRINT FUNCTIONS **
//*********************

function fmPrint() {
//do: print the current view

	document.getElementById("fmASEngine").SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "print");
	document.getElementById("fmASEngine").TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");
}

//**********************
//** RESIZE FUNCTIONS **
//**********************

function fmRecalcWidthHeightFactor() {
//do: 	Recalc width and height factors of the actual root movieclip relative to its original size
	fmEngine.SetVariable("_root." + fmASMcPath + "ASEngine_mc.outsideAction", "recalcWidthHeightFactor");
	fmEngine.TCallLabel("_root." + fmASMcPath + "ASEngine_mc.outside_mc", "doAction");
}
