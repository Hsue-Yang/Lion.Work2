var _formElement;

function SystemFunDetailForm_onLoad(formElement) {
    _formElement = formElement;

    $('#SysIDReadOnlyText').val($('#SysID').find("option:selected").text());
    $('#FunControllerIDReadOnlyText').val($('#FunControllerID').find("option:selected").text());

    return true;
}

function SystemFunAssign_onClick(srcElement) {
    $(srcElement).attr('con', 'Sys');
    $(srcElement).attr('act', 'SystemFunAssign');
    $('#ExecAction').val(_ActionTypeUpdate);
    _submit($(srcElement));

    return false;
}

function SysID_onChange(srcElement) {
    if ($(srcElement).val() != '') {
        $.ajax({
            url: '/Sys/GetSystemFunControllerIDList',
            type: 'POST',
            data: { sysID: $(srcElement).val() },
            dataType: 'json',
            async: false,
            success: function (result) {
                if (result != null) {
                    $('#FunControllerID > option', _formElement).remove();
                    for (var i = 0; i < result.length; i++) {
                        $('#FunControllerID', _formElement).append("<option value='" + result[i].Value + "'>" + result[i].Text + "</option>");
                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                _AddJsErrMessage(JsMsg_GetSystemFunControllerIDList);
                _ShowJsErrMessageBox();
            }
        });
    }
    else {
        $('#FunControllerID > option', _formElement).remove();
    }
}

function FunMenuSysID_onChange(srcElement) {
    var funMenu = srcElement.id.split('.')[0] + '.FunMenu';
    var objFunMenu = $("select[name='" + funMenu + "']");

    if ($(srcElement).val() != '') {
        $.ajax({
            url: '/Sys/GetSystemFunMenuList',
            type: 'POST',
            data: { sysID: $(srcElement).val() },
            dataType: 'json',
            async: false,
            success: function (result) {
                if (result != null) {
                    objFunMenu.find('option').remove();
                    for (var i = 0; i < result.length; i++) {
                        objFunMenu.append("<option value='" + result[i].Value + "'>" + result[i].Text + "</option>");
                    }
                }
                else {
                    objFunMenu.find('option').remove();
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                _AddJsErrMessage(JsMsg_GetSystemFunMenuList);
                _ShowJsErrMessageBox();
            }
        });
    }
    else {
        objFunMenu.find('option').remove();
    }
}

//----Button----//
function AddButton_onClick(srcElement) {
    SetValidate();
    var Result = _FormValidation();
    $('#ExecAction').val(_ActionTypeAdd);
    if (Result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function UpdateButton_onClick(srcElement) {
    SetValidate();
    var Result = _FormValidation();
    $('#ExecAction').val(_ActionTypeUpdate);
    if (Result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function DeleteButton_onClick(srcElement) {
    $.blockUI({ message: '' });

    _alert('dialog_Confirm');
}

function CancelButton_onClick(srcElement) {
    $.blockUI({ message: '' });
    
    $('#ExecAction').val(_ActionTypeSelect);
    _formElement.submit();
}

function ConfirmOKButton_onClick(srcElement) {
    var Result = _FormValidation();

    $('#ExecAction').val(_ActionTypeDelete);
    if (Result) {
        _formElement.submit();
    }

    _btnUnblockUI(this, false);
}

function ConfirmNOButton_onClick(srcElement) {
    _btnUnblockUI(this, false);
}

function AddRowButton_onClick(srcElement) {
    _addTR("IsProcess");

    var rows = $("[name*='IsProcess']");
    var obj = rows.last();
    var tr = obj.closest("tr");

    //新增行時清空值
    tr.find("input:text,textarea,select").each(function (idx, item) {
        if (item.tagName == 'SELECT') {
            if (item.id.split('.')[1] == 'FunMenu') {
                $(item).find('option').remove();
            }
        }
    });

    //設定input的title
    _setTitle();

    //設定title
    $("[title]").tooltip();
}

function DeleteRowButton_onClick(srcElement) {
    var objDropdown = $("select[name*='.FunMenuSysID']");
    var objTR = _GetParentElementByTag(srcElement, 'TR');

    if (objDropdown.length == 1) {
        $(objTR).find("input:text,textarea,select").each(function (idx, item) {
            if (item.tagName == 'SELECT') {
                if (item.id.split('.')[1] == 'FunMenuSysID') {
                    $(item).val('');
                }

                if (item.id.split('.')[1] == 'FunMenu') {
                    $(item).find('option').remove();
                }
            }
        });
    }
    else {
        $(objTR).find("input[id=IsProcess]").prop("checked", true);
    }

    _deleteTR("IsProcess");
}

//----Private Function----//
function SetValidate() {
    if ($('input:checkbox[id=HasRole][checked=checked]').length == 0) {
        _AddJsErrMessage(JsMsg_ChooseOneRoleID);
    }
}
