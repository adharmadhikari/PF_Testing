/*
CSS Browser Selector v0.3.4 (Sep 29, 2009)
Rafael Lima (http://rafael.adm.br)
http://rafael.adm.br/css_browser_selector
License: http://creativecommons.org/licenses/by/2.5/
Contributors: http://rafael.adm.br/css_browser_selector#contributors

<style type="text/css">
.ie .example {
  background-color: yellow
}
.ie7 .example {
  background-color: orange
}
.gecko .example {
  background-color: gray
}
.win.gecko .example {
  background-color: red
}
.linux.gecko .example {
  background-color: pink
}
.opera .example {
  background-color: green
}
.konqueror .example {
  background-color: blue
}
.webkit .example {
  background-color: black
}
.chrome .example {
  background-color: cyan
}
.example {
  width: 100px;
  height: 100px;
}
.no_js { display: block }
.has_js { display: none }
.js .no_js { display: none }
.js .has_js { display: block }
</style>

Examples:

    * html.gecko div#header { margin: 1em; }
    * .opera #header { margin: 1.2em; }
    * .ie .mylink { font-weight: bold; }
    * .mac.ie .mylink { font-weight: bold; }
    * .[os].[browser] .mylink { font-weight: bold; } -> without space between .[os] and .[browser]

Available OS Codes [os]:

    * win - Microsoft Windows
    * linux - Linux (x11 and linux)
    * mac - Mac OS
    * freebsd - FreeBSD
    * ipod - iPod Touch
    * iphone - iPhone
    * webtv - WebTV
    * mobile - J2ME Devices (ex: Opera mini)

Available Browser Codes [browser]:

    * ie - Internet Explorer (All versions)
    * ie8 - Internet Explorer 8.x
    * ie7 - Internet Explorer 7.x
    * ie6 - Internet Explorer 6.x
    * ie5 - Internet Explorer 5.x
    * gecko - Mozilla, Firefox (all versions), Camino
    * ff2 - Firefox 2
    * ff3 - Firefox 3
    * ff3_5 - Firefox 3.5 new
    * opera - Opera (All versions)
    * opera8 - Opera 8.x
    * opera9 - Opera 9.x
    * opera10 - Opera 10.x
    * konqueror - Konqueror
    * webkit or safari - Safari, NetNewsWire, OmniWeb, Shiira, Google Chrome
    * safari3 - Safari 3.x
    * chrome - Google Chrome
    * iron - SRWare Iron new

Extra Codes:

    * js - Will be available when js is enable, so you can show/hide some stuffs




*/
function css_browser_selector(u){var ua = u.toLowerCase(),is=function(t){return ua.indexOf(t)>-1;},g='gecko',w='webkit',s='safari',o='opera',h=document.getElementsByTagName('html')[0],b=[(!(/opera|webtv/i.test(ua))&&/msie\s(\d)/.test(ua))?('ie ie'+RegExp.$1):is('firefox/2')?g+' ff2':is('firefox/3.5')?g+' ff3 ff3_5':is('firefox/3')?g+' ff3':is('gecko/')?g:is('opera')?o+(/version\/(\d+)/.test(ua)?' '+o+RegExp.$1:(/opera(\s|\/)(\d+)/.test(ua)?' '+o+RegExp.$2:'')):is('konqueror')?'konqueror':is('chrome')?w+' chrome':is('iron')?w+' iron':is('applewebkit/')?w+' '+s+(/version\/(\d+)/.test(ua)?' '+s+RegExp.$1:''):is('mozilla/')?g:'',is('j2me')?'mobile':is('iphone')?'iphone':is('ipod')?'ipod':is('mac')?'mac':is('darwin')?'mac':is('webtv')?'webtv':is('win')?'win':is('freebsd')?'freebsd':(is('x11')||is('linux'))?'linux':'','js']; c = b.join(' '); h.className += ' '+c; return c;}; css_browser_selector(navigator.userAgent);
