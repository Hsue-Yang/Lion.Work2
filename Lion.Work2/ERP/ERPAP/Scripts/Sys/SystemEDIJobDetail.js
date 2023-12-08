var _formElement;

function SystemEDIJobDetailForm_onLoad(formElement) {
    _formElement = formElement;

    $('#SysIDReadOnlyText').val($('#SysID').find("option:selected").text());
    $('#EDIFlowIDReadOnlyText').val($('#EDIFlowID').find("option:selected").text());
    $('#EDIJobIDReadOnlyText').val($('#EDIJobID').val());

    if ($('#EDIJobType').val() === 'CopyFile' || $('#EDIJobType').val() === 'CallAPI') {
        $('#Label_URLPath').show();
        $('#TextBox_URLPath').show();
    } 
    else {
        $('#Label_URLPath').hide();
        $('#TextBox_URLPath').hide();
    }

    if ($('#EDIJobType').val() === 'Import' || $('#EDIJobType').val() === 'Export' || $('#EDIJobType').val() === 'QueryExport') {
        $('#Label_IgnoreWarning').show();
        $('#TextBox_IgnoreWarning').show();
    } else {
        $('#IgnoreWarning').attr("checked", false);
        $('#Label_IgnoreWarning').hide();
        $('#TextBox_IgnoreWarning').hide();
    }

    $('#EDIFlowID', _formElement).combobox({
        width: 200,
        isRemoveButton: true,
        onChange: EDIFlowID_onChange
    });

    $('#EDIJobType', _formElement).combobox({
        width: 200,
        isRemoveButton: true,
        onChange: EDIJobType_onChange
    });

    $('#EDIConID, #FileEncoding, #DepEDIJobID', _formElement).combobox({
        width: 200,
        isRemoveButton: true
    });

    return true;
}

function EDIFlowID_onChange(event) {
    if (event.select.val() === '') {
        $('#EDIConID > option', _formElement).remove();
        $('#EDIConID', _formElement).combobox('SetSelected', '');
        return false;
    }

    $.ajax({
        url: '/Sys/GetSysSystemEDIConList',
        type: 'POST',
        data: { SysID: $('#SysID').val(), EDIFlowID: event.select.val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            $('#EDIConID > option', _formElement).remove();
            if (result != null) {
                $('#EDIConID > option', _formElement).remove();
                $('#EDIConID', _formElement).combobox('SetSelected', '');
                for (var i = 0; i < result.length; i++) {
                    $('#EDIConID', _formElement).append("<option value='" + result[i].Value + "'>" + result[i].Text + "</option>");
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetFunMenuList);
            _ShowJsErrMessageBox();
        }
    });
}

function EDIJobType_onChange(srcElement) {
    if ($('#EDIJobType').val() === 'Import' || $('#EDIJobType').val() === 'Export' || $('#EDIJobType').val() === 'QueryExport') {
        $('#Label_IgnoreWarning').show();
        $('#TextBox_IgnoreWarning').show();
    } else {
        $('#IgnoreWarning').attr("checked", false);
        $('#Label_IgnoreWarning').hide();
        $('#TextBox_IgnoreWarning').hide();
    }

    if ($('#EDIJobType').val() === 'CopyFile' || $('#EDIJobType').val() === 'CallAPI') {
        $('#Label_URLPath').show();
        $('#TextBox_URLPath').show();
    }
    else {
        $('#Label_URLPath').hide();
        $('#TextBox_URLPath').hide();
    }

    if ($('#EDIJobType').val() === 'Import') {
        $('#IsUseRes').prop('checked', true);
    }
    else {
        $('#IsUseRes').prop('checked', false);
    }
}

//----Button----//

function AddButton_onClick(srcElement) {
    var Result = _FormValidation();
    $('#ExecAction').val(_ActionTypeAdd);
    if (Result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function UpdateButton_onClick(srcElement) {
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