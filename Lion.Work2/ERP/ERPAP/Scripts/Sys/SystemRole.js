var _formElement;

function SystemRoleForm_onLoad(formElement) {
    _formElement = formElement;
    var table = $('#SystemRoleTable', _formElement);

    if (table.length > 0) {
        table.hide();
        var width = $(window).width();
        table.show();
        var height = $(window).height() - table.offset().top;
        $('.tblsearch').each(function () { height -= $(this).height(); });
        table.freezePanes({ width: width + 'px', height: height + 'px', fixedCols: 0 });
    }

    $('.BaseContainer').css('z-index', 0);

    $('table.tblsearch #QuerySysID', _formElement).combobox({
        width: 180,
        isRemoveButton: false,
        onChange: QuerySysID_onChange
    });

    $('#QueryRoleCategoryID, #RoleCategoryID', $('table.tblsearch', _formElement)).combobox({
        width: 180,
        isRemoveButton: false
    });

    return true;
}

function QuerySysID_onChange(event) {
    if (event.select.val() === '') {
        $('#QueryRoleCategoryID > option', _formElement).remove();
        $('#RoleCategoryID > option', _formElement).remove();
        $('#QueryRoleCategoryID', _formElement).combobox('SetSelected', '');
        $('#RoleCategoryID', _formElement).combobox('SetSelected', '');

        return false;
    }

    $.ajax({
        url: '/Sys/GetSystemRoleCategoryIDList',
        type: 'POST',
        data: { sysID: event.select.val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                $('#QueryRoleCategoryID > option', _formElement).remove();
                $('#QueryRoleCategoryID', _formElement).combobox('SetSelected', '');
                for (var i = 0; i < result.length; i++) {
                    $('#QueryRoleCategoryID', _formElement).append('<option value="' + result[i].Value + '">' + result[i].Text + '</option>');
                }

                $('#RoleCategoryID > option', _formElement).remove();
                $('#RoleCategoryID', _formElement).combobox('SetSelected', '');
                for (var i = 0; i < result.length; i++) {
                    $('#RoleCategoryID', _formElement).append('<option value="' + result[i].Value + '">' + result[i].Text + '</option>');
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_UnGetSystemRoleCategoryByIdList);
            _ShowJsErrMessageBox();
        }
    });
}

function DetailLinkFunKey_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#SysID').val(keys[1]);
    $('#RoleID').val(keys[2]);

    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}

function ListLinkFunKey_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#SysID').val(keys[1]);
    $('#SysNM').val(keys[2]);
    $('#RoleID').val(keys[3]);
    $('#RoleNM').val(keys[4]);

    $('#ExecAction').val(_ActionTypeSelect);
    return true;
}

function ListLinkRoleFunKey_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#SysID').val(keys[1]);
    $('#SysNM').val(keys[2]);
    $('#RoleID').val(keys[3]);
    $('#RoleNM').val(keys[4]);

    $('#ExecAction').val(_ActionTypeSelect);
    return true;
}

function ListLinkRoleElmKey_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#SysID').val(keys[1]);
    $('#SysNM').val(keys[2]);
    $('#RoleID').val(keys[3]);
    $('#RoleNM').val(keys[4]);

    $('#ExecAction').val(_ActionTypeSelect);
    return true;
}

function PageSize_onEnter(srcElement) {
    SelectButton_onClick();
    return true;
}

//----Button----//
function SelectButton_onClick(srcElement) {
    var Result = _FormValidation();
    $('#ExecAction').val(_ActionTypeSelect);
    if (Result) {
        $.blockUI({ message: '' });
        $('#PageIndex').val(1);
        _formElement.submit();
    }
}

function EditButton_onClick(srcElement) {
    var para = 'SysID=' + $('#QuerySysID').val() + '&ExecAction=' + _ActionTypeSelect;
    var objfeatures = { width: window.screen.availWidth, height: window.screen.availWidth, top: 0, left: 0 };

    _openWin('newwindow', '/Sys/SystemRoleCategory', para, objfeatures);
}

function AddButton_onClick(srcElement) {
    var Result = _FormValidation();
    if (Result) {
        $.blockUI({ message: '' });
        Clean_HiddenValue();

        $('#SysID').val($('#QuerySysID').val());
        $('#RoleID').val($('#QueryRoleID').val());

        $('#ExecAction').val(_ActionTypeAdd);
        return true;
    }
}

function SaveButton_onClick(srcElement) {
    var Result = _FormValidation();

    var dataCount = 10;
    if ($('[name*="PickList"]:checked').length > dataCount) {
        Result = false;
        _AddJsErrMessage(String.format(JsMsg_ExceedDataCountLimit, dataCount));
        _ShowJsErrMessageBox();
    }

    if (Result) {
        $.blockUI({ message: '' });

        $('#ExecAction').val(_ActionTypeUpdate);
        return true;
    }
}

//----Private Function----//
function Clean_HiddenValue() {
    $('#SysID').val('');
    $('#SysNM').val('');
    $('#RoleID').val('');
    $('#RoleNM').val('');
}

function SystemRoleGroupJoinLink_onClick(srcElement) {

    var objfeatures = { width: 800, height: 600, top: 0, left: 0 };
    _openWin('newwindow', '/Sys/SystemRoleGroupJoin', '', objfeatures);
}