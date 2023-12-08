var _formElement;

function SystemSettingDetailForm_onLoad(formElement) {
    _formElement = formElement;

    //For Icon Upload
    $(_formElement).attr("enctype", "multipart/form-data");

    $('#SysIDReadOnlyText').val($('#SysID').val());

    return true;
}

function IsOutsourcingCheckBox_onClick(srcElement) {
    if (srcElement.checked == true) {
        $('#IsOutsourcing').val('Y');
    }
    else {
        $('#IsOutsourcing').val('N');
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

function Help03Button_onClick(srcElement) {
    vMapFields = new Array(3);
    vMapFields[1] = "UserID";
    _hISearch(vMapFields, "newwindow", _enumPUBAP + "/Help/Ishlp03", "Name=" + encodeURI($.trim($("#UserID").val())));
}