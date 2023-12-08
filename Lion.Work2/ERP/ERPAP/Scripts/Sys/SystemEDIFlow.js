var _formElement, _serviceStatus;

function SystemEDIFlowForm_onLoad(formElement) {
    _formElement = formElement;
    var table = $('#SystemEDIFlowTable', _formElement);

    if (table.length > 0) {
        table.hide();
        var width = $(window).width();
        table.show();
        var height = $(window).height() - table.offset().top;
        $('.tblsearch').each(function () { height -= $(this).height(); });
        table.freezePanes({ width: width + 'px', height: height + 'px', fixedCols: 0 });
    }

    $('.BaseContainer').css('z-index', 0);

    $('#QuerySysID', _formElement).combobox({
        width: 180,
        isRemoveButton: false,
        onChange: QuerySysID_onChange
    });

    $('#QuerySCHFrequency', _formElement).combobox({
        width: 180,
        isRemoveButton: true
    });

    _GetSystemEDIServiceStatus();
    return true;
}

function LinkFunKeyDetail_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#SysID').val(keys[1]);
    $('#EDIFlowID').val(keys[2]);

    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}

function LinkFunKeyJob_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#QuerySysID').val(keys[1]);
    $('#QueryEDIFlowID').val(keys[2]);

    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}

function LinkFunKeyCon_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#QuerySysID').val(keys[1]);
    $('#QueryEDIFlowID').val(keys[2]);

    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}

function LinkFunKeyFlowLog_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#QuerySysID').val(keys[1]);
    $('#QueryEDIFlowID').val(keys[2]);

    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}
//排序
function MoveUp_onClick(srcElement) {
    var after;
    var current = $('input[name="IsMoved"]:checked').parent().parent();
    var prev = current.prev();
    if (prev[0] != undefined) {
        current.insertBefore(prev);
        after = prev.find('input[name$="AfterSortOrder"]').val();
        if (after != null) {
            prev.find('input[name$="AfterSortOrder"]').val(current.find('input[name$="AfterSortOrder"]').val());
            current.find('input[name$="AfterSortOrder"]').val(after);
        }
    }
}

function MoveDown_onClick(srcElement) {
    var after;
    var current = $('input[name="IsMoved"]:checked').parent().parent();
    var next = current.next();
    if (next[0] != undefined) {
        current.insertAfter(next);
        after = next.find('input[name$="AfterSortOrder"]').val();
        if (after != null) {
            next.find('input[name$="AfterSortOrder"]').val(current.find('input[name$="AfterSortOrder"]').val());
            current.find('input[name$="AfterSortOrder"]').val(after);
        }
    }
}

//----Button----//
function SearchButton_onClick(srcElement) { //按下搜尋，驗證combobox有無值
    var result = _FormValidation();
    $('#ExecAction').val(_ActionTypeSelect);
    if (result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function AddButton_onClick(srcElement) {
    var result = _FormValidation(); //form驗證
    if (result) {
        $.blockUI({ message: '' }); //按下按鈕跳轉前，畫面會鎖住
        Clean_HiddenValue();

        $('#SysID').val($('#QuerySysID').val());
        $('#ExecAction').val(_ActionTypeAdd); //jQuery套件執行Action (有底線為套件)
        return true;
    }
}

function CopyButton_onClick(srcElement) {
    var ediFlowId = $('input[name=IsMoved]:checked');
    if (ediFlowId.length > 0 && _FormValidation()) {
        $.blockUI({ message: '' });
        $('#SysID').val($('#QuerySysID').val());
        $('#EDIFlowID').val($('input[name$=EDIFlowID]', $('input[name=IsMoved]:checked').closest('td')).val());
        $('#ExecAction').val(_ActionTypeCopy);
        return true;
    } else {
        _AddJsErrMessage(window.JsMsg_EdiFlowChoose);
        _ShowJsErrMessageBox();
        return false;
    }
}

function OutputButton_onClick(srcElement) {
    $('#SysID').val($('#QuerySysID').val());
        $.blockUI({ message: '' });

        _alert('dialog_Confirm');
}
function SaveButton_onClick(srcElement) {
    $('#ExecAction').val(_ActionTypeUpdate);
    _formElement.submit();

}
//----Private Function----//
function Clean_HiddenValue() {
    //清空 hidden 內值
    $('#SysID').val('');
    $('#EDIFlowID').val('');
}

function ConfirmOKButton_onClick(srcElement) {
    var result = _FormValidation();

    $('#ExecAction').val(_ActionTypeCopy);
    if (result) {
        _formElement.submit();
    }

    _btnUnblockUI(this, false);
}

function ConfirmNOButton_onClick(srcElement) {
    _btnUnblockUI(this, false);
}

function QuerySysID_onChange(srcElement) {
    _GetSystemEDIServiceStatus();
}

function OperationButton_onClick(srcElement) {
    var sysID = $('#QuerySysID').val();
    if (sysID !== '') {
        $.blockUI({ message: '' });
        $.ajax({
            url: '/Sys/SystemEDIServiceExcute',
            type: 'POST',
            data: {
                QuerySysID: $('#QuerySysID').val(),
                ServiceStatus: _serviceStatus
            },
            dataType: 'json',
            async: false,
            success: function (response) {
                if (response && response.isSuccess) {
                    _serviceStatus = response.serviceStatus;
                    $(srcElement).attr('disabled', 'disabled');
                    if (response.serviceStatus === 'Running' ||
                        response.serviceStatus === 'Stopped') {
                        $('#OperationButton').removeAttr('disabled');

                        if (response.serviceStatus === 'Running') {
                            $('#ServiceStatus').css('color', 'blue');
                            $('#OperationButton').val(window.JsMsg_ServiceStop);
                        }
                        if (response.serviceStatus === 'Stopped') {
                            $('#ServiceStatus').css('color', 'red');
                            $('#OperationButton').val(window.JsMsg_ServiceStart);
                        }
                    }
                    $('#ServiceStatus').html(response.statusName);
                }

                $.unblockUI();
                return false;
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $.unblockUI();
                _AddJsErrMessage(window.JsMsg_SystemEDIServiceControl_Error);
                _ShowJsErrMessageBox();
            }
        });
    }
}

function _GetSystemEDIServiceStatus() {
    var sysID = $('#QuerySysID').val();
    $('#ServiceControlBox').hide();
    if (sysID !== '') {
        $.blockUI({ message: '' });
        $.ajax({
            url: '/Sys/SystemEDIServiceStatus',
            type: 'POST',
            data: {
                QuerySysID: $('#QuerySysID').val()
            },
            dataType: 'json',
            async: false,
            success: function (response) {
                if (response && response.isSuccess) {
                    _serviceStatus = response.serviceStatus;
                    $('#OperationButton').attr('disabled', 'disabled');
                    if (response.serviceStatus === 'Running' ||
                        response.serviceStatus === 'Stopped') {
                        $('#OperationButton').removeAttr('disabled');

                        if (response.serviceStatus === 'Running') {
                            $('#ServiceStatus').css('color', 'blue');
                            $('#OperationButton').val(window.JsMsg_ServiceStop);
                        }
                        if (response.serviceStatus === 'Stopped') {
                            $('#ServiceStatus').css('color', 'red');
                            $('#OperationButton').val(window.JsMsg_ServiceStart);
                        }
                    }
                    $('#ServiceStatus').html(response.statusName);
                    $('#ServiceControlBox').show();
                }

                $.unblockUI();
                return false;
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $.unblockUI();
                _AddJsErrMessage(window.JsMsg_SystemEDIServiceStatus_Error);
                _ShowJsErrMessageBox();
            }
        });
    }
}

function SystemEDIFlowDirButton_onClick(srcElement) {
    var para = '&QuerySysID=' + $('#QuerySysID').val() +
        '&ExecAction=' + _ActionTypeSelect;

    _openWin('SystemEDIFlowDir', '/Sys/SystemEDIFlowDir', para, { width: 800, height: 600 });

    return true;
}

function SystemEDIFlowScheduleListButton_onClick(srcElement) {
    var para = '&QuerySysID=' + $('#QuerySysID').val() +
        '&ExecAction=' + _ActionTypeSelect;

    _openWin('SystemEDIFlowScheduleList', '/Sys/SystemEDIFlowScheduleList', para, { width: 800, height: 600 });

    return true;
}