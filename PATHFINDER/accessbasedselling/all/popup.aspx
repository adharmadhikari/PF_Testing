<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Popup.master" AutoEventWireup="true" CodeFile="popup.aspx.cs" Inherits="accessbasedselling_all_popup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="scriptContainer" Runat="Server">
<style>

.abspannel
{
	display:none;
}

.abspannel a
{
	color:#fff;
}

.absTab
{
	background-position:0 0;
	padding-left:7px;
	margin-bottom: -40px;
}

.absTab a
{
	color:#fff;
	font-weight:bold;
}
#infoPopup .title
{
    padding-bottom:0px;
    padding-top:3px;
    }
.absSub
{
}
.absSub a
{
	color:#fff;
	font-weight:bold;
}
.absTab, .absTab * {
	cursor:pointer;
	display:inline-block;
	height:18px;
}
.absTab .bg {
	background-position: top right;
	background-repeat:no-repeat;
	padding-right:8px;

}
.absTab .bg .bg2 span{
	height:17px;

}
.absTab .bg2 {
	background-position:0 -52px;
	background-repeat:repeat-x;
}
.absTab input
{
	border:none;
	background:none;
}

.absSub a:hover
{
	color: #C0C0C0;
	font-weight: bold;
}

.selectedSub
{

}

.selectedSub a
{
	color: #C0C0C0;
	font-weight: bold;
}

.helpLink
{
    
    }
.helpLink a
{
	color:#009900;	
}
.helpLink a:hover
{
	text-decoration:underline !important;
}

.Popup_msg
{
    float:right;
    border: solid 5px #2d58a7;
    margin: 10px 5px 5px 5px;
    }
#mp3Player
{
    float:left;
    margin-top:85px;
    }
</style>
    <script type="text/javascript">
    
        /**
        * SWFObject v1.5: Flash Player detection and embed - http://blog.deconcept.com/swfobject/
        *
        * SWFObject is (c) 2007 Geoff Stearns and is released under the MIT License:
        * http://www.opensource.org/licenses/mit-license.php
        *
        */
        if (typeof deconcept == "undefined") { var deconcept = new Object(); } if (typeof deconcept.util == "undefined") { deconcept.util = new Object(); } if (typeof deconcept.SWFObjectUtil == "undefined") { deconcept.SWFObjectUtil = new Object(); } deconcept.SWFObject = function(_1, id, w, h, _5, c, _7, _8, _9, _a) { if (!document.getElementById) { return; } this.DETECT_KEY = _a ? _a : "detectflash"; this.skipDetect = deconcept.util.getRequestParameter(this.DETECT_KEY); this.params = new Object(); this.variables = new Object(); this.attributes = new Array(); if (_1) { this.setAttribute("swf", _1); } if (id) { this.setAttribute("id", id); } if (w) { this.setAttribute("width", w); } if (h) { this.setAttribute("height", h); } if (_5) { this.setAttribute("version", new deconcept.PlayerVersion(_5.toString().split("."))); } this.installedVer = deconcept.SWFObjectUtil.getPlayerVersion(); if (!window.opera && document.all && this.installedVer.major > 7) { deconcept.SWFObject.doPrepUnload = true; } if (c) { this.addParam("bgcolor", c); } var q = _7 ? _7 : "high"; this.addParam("quality", q); this.setAttribute("useExpressInstall", false); this.setAttribute("doExpressInstall", false); var _c = (_8) ? _8 : window.location; this.setAttribute("xiRedirectUrl", _c); this.setAttribute("redirectUrl", ""); if (_9) { this.setAttribute("redirectUrl", _9); } }; deconcept.SWFObject.prototype = { useExpressInstall: function(_d) { this.xiSWFPath = !_d ? "expressinstall.swf" : _d; this.setAttribute("useExpressInstall", true); }, setAttribute: function(_e, _f) { this.attributes[_e] = _f; }, getAttribute: function(_10) { return this.attributes[_10]; }, addParam: function(_11, _12) { this.params[_11] = _12; }, getParams: function() { return this.params; }, addVariable: function(_13, _14) { this.variables[_13] = _14; }, getVariable: function(_15) { return this.variables[_15]; }, getVariables: function() { return this.variables; }, getVariablePairs: function() { var _16 = new Array(); var key; var _18 = this.getVariables(); for (key in _18) { _16[_16.length] = key + "=" + _18[key]; } return _16; }, getSWFHTML: function() { var _19 = ""; if (navigator.plugins && navigator.mimeTypes && navigator.mimeTypes.length) { if (this.getAttribute("doExpressInstall")) { this.addVariable("MMplayerType", "PlugIn"); this.setAttribute("swf", this.xiSWFPath); } _19 = "<embed type=\"application/x-shockwave-flash\" src=\"" + this.getAttribute("swf") + "\" width=\"" + this.getAttribute("width") + "\" height=\"" + this.getAttribute("height") + "\" style=\"" + this.getAttribute("style") + "\""; _19 += " id=\"" + this.getAttribute("id") + "\" name=\"" + this.getAttribute("id") + "\" "; var _1a = this.getParams(); for (var key in _1a) { _19 += [key] + "=\"" + _1a[key] + "\" "; } var _1c = this.getVariablePairs().join("&"); if (_1c.length > 0) { _19 += "flashvars=\"" + _1c + "\""; } _19 += "/>"; } else { if (this.getAttribute("doExpressInstall")) { this.addVariable("MMplayerType", "ActiveX"); this.setAttribute("swf", this.xiSWFPath); } _19 = "<object id=\"" + this.getAttribute("id") + "\" classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\" width=\"" + this.getAttribute("width") + "\" height=\"" + this.getAttribute("height") + "\" style=\"" + this.getAttribute("style") + "\">"; _19 += "<param name=\"movie\" value=\"" + this.getAttribute("swf") + "\" />"; var _1d = this.getParams(); for (var key in _1d) { _19 += "<param name=\"" + key + "\" value=\"" + _1d[key] + "\" />"; } var _1f = this.getVariablePairs().join("&"); if (_1f.length > 0) { _19 += "<param name=\"flashvars\" value=\"" + _1f + "\" />"; } _19 += "</object>"; } return _19; }, write: function(_20) { if (this.getAttribute("useExpressInstall")) { var _21 = new deconcept.PlayerVersion([6, 0, 65]); if (this.installedVer.versionIsValid(_21) && !this.installedVer.versionIsValid(this.getAttribute("version"))) { this.setAttribute("doExpressInstall", true); this.addVariable("MMredirectURL", escape(this.getAttribute("xiRedirectUrl"))); document.title = document.title.slice(0, 47) + " - Flash Player Installation"; this.addVariable("MMdoctitle", document.title); } } if (this.skipDetect || this.getAttribute("doExpressInstall") || this.installedVer.versionIsValid(this.getAttribute("version"))) { var n = (typeof _20 == "string") ? document.getElementById(_20) : _20; n.innerHTML = this.getSWFHTML(); return true; } else { if (this.getAttribute("redirectUrl") != "") { document.location.replace(this.getAttribute("redirectUrl")); } } return false; } }; deconcept.SWFObjectUtil.getPlayerVersion = function() { var _23 = new deconcept.PlayerVersion([0, 0, 0]); if (navigator.plugins && navigator.mimeTypes.length) { var x = navigator.plugins["Shockwave Flash"]; if (x && x.description) { _23 = new deconcept.PlayerVersion(x.description.replace(/([a-zA-Z]|\s)+/, "").replace(/(\s+r|\s+b[0-9]+)/, ".").split(".")); } } else { if (navigator.userAgent && navigator.userAgent.indexOf("Windows CE") >= 0) { var axo = 1; var _26 = 3; while (axo) { try { _26++; axo = new ActiveXObject("ShockwaveFlash.ShockwaveFlash." + _26); _23 = new deconcept.PlayerVersion([_26, 0, 0]); } catch (e) { axo = null; } } } else { try { var axo = new ActiveXObject("ShockwaveFlash.ShockwaveFlash.7"); } catch (e) { try { var axo = new ActiveXObject("ShockwaveFlash.ShockwaveFlash.6"); _23 = new deconcept.PlayerVersion([6, 0, 21]); axo.AllowScriptAccess = "always"; } catch (e) { if (_23.major == 6) { return _23; } } try { axo = new ActiveXObject("ShockwaveFlash.ShockwaveFlash"); } catch (e) { } } if (axo != null) { _23 = new deconcept.PlayerVersion(axo.GetVariable("$version").split(" ")[1].split(",")); } } } return _23; }; deconcept.PlayerVersion = function(_29) { this.major = _29[0] != null ? parseInt(_29[0]) : 0; this.minor = _29[1] != null ? parseInt(_29[1]) : 0; this.rev = _29[2] != null ? parseInt(_29[2]) : 0; }; deconcept.PlayerVersion.prototype.versionIsValid = function(fv) { if (this.major < fv.major) { return false; } if (this.major > fv.major) { return true; } if (this.minor < fv.minor) { return false; } if (this.minor > fv.minor) { return true; } if (this.rev < fv.rev) { return false; } return true; }; deconcept.util = { getRequestParameter: function(_2b) { var q = document.location.search || document.location.hash; if (_2b == null) { return q; } if (q) { var _2d = q.substring(1).split("&"); for (var i = 0; i < _2d.length; i++) { if (_2d[i].substring(0, _2d[i].indexOf("=")) == _2b) { return _2d[i].substring((_2d[i].indexOf("=") + 1)); } } } return ""; } }; deconcept.SWFObjectUtil.cleanupSWFs = function() { var _2f = document.getElementsByTagName("OBJECT"); for (var i = _2f.length - 1; i >= 0; i--) { _2f[i].style.display = "none"; for (var x in _2f[i]) { if (typeof _2f[i][x] == "function") { _2f[i][x] = function() { }; } } } }; if (deconcept.SWFObject.doPrepUnload) { if (!deconcept.unloadSet) { deconcept.SWFObjectUtil.prepUnload = function() { __flash_unloadHandler = function() { }; __flash_savedUnloadHandler = function() { }; window.attachEvent("onunload", deconcept.SWFObjectUtil.cleanupSWFs); }; window.attachEvent("onbeforeunload", deconcept.SWFObjectUtil.prepUnload); deconcept.unloadSet = true; } } if (!document.getElementById && document.all) { document.getElementById = function(id) { return document.all[id]; }; } var getQueryParamValue = deconcept.util.getRequestParameter; var FlashObject = deconcept.SWFObject; var SWFObject = deconcept.SWFObject;


        /* 
        * flowplayer.js 3.1.2. The Flowplayer API
        * 
        * Copyright 2009 Flowplayer Oy
        * 
        * This file is part of Flowplayer.
        * 
        * Flowplayer is free software: you can redistribute it and/or modify
        * it under the terms of the GNU General Public License as published by
        * the Free Software Foundation, either version 3 of the License, or
        * (at your option) any later version.
        * 
        * Flowplayer is distributed in the hope that it will be useful,
        * but WITHOUT ANY WARRANTY; without even the implied warranty of
        * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
        * GNU General Public License for more details.
        * 
        * You should have received a copy of the GNU General Public License
        * along with Flowplayer.  If not, see <http://www.gnu.org/licenses/>.
        * 
        * Date: 2009-02-25 16:24:29 -0500 (Wed, 25 Feb 2009)
        * Revision: 166 
        */
        (function() { function g(o) { console.log("$f.fireEvent", [].slice.call(o)) } function k(q) { if (!q || typeof q != "object") { return q } var o = new q.constructor(); for (var p in q) { if (q.hasOwnProperty(p)) { o[p] = k(q[p]) } } return o } function m(t, q) { if (!t) { return } var o, p = 0, r = t.length; if (r === undefined) { for (o in t) { if (q.call(t[o], o, t[o]) === false) { break } } } else { for (var s = t[0]; p < r && q.call(s, p, s) !== false; s = t[++p]) { } } return t } function c(o) { return document.getElementById(o) } function i(q, p, o) { if (typeof p != "object") { return q } if (q && p) { m(p, function(r, s) { if (!o || typeof s != "function") { q[r] = s } }) } return q } function n(s) { var q = s.indexOf("."); if (q != -1) { var p = s.substring(0, q) || "*"; var o = s.substring(q + 1, s.length); var r = []; m(document.getElementsByTagName(p), function() { if (this.className && this.className.indexOf(o) != -1) { r.push(this) } }); return r } } function f(o) { o = o || window.event; if (o.preventDefault) { o.stopPropagation(); o.preventDefault() } else { o.returnValue = false; o.cancelBubble = true } return false } function j(q, o, p) { q[o] = q[o] || []; q[o].push(p) } function e() { return "_" + ("" + Math.random()).substring(2, 10) } var h = function(t, r, s) { var q = this; var p = {}; var u = {}; q.index = r; if (typeof t == "string") { t = { url: t} } i(this, t, true); m(("Begin*,Start,Pause*,Resume*,Seek*,Stop*,Finish*,LastSecond,Update,BufferFull,BufferEmpty,BufferStop").split(","), function() { var v = "on" + this; if (v.indexOf("*") != -1) { v = v.substring(0, v.length - 1); var w = "onBefore" + v.substring(2); q[w] = function(x) { j(u, w, x); return q } } q[v] = function(x) { j(u, v, x); return q }; if (r == -1) { if (q[w]) { s[w] = q[w] } if (q[v]) { s[v] = q[v] } } }); i(this, { onCuepoint: function(x, w) { if (arguments.length == 1) { p.embedded = [null, x]; return q } if (typeof x == "number") { x = [x] } var v = e(); p[v] = [x, w]; if (s.isLoaded()) { s._api().fp_addCuepoints(x, r, v) } return q }, update: function(w) { i(q, w); if (s.isLoaded()) { s._api().fp_updateClip(w, r) } var v = s.getConfig(); var x = (r == -1) ? v.clip : v.playlist[r]; i(x, w, true) }, _fireEvent: function(v, y, w, A) { if (v == "onLoad") { m(p, function(B, C) { if (C[0]) { s._api().fp_addCuepoints(C[0], r, B) } }); return false } A = A || q; if (v == "onCuepoint") { var z = p[y]; if (z) { return z[1].call(s, A, w) } } if (y && "onBeforeBegin,onMetaData,onStart,onUpdate,onResume".indexOf(v) != -1) { i(A, y); if (y.metaData) { if (!A.duration) { A.duration = y.metaData.duration } else { A.fullDuration = y.metaData.duration } } } var x = true; m(u[v], function() { x = this.call(s, A, y, w) }); return x } }); if (t.onCuepoint) { var o = t.onCuepoint; q.onCuepoint.apply(q, typeof o == "function" ? [o] : o); delete t.onCuepoint } m(t, function(v, w) { if (typeof w == "function") { j(u, v, w); delete t[v] } }); if (r == -1) { s.onCuepoint = this.onCuepoint } }; var l = function(p, r, q, t) { var s = {}; var o = this; var u = false; if (t) { i(s, t) } m(r, function(v, w) { if (typeof w == "function") { s[v] = w; delete r[v] } }); i(this, { animate: function(y, z, x) { if (!y) { return o } if (typeof z == "function") { x = z; z = 500 } if (typeof y == "string") { var w = y; y = {}; y[w] = z; z = 500 } if (x) { var v = e(); s[v] = x } if (z === undefined) { z = 500 } r = q._api().fp_animate(p, y, z, v); return o }, css: function(w, x) { if (x !== undefined) { var v = {}; v[w] = x; w = v } r = q._api().fp_css(p, w); i(o, r); return o }, show: function() { this.display = "block"; q._api().fp_showPlugin(p); return o }, hide: function() { this.display = "none"; q._api().fp_hidePlugin(p); return o }, toggle: function() { this.display = q._api().fp_togglePlugin(p); return o }, fadeTo: function(y, x, w) { if (typeof x == "function") { w = x; x = 500 } if (w) { var v = e(); s[v] = w } this.display = q._api().fp_fadeTo(p, y, x, v); this.opacity = y; return o }, fadeIn: function(w, v) { return o.fadeTo(1, w, v) }, fadeOut: function(w, v) { return o.fadeTo(0, w, v) }, getName: function() { return p }, getPlayer: function() { return q }, _fireEvent: function(w, v, x) { if (w == "onUpdate") { var y = q._api().fp_getPlugin(p); if (!y) { return } i(o, y); delete o.methods; if (!u) { m(y.methods, function() { var A = "" + this; o[A] = function() { var B = [].slice.call(arguments); var C = q._api().fp_invoke(p, A, B); return C === "undefined" || C === undefined ? o : C } }); u = true } } var z = s[w]; if (z) { z.apply(o, v); if (w.substring(0, 1) == "_") { delete s[w] } } } }) }; function b(o, t, z) { var E = this, y = null, x, u, p = [], s = {}, B = {}, r, v, w, D, A, q; i(E, { id: function() { return r }, isLoaded: function() { return (y !== null) }, getParent: function() { return o }, hide: function(F) { if (F) { o.style.height = "0px" } if (y) { y.style.height = "0px" } return E }, show: function() { o.style.height = q + "px"; if (y) { y.style.height = A + "px" } return E }, isHidden: function() { return y && parseInt(y.style.height, 10) === 0 }, load: function(F) { if (!y && E._fireEvent("onBeforeLoad") !== false) { m(a, function() { this.unload() }); x = o.innerHTML; if (x && !flashembed.isSupported(t.version)) { o.innerHTML = "" } flashembed(o, t, { config: z }); if (F) { F.cached = true; j(B, "onLoad", F) } } return E }, unload: function() { try { if (!y || y.fp_isFullscreen()) { return E } } catch (F) { return E } if (x.replace(/\s/g, "") !== "") { if (E._fireEvent("onBeforeUnload") === false) { return E } y.fp_close(); y = null; o.innerHTML = x; E._fireEvent("onUnload") } return E }, getClip: function(F) { if (F === undefined) { F = D } return p[F] }, getCommonClip: function() { return u }, getPlaylist: function() { return p }, getPlugin: function(F) { var H = s[F]; if (!H && E.isLoaded()) { var G = E._api().fp_getPlugin(F); if (G) { H = new l(F, G, E); s[F] = H } } return H }, getScreen: function() { return E.getPlugin("screen") }, getControls: function() { return E.getPlugin("controls") }, getConfig: function(F) { return F ? k(z) : z }, getFlashParams: function() { return t }, loadPlugin: function(I, H, K, J) { if (typeof K == "function") { J = K; K = {} } var G = J ? e() : "_"; E._api().fp_loadPlugin(I, H, K, G); var F = {}; F[G] = J; var L = new l(I, null, E, F); s[I] = L; return L }, getState: function() { return y ? y.fp_getState() : -1 }, play: function(G, F) { function H() { if (G !== undefined) { E._api().fp_play(G, F) } else { E._api().fp_play() } } if (y) { H() } else { E.load(function() { H() }) } return E }, getVersion: function() { var G = "flowplayer.js 3.1.2"; if (y) { var F = y.fp_getVersion(); F.push(G); return F } return G }, _api: function() { if (!y) { throw "Flowplayer " + E.id() + " not loaded when calling an API method" } return y }, setClip: function(F) { E.setPlaylist([F]); return E }, getIndex: function() { return w } }); m(("Click*,Load*,Unload*,Keypress*,Volume*,Mute*,Unmute*,PlaylistReplace,ClipAdd,Fullscreen*,FullscreenExit,Error,MouseOver,MouseOut").split(","), function() { var F = "on" + this; if (F.indexOf("*") != -1) { F = F.substring(0, F.length - 1); var G = "onBefore" + F.substring(2); E[G] = function(H) { j(B, G, H); return E } } E[F] = function(H) { j(B, F, H); return E } }); m(("pause,resume,mute,unmute,stop,toggle,seek,getStatus,getVolume,setVolume,getTime,isPaused,isPlaying,startBuffering,stopBuffering,isFullscreen,toggleFullscreen,reset,close,setPlaylist,addClip").split(","), function() { var F = this; E[F] = function(H, G) { if (!y) { return E } var I = null; if (H !== undefined && G !== undefined) { I = y["fp_" + F](H, G) } else { I = (H === undefined) ? y["fp_" + F]() : y["fp_" + F](H) } return I === "undefined" || I === undefined ? E : I } }); E._fireEvent = function(O) { if (typeof O == "string") { O = [O] } var P = O[0], M = O[1], K = O[2], J = O[3], I = 0; if (z.debug) { g(O) } if (!y && P == "onLoad" && M == "player") { y = y || c(v); A = y.clientHeight; m(p, function() { this._fireEvent("onLoad") }); m(s, function(Q, R) { R._fireEvent("onUpdate") }); u._fireEvent("onLoad") } if (P == "onLoad" && M != "player") { return } if (P == "onError") { if (typeof M == "string" || (typeof M == "number" && typeof K == "number")) { M = K; K = J } } if (P == "onContextMenu") { m(z.contextMenu[M], function(Q, R) { R.call(E) }); return } if (P == "onPluginEvent") { var F = M.name || M; var G = s[F]; if (G) { G._fireEvent("onUpdate", M); G._fireEvent(K, O.slice(3)) } return } if (P == "onPlaylistReplace") { p = []; var L = 0; m(M, function() { p.push(new h(this, L++, E)) }) } if (P == "onClipAdd") { if (M.isInStream) { return } M = new h(M, K, E); p.splice(K, 0, M); for (I = K + 1; I < p.length; I++) { p[I].index++ } } var N = true; if (typeof M == "number" && M < p.length) { D = M; var H = p[M]; if (H) { N = H._fireEvent(P, K, J) } if (!H || N !== false) { N = u._fireEvent(P, K, J, H) } } m(B[P], function() { N = this.call(E, M, K); if (this.cached) { B[P].splice(I, 1) } if (N === false) { return false } I++ }); return N }; function C() { if ($f(o)) { $f(o).getParent().innerHTML = ""; w = $f(o).getIndex(); a[w] = E } else { a.push(E); w = a.length - 1 } q = parseInt(o.style.height, 10) || o.clientHeight; if (typeof t == "string") { t = { src: t} } r = o.id || "fp" + e(); v = t.id || r + "_api"; t.id = v; z.playerId = r; if (typeof z == "string") { z = { clip: { url: z}} } if (typeof z.clip == "string") { z.clip = { url: z.clip} } z.clip = z.clip || {}; if (o.getAttribute("href", 2) && !z.clip.url) { z.clip.url = o.getAttribute("href", 2) } u = new h(z.clip, -1, E); z.playlist = z.playlist || [z.clip]; var F = 0; m(z.playlist, function() { var H = this; if (typeof H == "object" && H.length) { H = { url: "" + H} } m(z.clip, function(I, J) { if (J !== undefined && H[I] === undefined && typeof J != "function") { H[I] = J } }); z.playlist[F] = H; H = new h(H, F, E); p.push(H); F++ }); m(z, function(H, I) { if (typeof I == "function") { if (u[H]) { u[H](I) } else { j(B, H, I) } delete z[H] } }); m(z.plugins, function(H, I) { if (I) { s[H] = new l(H, I, E) } }); if (!z.plugins || z.plugins.controls === undefined) { s.controls = new l("controls", null, E) } s.canvas = new l("canvas", null, E); t.bgcolor = t.bgcolor || "#000000"; t.version = t.version || [9, 0]; t.expressInstall = "http://www.flowplayer.org/swf/expressinstall.swf"; function G(H) { if (!E.isLoaded() && E._fireEvent("onBeforeClick") !== false) { E.load() } return f(H) } x = o.innerHTML; if (x.replace(/\s/g, "") !== "") { if (o.addEventListener) { o.addEventListener("click", G, false) } else { if (o.attachEvent) { o.attachEvent("onclick", G) } } } else { if (o.addEventListener) { o.addEventListener("click", f, false) } E.load() } } if (typeof o == "string") { flashembed.domReady(function() { var F = c(o); if (!F) { throw "Flowplayer cannot access element: " + o } else { o = F; C() } }) } else { C() } } var a = []; function d(o) { this.length = o.length; this.each = function(p) { m(o, p) }; this.size = function() { return o.length } } window.flowplayer = window.$f = function() { var p = null; var o = arguments[0]; if (!arguments.length) { m(a, function() { if (this.isLoaded()) { p = this; return false } }); return p || a[0] } if (arguments.length == 1) { if (typeof o == "number") { return a[o] } else { if (o == "*") { return new d(a) } m(a, function() { if (this.id() == o.id || this.id() == o || this.getParent() == o) { p = this; return false } }); return p } } if (arguments.length > 1) { var r = arguments[1]; var q = (arguments.length == 3) ? arguments[2] : {}; if (typeof o == "string") { if (o.indexOf(".") != -1) { var t = []; m(n(o), function() { t.push(new b(this, k(r), k(q))) }); return new d(t) } else { var s = c(o); return new b(s !== null ? s : o, r, q) } } else { if (o) { return new b(o, r, q) } } } return null }; i(window.$f, { fireEvent: function() { var o = [].slice.call(arguments); var q = $f(o[0]); return q ? q._fireEvent(o.slice(1)) : null }, addPlugin: function(o, p) { b.prototype[o] = p; return $f }, each: m, extend: i }); if (typeof jQuery == "function") { jQuery.prototype.flowplayer = function(q, p) { if (!arguments.length || typeof arguments[0] == "number") { var o = []; this.each(function() { var r = $f(this); if (r) { o.push(r) } }); return arguments.length ? o[arguments[0]] : new d(o) } return this.each(function() { $f(this, k(q), p ? k(p) : {}) }) } } })(); (function() { var e = typeof jQuery == "function"; function i() { if (c.done) { return false } var k = document; if (k && k.getElementsByTagName && k.getElementById && k.body) { clearInterval(c.timer); c.timer = null; for (var j = 0; j < c.ready.length; j++) { c.ready[j].call() } c.ready = null; c.done = true } } var c = e ? jQuery : function(j) { if (c.done) { return j() } if (c.timer) { c.ready.push(j) } else { c.ready = [j]; c.timer = setInterval(i, 13) } }; function f(k, j) { if (j) { for (key in j) { if (j.hasOwnProperty(key)) { k[key] = j[key] } } } return k } function g(j) { switch (h(j)) { case "string": j = j.replace(new RegExp('(["\\\\])', "g"), "\\$1"); j = j.replace(/^\s?(\d+)%/, "$1pct"); return '"' + j + '"'; case "array": return "[" + b(j, function(m) { return g(m) }).join(",") + "]"; case "function": return '"function()"'; case "object": var k = []; for (var l in j) { if (j.hasOwnProperty(l)) { k.push('"' + l + '":' + g(j[l])) } } return "{" + k.join(",") + "}" } return String(j).replace(/\s/g, " ").replace(/\'/g, '"') } function h(k) { if (k === null || k === undefined) { return false } var j = typeof k; return (j == "object" && k.push) ? "array" : j } if (window.attachEvent) { window.attachEvent("onbeforeunload", function() { __flash_unloadHandler = function() { }; __flash_savedUnloadHandler = function() { } }) } function b(j, m) { var l = []; for (var k in j) { if (j.hasOwnProperty(k)) { l[k] = m(j[k]) } } return l } function a(q, s) { var o = f({}, q); var r = document.all; var m = '<object width="' + o.width + '" height="' + o.height + '"'; if (r && !o.id) { o.id = "_" + ("" + Math.random()).substring(9) } if (o.id) { m += ' id="' + o.id + '"' } o.src += ((o.src.indexOf("?") != -1 ? "&" : "?") + Math.random()); if (o.w3c || !r) { m += ' data="' + o.src + '" type="application/x-shockwave-flash"' } else { m += ' classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000"' } m += ">"; if (o.w3c || r) { m += '<param name="movie" value="' + o.src + '" />' } o.width = o.height = o.id = o.w3c = o.src = null; for (var j in o) { if (o[j] !== null) { m += '<param name="' + j + '" value="' + o[j] + '" />' } } var n = ""; if (s) { for (var l in s) { if (s[l] !== null) { n += l + "=" + (typeof s[l] == "object" ? g(s[l]) : s[l]) + "&" } } n = n.substring(0, n.length - 1); m += '<param name="flashvars" value=\'' + n + "' />" } m += "</object>"; return m } function d(l, o, k) { var j = flashembed.getVersion(); f(this, { getContainer: function() { return l }, getConf: function() { return o }, getVersion: function() { return j }, getFlashvars: function() { return k }, getApi: function() { return l.firstChild }, getHTML: function() { return a(o, k) } }); var p = o.version; var q = o.expressInstall; var n = !p || flashembed.isSupported(p); if (n) { o.onFail = o.version = o.expressInstall = null; l.innerHTML = a(o, k) } else { if (p && q && flashembed.isSupported([6, 65])) { f(o, { src: q }); k = { MMredirectURL: location.href, MMplayerType: "PlugIn", MMdoctitle: document.title }; l.innerHTML = a(o, k) } else { if (l.innerHTML.replace(/\s/g, "") !== "") { } else { l.innerHTML = "<h2>Flash version " + p + " or greater is required</h2><h3>" + (j[0] > 0 ? "Your version is " + j : "You have no flash plugin installed") + "</h3>" + (l.tagName == "A" ? "<p>Click here to download latest version</p>" : "<p>Download latest version from <a href='http://www.adobe.com/go/getflashplayer'>here</a></p>"); if (l.tagName == "A") { l.onclick = function() { location.href = "http://www.adobe.com/go/getflashplayer" } } } } } if (!n && o.onFail) { var m = o.onFail.call(this); if (typeof m == "string") { l.innerHTML = m } } if (document.all) { window[o.id] = document.getElementById(o.id) } } window.flashembed = function(k, l, j) { if (typeof k == "string") { var m = document.getElementById(k); if (m) { k = m } else { c(function() { flashembed(k, l, j) }); return } } if (!k) { return } var n = { width: "100%", height: "100%", allowfullscreen: true, allowscriptaccess: "always", quality: "high", version: null, onFail: null, expressInstall: null, w3c: false }; if (typeof l == "string") { l = { src: l} } f(n, l); return new d(k, n, j) }; f(window.flashembed, { getVersion: function() { var l = [0, 0]; if (navigator.plugins && typeof navigator.plugins["Shockwave Flash"] == "object") { var k = navigator.plugins["Shockwave Flash"].description; if (typeof k != "undefined") { k = k.replace(/^.*\s+(\S+\s+\S+$)/, "$1"); var m = parseInt(k.replace(/^(.*)\..*$/, "$1"), 10); var q = /r/.test(k) ? parseInt(k.replace(/^.*r(.*)$/, "$1"), 10) : 0; l = [m, q] } } else { if (window.ActiveXObject) { try { var o = new ActiveXObject("ShockwaveFlash.ShockwaveFlash.7") } catch (p) { try { o = new ActiveXObject("ShockwaveFlash.ShockwaveFlash.6"); l = [6, 0]; o.AllowScriptAccess = "always" } catch (j) { if (l[0] == 6) { return l } } try { o = new ActiveXObject("ShockwaveFlash.ShockwaveFlash") } catch (n) { } } if (typeof o == "object") { k = o.GetVariable("$version"); if (typeof k != "undefined") { k = k.replace(/^\S+\s+(.*)$/, "$1").split(","); l = [parseInt(k[0], 10), parseInt(k[2], 10)] } } } } return l }, isSupported: function(j) { var l = flashembed.getVersion(); var k = (l[0] > j[0]) || (l[0] == j[0] && l[1] >= j[1]); return k }, domReady: c, asString: g, getHTML: a }); if (e) { jQuery.tools = jQuery.tools || { version: {} }; jQuery.tools.version.flashembed = "1.0.3"; jQuery.fn.flashembed = function(k, j) { var l = null; this.each(function() { l = flashembed(this, k, j) }); return k.api === false ? this : l } } })();
        
        clientManager.add_pageLoaded(abspop_pageLoaded, "infoPopup");
        clientManager.add_pageUnloaded(abspop_pageUnloaded, "infoPopup");

        function abspop_pageLoaded()
        {
            var c = $find("subMenu");
            if (c)
            {
                c.add_itemClicked(subClicked);
            }
            
            c = $find("tabMenu");
            if (c)
            {
                c.clear();
                c.addItem("tactics", "Tactics", 1);
                c.addItem("msg", "Messaging", 2);
                c.addItem("trn", "Training", 3);
                c.addItem("str", "Strategic Planning", 4);
                c.add_itemClicked(tabClicked);
//                c.selectItem("tactics");
//                c.get_element().click();
            }
            $(".abspannel:first").show();
        }

        function abspop_pageUnloaded()
        {            
            $disposeControl($find("tabMenu"));
            $disposeControl($find("subMenu"));

            clientManager.remove_pageLoaded(abspop_pageLoaded, "infoPopup");
            clientManager.remove_pageUnloaded(abspop_pageUnloaded, "infoPopup");        
        }
        
        function tabClicked(sender, args)
        {
            $(".abspannel").hide();
            
            var c = $find("subMenu");            
            if (c)
            {
                c.clear();

                switch (args.item.value)
                {
                    case 1:
                        c.addItem("1", "Combination Status Card |", 1);
                        c.addItem("2", "Prior Authorization Tip Sheet |", 2);
                        c.addItem("3", "Formulary Sell Sheet |", 3);
                        c.addItem("4", "Prior Authorization Form", 4);         
                        break;
                    case 2:
                        $("#msg_0").show();
                        break;
                    case 3:
                        c.addItem("1", "Pull-Through Lifecycle |", 1);
                        c.addItem("2", "Pull-Through Role Play |", 2);
                        c.addItem("3", "Assessment", 3);                        
                        break;
                    case 4:
                        $("#str_0").show();
                        break;
                }
                c.selectItem("1");
            }
        }

        function subClicked(sender, args)
        {
            $(".abspannel").hide();
        
            var id = $find("tabMenu").get_selectedItem().id + "_" + args.item.id;
            $("#" + id).show();
        }

        function correctAnswer(index)
        {
            $("#answ" + index).show();
            $("#cor" + index).show();
            $("label[for=rad" + index + "]").css("font-weight", "bold");
            $("input[name=rad]").attr("disabled", true);
            $("#rad1").attr("disabled", false);

            var so = new SWFObject("content/imagesabs/resources/player_mp3_maxi.swf", "mp3Player", "140", "22", "8");
            so.addParam("quality", "high");
            so.addParam("wmode", "transparent");
            so.addVariable("config", "content/imagesabs/resources/config2.html");
            so.addVariable("mp3", "content/imagesabs/resources/right.mp3");
            so.write("flashcontent2");
        }

        function openTutorial()
        {
            clientManager.unloadPage("infoPopup");
            $("#infoPopup").hide();
            
            $openWindow("content/imagesabs/resources/form.aspx", null, null, 710, 475);
        }

//        var so = new SWFObject("content/imagesabs/resources/player_mp3_maxi.swf", "mp3Player", "140", "22", "8");
//        so.addParam("quality", "high");
//        so.addParam("wmode", "transparent");
//        so.addVariable("config", "content/imagesabs/resources/config.html");
//        so.addVariable("mp3", "content/imagesabs/resources/doctor.mp3");
//        so.write("flashcontent");


        flowplayer("player", "content/imagesabs/resources/flowplayer-3.1.2.swf", { clip: { autoPlay: false, autoBuffering: false} }); 

        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" Runat="Server">
        <div id="tabMenu" style="height:19px"></div>
        <pinso:Menu runat="server" ID="menuScript" Target="tabMenu" CssClass="absTab" />        
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="tools" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="main" Runat="Server">    
    <div style="background-color:#2d58a7;height:100%">    
        <div style="padding:10px">
            <div id="subMenu"></div>
            <pinso:Menu runat="server" ID="menuScript2" Target="subMenu" CssClass="absSub" SelectedCssClass="selectedSub" />
        </div>    
        <div>
            <div class="abspannel"  style="margin:0px 15px 0px 15px;color:#fff">
                <div style="font-size:21pt;padding:15px 0px 0px 50px">Opportunity!</div>
                
                <div style="font-size:14pt;padding:25px 150px 25px 50px">
                    You have tactics, message, training and strategic planning to complete for this Medicare Part D plan.
                </div>
                <br />
                <div style="font-size:14pt;padding:0px 150px 25px 50px">
                    Please complete these items in preparation for your call to Dr. Jones.
                </div>
            </div>
            <div class="abspannel" id="tactics_1" style="margin:0px 50px 0px 50px">
                <img src="content/imagesabs/tactics_1.png" />
                <br />
                <a href="javascript:void(0)">Download</a><span style="color:#fff;">&nbsp;|&nbsp;</span><a href="javascript:void(0)">View Larger</a>
            </div>
            <div class="abspannel" id="tactics_2" style="margin:0px 50px 0px 50px">
                <img src="content/imagesabs/tactics_2.png" />
                <br />
                <a href="javascript:void(0)">Download</a><span style="color:#fff;">&nbsp;|&nbsp;</span><a href="javascript:void(0)">View Larger</a>        
            </div>
            <div class="abspannel" id="tactics_3" style="margin:0px 50px 0px 50px">
                <img src="content/imagesabs/tactics_3.png" />
                <br />
                <a href="javascript:void(0)">Download</a><span style="color:#fff;">&nbsp;|&nbsp;</span><a href="javascript:void(0)">View Larger</a>        
            </div>
            <div class="abspannel" id="tactics_4" style="margin:0px 50px 0px 50px">
                <img src="content/imagesabs/182_HumanaPA_Form.jpg" style="height:386px" />
                <br />
                <a href="javascript:void(0)">Download</a><span style="color:#fff;">&nbsp;|&nbsp;</span><a href="javascript:void(0)">View Larger</a>        
            </div>            
                        
            <div class="abspannel" id="msg_0" style="margin:0px 15px 0px 15px">
                <div style="height:385px;background-color:#fff;padding:20px 20px 10px 10px">
                    <p style="font-size:13pt;color:#666666;word-spacing:5px;">STANDARD MESSAGE FOR PHYSICIAN WHERE ACCESS HAS BEEN
                    IMPROVED FOR A PART D PLAN.</p>
                    <br />
                    <p style="font-size:9pt;color:#2d58a7;text-align:left">Doctor, I have good news for your patients for whom you prescribe Product X. <span style='font-weight:bold;'>Humana PDP Enhanced</span>,
                    a PDP plan that manages drug coverage for <span style='font-weight:bold;'>30%</span> of Part D insured patients in this area now requires a
                    lower co-payment for Product X of <span style='font-weight:bold;'>$15</span>. <img class="Popup_msg" src="content/imagesabs/msg_0.png"  />This means that you can prescribe Product X with the added confidence of fewer pharmacy
                    call-backs and patient complaints related to formulary restrictions.
                    </p>
                    <div id="flashcontent" style="margin:80px 0px 0px 25px">
                      <img src="content/imagesabs/mp3.png" />
                    </div>                  
                </div>
                <a href="javascript:void(0)">Export</a>                                
            </div>
            
            <div class="abspannel" id="trn_1" style="margin:0px 15px 0px 15px">
                <div style="height:380px;background-color:#fff;padding:10px 10px 10px 10px;font-weight:bold;">
                     <div style="margin:50px auto auto auto;">
                        <div>Matching Pull-Through to Product Lifecycles</div>
                        <img src="content/imagesabs/trn_1.png" />
                    </div>
                </div>
                <a href="javascript:void(0)">Download</a>                                
            </div>
            <div class="abspannel" id="trn_2" style="margin:0px 15px 0px 15px">
                <div style="height:380px;background-color:#fff;padding:10px 10px 10px 10px;font-weight:bold;">
                    <h2 style="color:#555555">Medicare Part D Preferred Formulary Position (T2).</h2>
                    <br />
                    <div style="font-size:9pt;color:#2d58a7;text-align:left">
                        Product X is listed on Tier 2 as a preferred product at Humana Part D.  What points might you mention during the call to help drive utilization?
                        <br />
                        <ul>
                            <li>Mention that the P&T has identified Product X as a safe, efficacious, and cost-effective therapy.</li>
                            <li>Differentiate Product X from competitive anti-depressant therapies by emphasizing the features and benefits of the product compared to the competition.</li>
                            <li>Note that co-payments on Tier 2 tend to be reasonable, helping to make Product X affordable for patients.</li>
                            <li>Encourage long-term use of Product X after successful trial starts.</li>
                        </ul>
                        <br />    
                        <div style="float:left;width:50%">
                            <h3 style="color:#666666">KEY POINTS: CLOSE STRATEGICALLY BY SECURING A COMMITMENT TO ACTION</h3>
                            <ul>
                                <li>Short term (Patient starts)</li>
                                <li>Long Term (Advocate)</li>
                                <li>Acceptance takes time</li>
                                <li>Preferred position alone does not drive utilization</li>
                                <li>Differentiate Product X from the competition!</li>
                            </ul>
                        </div>                     
                        <div style="float:right;width:50%">
                            <a href="content/imagesabs/resources/roleplay.flv" style="border:solid 3px #2d58a7;display:block;width:300px;height:200px;" id="player"></a> 
                        </div>
                        <div class="clearAll"></div>
                    </div>
                </div>    
                <a href="javascript:void(0)">Export</a>          
            </div>
            <div class="abspannel" id="trn_3" style="margin:0px 15px 0px 15px">
                <div style="height:380px;color:#2d58a7;background-color:#fff;padding:10px 10px 10px 10px;">
                    <div style="float:left;"><h2 style="color:#666666">ASSESSMENT</h2></div><div style="float:right;">< 1 of 10 ></div>
                    <br />
                    <br />
                    <h3>DISTINCTIONS AMONG PREFERRED BRANDS</h3>
                    A preferred brand is sometimes granted “exclusive” status on Tier 2, which means that it is the only preferred Tier 2 product in its class or category. In other cases, a branded medication shares equal status with other brands in a class or category. What is the term for this latter situation?
                    <br />
                    <br />
                    <div style="vertical-align:middle">
                        <input type="radio" name="rad" onclick="correctAnswer(1)" id="rad1" /><label for="rad1">A. Neutral</label><span style="display:none;font-weight:bold;" id="cor1">&nbsp;(correct)</span><br />
                        <input type="radio" name="rad" id="rad2" /><label for="rad2">B. Co-Tiered</label><br />
                        <input type="radio" name="rad" id="rad3" /><label for="rad3">C. Multisource</label><br />
                        <input type="radio" name="rad" id="rad4" /><label for="rad4">D. Dual-Preferred</label><br />
                    </div>
                    <div style="margin:20px;display:none;" id="answ1">
                        <h2 style="color:#666666">FEEDBACK</h2>
                        That’s right. Preferred brands can share equal Tier 2 status with other brands in a class or category, and this is called “parity” (or sometimes “at par”). From a strategic selling perspective, the product is said to be in a “neutral” position. To sell effectively, sales representative must differentiate their parity medications from other parity medications when promoting to physicians.
                        <div id="flashcontent2">This text is replaced by the Flash movie.</div>                         
                    </div>
                </div>
                <a href="javascript:void(0)">Export</a>  
            </div>        
            
            <div class="abspannel" id="str_0" style="margin:0px 15px 0px 15px">
 
                <div style="height:395px;color:#2d58a7;background-color:#fff;padding:10px 10px 10px 10px;">
                    <div style="float:left;width:50%">
                        <img src="content/imagesabs/str_0a.png" style="height:375px;border:solid 3px #2d58a7" />
                    </div>
                    <div style="float:right;width:50%;">
                        <div style="height:200px;vertical-align:middle;padding-top:20px">
                            <a style="color:#2d58a7;font-weight:bold;font-size:16pt" href="javascript:openTutorial()">Form Tutorial</a>
                        </div>
                        <div>
                            <img src="content/imagesabs/str_0b.png" style="height:110px;border:solid 3px #2d58a7"  />
                            <img src="content/imagesabs/str_0c.png" style="height:110px;border:solid 3px #2d58a7"  />
                        </div>
                    </div>
                    <div class="clearAll"></div>
                </div>                    
                <a href="javascript:void(0)">Download</a><span style="color:#fff;">&nbsp;|&nbsp;</span><a href="javascript:void(0)">View Larger</a>                    
            </div>
        </div>
    </div>
</asp:Content>

