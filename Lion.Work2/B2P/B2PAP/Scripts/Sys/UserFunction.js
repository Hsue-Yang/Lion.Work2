var _formElement;
var _sysID;

function UserFunctionForm_onLoad(formElement) {
    _formElement = formElement;

    return true;
}

function SysID_onChange(srcElement) {
    var funControllerID = srcElement.id.split('.')[0] + '.FunControllerID';
    var objFunControllerID = $("select[name='" + funControllerID + "']");
    _sysID = $(srcElement).val();
    if ($(srcElement).val() != '') {
        $.ajax({
            url: '/Sys/GetSystemFunControllerIDList',
            type: 'POST',
            data: { sysID: _sysID },
            dataType: 'json',
            async: false,
            success: function (result) {
                if (result != null) {
                    objFunControllerID.find('option').remove();
                    for (var i = 0; i < result.length; i++) {
                        objFunControllerID.append("<option value='" + result[i].Value + "'>" + result[i].Text + "</option>");
                    }
                }
                else {
                    objFunControllerID.find('option').remove();
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                _AddJsErrMessage(JsMsg_GetSystemFunControllerIDList);
                _ShowJsErrMessageBox();
            }
        });

        var aa = $(srcElement).val();

    }
    else {
        objFunControllerID.find('option').remove();
    }
}

function FunControllerID_onChange(srcElement) {
    var funActionName = srcElement.id.split('.')[0] + '.FunActionName';
    var objFunActionName = $("select[name='" + funActionName + "']");

    if ($(srcElement).val() != '') {
        $.ajax({
            url: '/Sys/GetSystemFunActionNameList',
            type: 'POST',
            data: { sysID: _sysID, funControllerID: $(srcElement).val() },
            dataType: 'json',
            async: false,
            success: function (result) {
                if (result != null) {
                    objFunActionName.find('option').remove();
                    for (var i = 0; i < result.length; i++) {
                        objFunActionName.append("<option value='" + result[i].Value + "'>" + result[i].Text + "</option>");
                    }
                }
                else {
                    objFunActionName.find('option').remove();
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                _AddJsErrMessage(JsMsg_GetSystemFunActionNameList);
                _ShowJsErrMessageBox();
            }
        });

    }
    else {
        objFunActionName.find('option').remove();
    }
}

//----Tab----//
function UserRoleFunDetail_onClick(srcElement) {
    $(srcElement).attr('con', 'Sys');
    $(srcElement).attr('act', 'UserRoleFunDetail');
    $('#ExecAction').val(_ActionTypeUpdate);
    _submit($(srcElement));

    return false;
}

//----Button----//
function UpdateButton_onClick(srcElement) {
    var Result = _FormValidation();
    $('#ExecAction').val(_ActionTypeUpdate);
    if (Result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function CancelButton_onClick(srcElement) {
    $.blockUI({ message: '' });

    $('#ExecAction').val(_ActionTypeQuery);
    _formElement.submit();
}

function AddRowButton_onClick(srcElement) {
    _addTR("IsProcess");
}

function DeleteRowButton_onClick(srcElement) {
    var objComboBox = $("select[name*='.SysID']");
    var objTR = _GetParentElementByTag(srcElement, 'TR');

    if (objComboBox.length == 1) {
        $("select[name*='.SysID']").val('');
        $("select[name*='.FunControllerID']").val('');
        $("select[name*='.FunActionName']").val('');
    }
    else {
        $(objTR).find("input[id=IsProcess]").prop("checked", true);
    }

    _deleteTR("IsProcess");
}

function DeleteDataRowButton_onClick(srcElement) {
    var objTextbox = $("input[name*='.SysID']");
    var objTR = _GetParentElementByTag(srcElement, 'TR');

    $(objTR).find("input[id=IsProcess]").prop("checked", true);

    _deleteTR("IsProcess");
}