﻿$(function () {	
	var head  = document.getElementsByTagName('head')[0];
    var link  = document.createElement('link');    
    link.rel  = 'shortcut icon';
	
    if (document.domain.indexOf('liontravel.com.tw') > -1) {
		link.href = 'http://userp.liontravel.com.tw/favIcon.ico';
    } else {
		link.href = 'https://serp.liontravel.com/favIcon.ico';
    }

    head.appendChild(link);
});

//#region 另開視窗回傳值

function _returnKey(rc) {
	document.domain = _domain;
    Ini_decData();
    var wIndex = parent.opener.help_index;
    if (wIndex == undefined) { wIndex = 0 }
    for (var i = 1; i <= parent.opener._decData.vMapFields.length - 1; i++) {
    	if (parent.opener._decData.vMapFields[i] != "" && parent.opener._decData.vMapFields[i] != undefined) {
    		var el = $("[name='" + parent.opener._decData.vMapFields[i] + "']", parent.opener.window.document);
    		if (el[wIndex] != undefined) {
        		switch (el[wIndex].tagName) {
                	case "LABEL":
                		el[wIndex].innerText = document.getElementById('' + rc + '' + i).value;
                		break;
                    case "SPAN":
                        el[wIndex].innerText = document.getElementById('' + rc + '' + i).value;
                        break;
                    default:
                    	if ("undefined" != typeof (el[wIndex].setValue))
                        	el[wIndex].setValue(document.getElementById('' + rc + '' + i)[0].value);
                        else
                            el[wIndex].value = document.getElementById('' + rc + '' + i).value;
                        break;
                }
                if (el[wIndex].tagName == "INPUT")
                    _hIChanged(el[wIndex]);
            }
        }
    }
	var Result = true;
	if (parent.opener._decData.vMapFields[0] > "") {
		var functionName = parent.opener._decData.vMapFields[0];
		eval("if(typeof(parent.opener." + functionName + ") != undefined){ Result = parent.opener." + functionName + "(); }");
		
	}
	if (Result) {
		parent.opener.help_index = 0;
		parent.window.close();
	}
}
//#endregion
