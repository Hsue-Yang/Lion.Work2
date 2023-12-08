var _formElement;

function SystemSettingForm_onLoad(formElement) {
    _formElement = formElement;
    var table = $('#SystemSettingTable', _formElement);

    if (table.length > 0) {
        table.hide();
        var width = $(window).width();
        table.show();
        var height = $(window).height() - table.offset().top - $('.SelectTable').height();
        table.freezePanes({ width: width + 'px', height: height + 'px' });
    }

    return true;
}

function DetailLinkFunKey_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#SysID').val(keys[1]);
    $('#SysNM').val(encodeURI(keys[2]));

    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}

function IPLinkFunKey_onClick(srcElement, keys) {
    Clean_HiddenValue();

    var aryPara = [];
    aryPara[aryPara.length] = 'SysID=' + keys[1];
    aryPara[aryPara.length] = 'SysNM=' + encodeURI(keys[2]);
    aryPara[aryPara.length] = 'ExecAction=' + _ActionTypeUpdate;

    var objfeatures = { width: 1500, height: 800 };

    _openWin('newwindow', '/Sys/SystemIPList', aryPara.join('&'), objfeatures);

    return false;
}

function ServiceLinkFunKey_onClick(srcElement, keys) {
    Clean_HiddenValue();

    var aryPara = [];
    aryPara[aryPara.length] = 'SysID=' + keys[1];
    aryPara[aryPara.length] = 'SysNM=' + encodeURI(keys[2]);
    aryPara[aryPara.length] = 'ExecAction=' + _ActionTypeUpdate;

    var objfeatures = { width: 800, height: 600 };

    _openWin('newwindow', '/Sys/SystemServiceList', aryPara.join('&'), objfeatures);

    return false;
}

function SubsystemLinkFunKey_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#SysID').val(keys[1]);
    $('#SysNM').val(encodeURI(keys[2]));

    $('#ExecAction').val(_ActionTypeSelect);
    return true;
}

function ListLinkFunKey_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#SysID').val(keys[1]);
    $('#SysNM').val(encodeURI(keys[2]));

    $('#ExecAction').val(_ActionTypeSelect);
    return true;
}

function LinkFunKey_onClick(srcElement, keys) {
    var tr = $('tr[id="SubsystemList[' + keys[1] + ']"]');
    tr.toggle();

    return false;
}

//----Button----//
function AddButton_onClick(srcElement) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#SysID').val($('#QuerySysID').val());

    $('#ExecAction').val(_ActionTypeAdd);
    return true;
}

//----Private Function----//
function Clean_HiddenValue() {
    $('#SysID').val('');
}

function AfterIconDownload() {
    $.unblockUI();
}