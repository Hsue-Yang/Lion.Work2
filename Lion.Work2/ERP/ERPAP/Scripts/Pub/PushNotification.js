var _formElement;

function PushNotificationForm_onLoad(formElement) {
    _formElement = formElement;

    var table = $('#PushNotificationTable', _formElement);

    if (table.length > 0) {
        table.hide();
        var width = $(window).width();
        table.show();
        var height = $(window).height() - table.offset().top;
        $('.tblsearch').each(function () { height -= $(this).height(); });
        table.freezePanes({ width: width + 'px', height: height + 'px', fixedCols: 1 });
    }

    $('.BaseContainer').css('z-index', 0);

    $('#AppFunID, #PushStatus', $('table.tblsearch', _formElement)).combobox(
        {
            isRemoveButton: true
        }
    );
}

function AddButton_onClick(srcElement) {
    $('#ExecAction').val(_ActionTypeAdd);
    return true;
}

function PageSize_onEnter(srcElement) {
    SelectButton_onClick();
    return true;
}

function SelectButton_onClick(srcElement) {
    if (_PushDateValidation() && _FormValidation()) {
        $('#ExecAction').val(_ActionTypeSelect);
        $.blockUI({ message: '' });
        _formElement.submit();
    }

    _ShowJsErrMessageBox();
    return false;
}

function _PushDateValidation(srcElement) {
    var result = true;
    var startPushDT = Date.parse($('#StartPushDT').val().replace(/(\d{4})(\d\d)(\d\d)/g, '$1-$2-$3'));
    var endPushDT = Date.parse($('#EndPushDT').val().replace(/(\d{4})(\d\d)(\d\d)/g, '$1-$2-$3'));

    if (Math.abs(parseInt((endPushDT - startPushDT) / 86400000)) > 31) {
        _AddJsErrMessage(JsMsg_PushDateRange_Error);
        result = false;
    }

    if (parseInt(startPushDT - endPushDT) > 0) {
        _AddJsErrMessage(JsMsg_PushDate_Error);
        result = false;
    }

    return result;
}

function Help03Button_onClick(srcElement) {
    var vMapFields = new Array(1);
    vMapFields[1] = 'UserID';
    _hISearch(vMapFields, 'newwindow', _enumPUBAP + '/Help/Ishlp03', 'Name=' + encodeURI($.trim($('#UserID').val())));
}

function CancelPushMsgButton_onClick(srcElement) {
    if ($('input[name=CancelPushMsgList]:checked').length > 0) {
        $('#ExecAction').val(_ActionTypeDelete);
        return true;
    } else {
        _AddJsErrMessage(JsMsg_ChooseData);
        _ShowJsErrMessageBox();
    }
    return false;
}

function PushErrorMsgButton_onClick(srcElement, key) {
    var para = 'MessageID=' + key[1] + '&UserID=' + key[2] + '&AppUUID=' + key[3];
    _openWin('PushNotificationErrorMsg', '/Pub/PushNotificationErrorMsg', para, { width: 450, height: 320 });
    return false;
}