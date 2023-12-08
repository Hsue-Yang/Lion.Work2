var _formElement;

function SystemFunToolSettingForm_onLoad(formElement) {
    _formElement = formElement;
    var table = $('#SystemFunToolSettingTable', _formElement);

    if (table.length > 0) {
        var width = $(window).width();
        table.show();
        var height = $(window).height() - table.offset().top;
        $('table[id=Pager]').each(function () { height -= $(this).height(); });
        table.freezePanes({ width: width + 'px', height: height + 'px', fixedCols: 0 });
    }

    $('.BaseContainer').css('z-index', 0);

    $('table.tblsearch #SysID', _formElement).combobox({
        width: 180,
        isRemoveButton: false,
        onChange: SysID_onChange
    });

    $('table.tblsearch #FunControllerID', _formElement).combobox({
        width: 180,
        isRemoveButton: false,
        onChange: FunControllerID_onChange
    });

    $('table.tblsearch #FunActionName', _formElement).combobox({
        width: 180,
        isRemoveButton: false
    });

    $('table.tblsearch #FunControllerIDSearch', _formElement).combobox({
        width: 180,
        isRemoveButton: false,
        onChange: FunControllerIDSearch_onChange
    });

    $('table.tblsearch #FunActionNameSearch', _formElement).combobox({
        width: 180,
        isRemoveButton: false
    });
    
    AdvancedSearch_OnOff();

    return true;
}

function SysID_onChange(event) {

    if ($('#IsAdSearch').val() === 'N') {
        if (event.select.val() === '') {
            $('#FunControllerID > option', _formElement).remove();
            $('#FunActionName > option', _formElement).remove();
            $('#FunControllerID', _formElement).combobox('SetSelected', '');
            $('#FunActionName', _formElement).combobox('SetSelected', '');
            return false;
        }

        $.ajax({
            url: '/Sys/GetSystemFunToolControllerIDList',
            type: 'POST',
            data: { sysID: event.select.val() },
            dataType: 'json',
            async: false,
            success: function (result) {
                if (result != null) {
                    $('#FunControllerID > option', _formElement).remove();
                    $('#FunControllerID', _formElement).combobox('SetSelected', '');
                    for (var i = 0; i < result.length; i++) {
                        $('#FunControllerID', _formElement).append('<option value="' + result[i].Value + '">' + result[i].Text + '</option>');
                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                _AddJsErrMessage(JsMsg_GetSystemFunControllerIDList);
                _ShowJsErrMessageBox();
            }
        });

        FunControllerID_onChange();
    } else {

        if (event.select.val() === '') {
            $('#FunControllerIDSearch > option', _formElement).remove();
            $('#FunActionNameSearch > option', _formElement).remove();
            $('#FunControllerIDSearch', _formElement).combobox('SetSelected', '');
            $('#FunActionNameSearch', _formElement).combobox('SetSelected', '');
            return false;
        }

        $.ajax({
            url: '/Sys/GetSearchSystemFunToolControllerIDList',
            type: 'POST',
            data: { sysID: event.select.val(), condition: $('#SysCondition').val() },
            dataType: 'json',
            async: false,
            success: function (result) {
                if (result != null) {
                    $('#FunControllerIDSearch > option', _formElement).remove();
                    $('#FunControllerIDSearch', _formElement).combobox('SetSelected', '');
                    for (var i = 0; i < result.length; i++) {
                        $('#FunControllerIDSearch', _formElement).append('<option value="' + result[i].Value + '">' + result[i].Text + '</option>');
                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                _AddJsErrMessage(JsMsg_GetSystemFunControllerIDList);
                _ShowJsErrMessageBox();
            }
        });

        FunControllerIDSearch_onChange();
    }
}

function FunControllerID_onChange(event) {
    if (event == undefined) {
        $('#FunActionName > option', _formElement).remove();
        $('#FunActionName', _formElement).combobox('SetSelected', '');
        return false;
    }

    $.ajax({
        url: '/Sys/GetSystemFunToolFunNameList',
        type: 'POST',
        data: { sysID: $('#SysID').val(), funControllerID: event.select.val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                $('#FunActionName > option', _formElement).remove();
                $('#FunActionName', _formElement).combobox('SetSelected', '');
                for (var i = 0; i < result.length; i++) {
                    $('#FunActionName', _formElement).append('<option value="' + result[i].Value + '">' + result[i].Text + '</option>');
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetSystemFunNameList);
            _ShowJsErrMessageBox();
        }
    });
}

function FunControllerIDSearch_onChange(event) {
    if (event == undefined) {
        $('#FunActionNameSearch > option', _formElement).remove();
        $('#FunActionNameSearch', _formElement).combobox('SetSelected', '');
        return false;
    }

    $.ajax({
        url: '/Sys/GetSearchSystemFunToolFunNameList',
        type: 'POST',
        data: { sysID: $('#SysID').val(), funControllerID: event.select.val(), condition: $('#SysCondition').val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                $('#FunActionNameSearch > option', _formElement).remove();
                $('#FunActionNameSearch', _formElement).combobox('SetSelected', '');
                for (var i = 0; i < result.length; i++) {
                    $('#FunActionNameSearch', _formElement).append('<option value="' + result[i].Value + '">' + result[i].Text + '</option>');
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetSystemFunNameList);
            _ShowJsErrMessageBox();
        }
    });
}

function SysCondition_onBlur(srcElement) {
    $('#FunControllerIDSearch > option', _formElement).remove();
    $('#FunActionNameSearch > option', _formElement).remove();

    $.ajax({
        url: '/Sys/GetSearchSystemFunToolControllerIDList',
        type: 'POST',
        data: { sysID: $('#SysID').val(), condition: $(srcElement).val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                $('#FunControllerIDSearch > option', _formElement).remove();
                for (var i = 0; i < result.length; i++) {
                    $('#FunControllerIDSearch', _formElement).append('<option value="' + result[i].Value + '">' + result[i].Text + '</option>');
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetSystemFunControllerIDList);
            _ShowJsErrMessageBox();
        }
    });
}

//----Button----//
function SelectButton_onClick(srcElement) {
    var result = _FormValidation();
    $('#ExecAction').val(_ActionTypeSelect);

    if (result) {
        if (ValidButton()) {
            $.blockUI({ message: '' });
            _formElement.submit();
            return true;
        }
        else {
            _ShowJsErrMessageBox();
            return false;
        }
    } else {
        return false;
    }
}

function UpdateButton_onClick(srcElement) {
    var result = _FormValidation();
    
    var dataCount = 10;
    if ($('[name*="PickData"]:checked').length > dataCount) {
        result = false;
        _AddJsErrMessage(String.format(JsMsg_ExceedDataCountLimit, dataCount));
        _ShowJsErrMessageBox();
    }

    if (result) {
        $.blockUI({ message: '' });
        $('#ExecAction').val(_ActionTypeUpdate);
        _formElement.submit();
        return true;
    }
}

function DeleteButton_onClick(srcElement) {
    var result = _FormValidation();

    var dataCount = 10;
    if ($('[name*="PickData"]:checked').length > dataCount) {
        result = false;
        _AddJsErrMessage(String.format(JsMsg_ExceedDataCountLimit, dataCount));
        _ShowJsErrMessageBox();
    }

    if ($('[name*="PickData"]:checked').length === 0) {
        result = false;
        _AddJsErrMessage(JsMsg_AtLeastCheckedOne);
        _ShowJsErrMessageBox();
    }

    if (result) {
        $.blockUI({ message: '' });
        $('#ExecAction').val(_ActionTypeDelete);
        _formElement.submit();
        return true;
    }
}

function CopyButton_onClick(srcElement) {
    if ($('input[name$="PickData"]:checked')[0] != null || $('input[name$="PickData"]:checked')[0] != undefined) {
        if ($('input[name$="PickData"]:checked').length === 1) {
            $.blockUI({ message: $('#SysSystemFunToolSettingCopyConfirmDialog') });

            _NoFormAutoCompleteTextBox_GetList('CopyToUserID', '_BaseAP', 'GetBaseRAWUserList');
        } else {
            _AddJsErrMessage(JsMsg_AtMostCheckedOneCopy);
            _ShowJsErrMessageBox();
            return false;
        }
    } else {
        _AddJsErrMessage(JsMsg_CheckedOneCopy);
        _ShowJsErrMessageBox();
        return false;
    }
}

function SysSystemFunToolSettingCopyConfirmOKButton_onClick(srcElement) {
    var result = _FormValidation();

    if (result) {
        $.blockUI({ message: '' });
        $('#ExecAction').val(_ActionTypeCopy);
        _formElement.submit();
        return true;
    }
}

function SysSystemFunToolSettingCopyConfirmNOButton_onClick(srcElement) {
    _btnUnblockUI(this, false);
}

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

function LinkFunKeyAdvancedSearchOn_onClick(srcElement, keys) {

    LinkFunKeyAdvancedSearchOn();

    $('#IsAdSearch').val(keys[1]);

    if ($('#SwitchSearch:visible').length !== 0) {
        $.ajax({
            url: '/Sys/GetSearchSystemFunToolControllerIDList',
            type: 'POST',
            data: { sysID: $('#SysID').val(), condition: $('#SysCondition').val() },
            dataType: 'json',
            async: false,
            success: function (result) {
                if (result != null) {
                    for (var i = 0; i < result.length; i++) {
                        $('#FunControllerIDSearch', _formElement).append('<option value="' + result[i].Value + '">' + result[i].Text + '</option>');
                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                _AddJsErrMessage(JsMsg_GetSystemFunControllerIDList);
                _ShowJsErrMessageBox();
            }
        });
    }

    return false;
}

function LinkFunKeyAdvancedSearchOff_onClick(srcElement, keys) {

    LinkFunKeyAdvancedSearchOff();

    $('#IsAdSearch').val(keys[1]);

    $.ajax({
        url: '/Sys/GetSystemFunToolControllerIDList',
        type: 'POST',
        data: { sysID: $('#SysID').val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                for (var i = 0; i < result.length; i++) {
                    $('#FunControllerID', _formElement).append('<option value="' + result[i].Value + '">' + result[i].Text + '</option>');
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetSystemFunControllerIDList);
            _ShowJsErrMessageBox();
        }
    });

    return false;
}

function LinkFunKeySystemFunToolPara_onClick(srcElement, keys) {
    $('#ExecAction').val(_ActionTypeSelect);

    $('#UserID').val(keys[1]);
    $('#SysID').val(keys[2]);
    $('#FunControllerID').val(keys[3]);
    $('#FunActionName').val(keys[4]);
    $('#ToolNo').val(keys[5]);

    var para = 'UserID=' + $('#UserID').val() + '&'
             + 'SysID=' + $('#SysID').val() + '&'
             + 'FunControllerID=' + $('#FunControllerID').val() + '&'
             + 'FunActionName=' + $('#FunActionName').val() + '&'
             + 'ToolNo=' + $('#ToolNo').val() + '&'
             + 'ExecAction=' + _ActionTypeSelect; ;

    _openWin('newwindow', '/Sys/SystemFunToolPara', para);

    return false;
}

//-----Valid-----//
function ValidButton() {

    if ($('#IsAdSearch').val() === 'N') {
        if ($('#FunControllerID').val() == undefined || $('#FunControllerID').val() == '') {
            _AddJsErrMessage(JsMsg_MustFunControllerID);
            return false;
        }
        if ($('#FunActionName').val() == undefined || $('#FunActionName').val() == '') {
            _AddJsErrMessage(JsMsg_MustFunActionName);
            return false;
        }
    } else {
        if ($('#FunControllerIDSearch').val() == undefined || $('#FunControllerIDSearch').val() == '') {
            _AddJsErrMessage(JsMsg_MustFunControllerID);
            return false;
        }
        if ($('#FunActionNameSearch').val() == undefined || $('#FunActionNameSearch').val() == '') {
            _AddJsErrMessage(JsMsg_MustFunActionName);
            return false;
        }
    }
    return true;
}

//-----Private Function-----//
function AdvancedSearch_OnOff() {
    $('#AdvancedSearchOn').show();
    $('#AdvancedSearchOff').hide();
    $('#SwitchSearch').hide();
}

function LinkFunKeyAdvancedSearchOn() {
    $('#SwitchSearchFunControllerID_th').hide();
    $('#SwitchSearchFunControllerID_td').hide();
    $('#SwitchSearchFunActionName_th').hide();
    $('#SwitchSearchFunActionName_td').hide();
    $('#SwithcWidth').hide();

    $('#SwitchSearch').show();

    $('#AdvancedSearchOff').show();
    $('#AdvancedSearchOn').hide();

    $('#FunControllerID > option', _formElement).remove();
    $('#FunActionName > option', _formElement).remove();
    $('#FunControllerIDSearch > option', _formElement).remove();
    $('#FunActionNameSearch > option', _formElement).remove();
}

function LinkFunKeyAdvancedSearchOff() {
    $('#SwitchSearchFunControllerID_th').show();
    $('#SwitchSearchFunControllerID_td').show();
    $('#SwitchSearchFunActionName_th').show();
    $('#SwitchSearchFunActionName_td').show();
    $('#SwithcWidth').show();

    $('#SwitchSearch').hide();

    $('#AdvancedSearchOff').hide();
    $('#AdvancedSearchOn').show();

    $('#FunControllerID > option', _formElement).remove();
    $('#FunActionName > option', _formElement).remove();
    $('#FunControllerIDSearch > option', _formElement).remove();
    $('#FunActionNameSearch > option', _formElement).remove();
    $('#SysCondition').val('');
}