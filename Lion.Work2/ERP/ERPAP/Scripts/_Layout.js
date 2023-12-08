$(function () {
    _UserMenu_onLoad();
    $('#UserSystemNotificationLoading a').html(_JsMsg_AjaxMessageLoading);

    //設定title
    $('[title]').tooltip();

    $('#topnav ul > li:eq(0) ul li a').each(function (e, i) {
        //語系拿掉國家
        if ($(i).html().split('(').length > 1) {
            var text = $(i).html().split('(')[1].split(')')[0];
            $(i).html(text);
        }
    });

    if ($(window).width() > 900) {
        var script = document.createElement('script');
        script.src = _enumERPAP + '/Scripts/MoreMenuBtn.js';
        script.type = 'text/javascript';
        document.head.appendChild(script);
    }
});

function LinkFunKeyUserMain_onClick(srcElement, keys) {
    
    var objfeatures = { width: 690, height: 210 };

    if (document.domain.indexOf('liontravel.com.tw') > -1) {
        window.open("http://uhcm.liontravel.com.tw/Staff/ProfileMaintain", "LionTech_blank", 'toolbar=no,location=no,status=no,menubar=no,resizable=yes,scrollbars=yes,left=0,top=0,width=1024,height=280', true);
    } else {
        window.open("https://hcm.liontravel.com/Staff/ProfileMaintain", "LionTech_blank", 'toolbar=no,location=no,status=no,menubar=no,resizable=yes,scrollbars=yes,left=0,top=0,width=1024,height=280', true);
    }
    
    return false;
}

function LinkFunKeyING_onClick(srcElement, keys) {
    var para = 'SystemID=' + keys[1];

    _openWin('newwindow', _enumERPAP + '/Pub/Bulletin', para, null);
    return false;
}

function GoogleSearchText_onEnter(srcElement) {
    GoogleSearchButton_onClick();
}

function GoogleSearchButton_onClick(srcElement) {
    var d = new Date();
    if (d.getFullYear() > 2016 && d.getDate() > 17) {
        alert('"Google Search合約到期日為2017/07/17，即日起將停止服務。');
        return;
    }
    document.domain = _domain;
    var gsaUrl = 'https://gsa.liontravel.com/search',
        gsaParam = [],
        wmpSystemID = 'WMPAP',
        googleSearchText = $('#GoogleSearchText').val(),
        iframeName = 'GSAIframe',
        iframe = $('<iframe name="' + iframeName + '" style="position:absolute;top:-9999px" />').appendTo('body'),
        documentForm = $(document.createElement('form'))
            .attr({
                'action': _enumERPAP + '/Home/GsaSSOLogin',
                'target': iframeName,
                'method': 'get'
            })
            .append(
                $(document.createElement('input')).attr({
                    'type': 'hidden',
                    'name': 'systemID'
                }).val(wmpSystemID)
            )
            .appendTo('body');

    gsaParam[gsaParam.length] = 'access=a';
    gsaParam[gsaParam.length] = 'client=google_frontend';
    gsaParam[gsaParam.length] = 'output=xml_no_dtd';
    gsaParam[gsaParam.length] = 'proxystylesheet=google_frontend';
    gsaParam[gsaParam.length] = 'wc=200';
    gsaParam[gsaParam.length] = 'wc_mc=1';
    gsaParam[gsaParam.length] = 'oe=UTF-8';
    gsaParam[gsaParam.length] = 'ie=UTF-8';
    gsaParam[gsaParam.length] = 'ud=1';
    gsaParam[gsaParam.length] = 'exclude_apps=1';
    gsaParam[gsaParam.length] = 'site=erp_collection';
    gsaParam[gsaParam.length] = 'portal=erp';
    gsaParam[gsaParam.length] = 'filter=0';
    gsaParam[gsaParam.length] = 'getfields=*';

    documentForm.submit(function () {
        $.blockUI({ message: '', baseZ: 9999 });
        iframe.load(function () {
            try {
                $.unblockUI();
                var contents = $(iframe).contents().get(0);
                var result = $(contents).children()[0].innerText;

                if (result.toUpperCase() === 'TRUE') {
                    gsaParam[gsaParam.length] = 'q=' + encodeURI(googleSearchText);
                    window.open(gsaUrl + '?' + gsaParam.join('&'));
                } else {
                    _AddJsErrMessage($('ErrorMessage', contents).html());
                    _ShowJsErrMessageBox();
                }
            } catch (ex) {
                console.log(ex);
            } finally {
                iframe.remove();
                documentForm.remove();
            }
        });
    }).submit();

    return true;
}

var AjaxUserSystemNotificationBox = null;
var isUserSystemNotificationClicked = false;
var isUserSystemNotificationNotFoundMessaged = false;
function _AjaxUserSystemNotificationBox() {
    $.ajax({
        url: '/_BaseAP/GetUserSystemNotificationList',
        type: 'POST',
        data: {
            dataIndex: $('#UserSystemNotificationBox li a').length - $('#UserSystemNotificationLoading').length
        },
        dataType: 'json',
        async: false,
        success: function (result) {
            $('#UserSystemNotificationUnReadCountBox').remove();
            if (result != null &&
                result.length > 0) {
                $('#UserSystemNotificationLoading').remove();
                for (var i = 0; i < result.length; i++) {
                    var li = $(document.createElement('li'));
                    var textLink = $(document.createElement('a'));
                    textLink.attr('href', (result[i].URL > '' ? result[i].URL : 'javascript:void();'));
                    textLink.html(result[i].Content);
                    if (result[i].IsRead === 'N') {
                        li.css('background-color', '#ecf0f7');
                    }
                    li.append(textLink);
                    $('#UserSystemNotificationBox', _formElement).append(li);
                    $('#UserSystemNotificationBox', _formElement).append('<li class="divider"></li>');
                }
            } else {
                isUserSystemNotificationNotFoundMessaged = true;
                clearInterval(AjaxUserSystemNotificationBox);
            }
            if ($('#UserSystemNotificationLoading').length === 1) {
                $('#UserSystemNotificationLoading a').html(_JsMsg_AjaxNotFoundMessaged);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
}

function UserSystemNotificationIcon_onClick(srcElement) {
    if (200 > $('#UserSystemNotificationBox').height() + 10 &&
        isUserSystemNotificationClicked === false &&
        isUserSystemNotificationNotFoundMessaged === false) {
        isUserSystemNotificationClicked = true;
        if (AjaxUserSystemNotificationBox != null) clearInterval(AjaxUserSystemNotificationBox);
        AjaxUserSystemNotificationBox = setTimeout('_AjaxUserSystemNotificationBox()', 500);
    }
}

function UserSystemNotificationBox_onScroll(srcElement) {
    if ((srcElement.scrollHeight === srcElement.scrollTop + $(srcElement).height() + 10) &&
        isUserSystemNotificationNotFoundMessaged === false) {
        if (AjaxUserSystemNotificationBox != null) clearInterval(AjaxUserSystemNotificationBox);
        AjaxUserSystemNotificationBox = setTimeout('_AjaxUserSystemNotificationBox()', 500);
    }
}