var _formElement;
var _execEDIEventNo;
var _targetSysID;

function SystemEventTargetEDIForm_onLoad(formElement) {
    _formElement = formElement;
    var table = $('#SystemEventTargetEDITable', _formElement);

    if (table.length > 0) {
        var width = $(window).width();
        table.show();
        var height = $(window).height() - table.offset().top;
        $('table[id=Pager]').each(function () { height -= $(this).height(); });
        table.freezePanes({ width: width + 'px', height: height + 'px', fixedCols: 0 });
    }

    $('.BaseContainer').css('z-index', 0);

    $('table.tblsearch #QueryTargetSysID', _formElement).combobox({
        width: 180,
        isRemoveButton: true
    });

    return true;
}

function LinkFunKeyTargetDetail_onClick(srcElement, keys) {
    var para = 'ExecEDIEventNo=' + keys[1];

    var objfeatures = { width: 450, height: 500 };

    _openWin('newwindow', '/Sys/SystemEventTargetDetail', para, objfeatures);
    return false;
}

function LinkFunKeyTargetEDI_onClick(srcElement, keys) {
    $.blockUI({ message: '' });

    _execEDIEventNo = keys[1];

    _alert('dialog_Confirm');
}

function LinkFunKeySpecifyResend_onClick(srcElement, keys) {
    $.blockUI({ message: '' });

    _targetSysID = keys[1];
    _execEDIEventNo = keys[2];

    _alert('dialog_Confirm');
}

function LinkFunKeyTargetLog_onClick(srcElement, keys) {
    var para = 'EDIDate=' + keys[1] + '&EDITime=' + keys[2] + '&QuerySysID=' + keys[3];

    var objfeatures = { width: 550, height: 500 };

    _openWin('newwindow', '/Sys/SystemEventTargetLog', para, objfeatures);
    return false;
}

function LinkFunKeyAPIDetailByAPINo_onClick(srcElement, keys) {
    var para = 'APINo=' + keys[1];

    var objfeatures = { width: 800, height: 600 };

    _openWin('newwindow', '/Sys/SystemAPIClientDetail', para, objfeatures);
    return false;
}

function PageSize_onEnter(srcElement) {
    SelectButton_onClick();
    return true;
}

//----Tab----//
function SystemEventTargetSend_onClick(srcElement) {
    $(srcElement).attr('con', 'Sys');
    $(srcElement).attr('act', 'SystemEventTargetSend');
    _submit($(srcElement));
    return false;
}

//----Button----//
function SelectButton_onClick(srcElement) {
    var result = _FormValidation();
    $('#ExecAction').val(_ActionTypeSelect);
    if (result) {
        $.blockUI({ message: '' });
        $('#PageIndex').val(1);
        _formElement.submit();
    }
}

function EventParaButton_onClick(srcElement) {
    var para = 'SysID=' + $('#SysID').val() + '&'
        + 'EventGroupID=' + $('#EventGroupID').val() + '&'
        + 'EventID=' + $('#EventID').val();

    var objfeatures = { width: 450, height: 500 };

    _openWin('newwindow', '/Sys/SystemEventPara', para, objfeatures);
    return false;
}

function CancelButton_onClick(srcElement) {
    $.blockUI({ message: '' });

    location.href = '/Sys/SystemEvent';
}

function ConfirmOKButton_onClick(srcElement) {
    Clean_HiddenValue();

    $('#ExecEDIEventNo').val(_execEDIEventNo);
    $('#TargetSysID').val(_targetSysID);

    $('#ExecAction').val(_ActionTypeAdd);
    _formElement.submit();
}

function ConfirmNOButton_onClick(srcElement) {
    _btnUnblockUI(this, false);
}

//----Private Function----//
function Clean_HiddenValue() {
    $('#ExecEDIEventNo').val('');
}
