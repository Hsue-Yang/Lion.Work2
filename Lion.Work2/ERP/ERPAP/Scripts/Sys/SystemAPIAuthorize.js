var _formElement;
var _clientSysID;

function SystemAPIAuthorizeForm_onLoad(formElement) {
    _formElement = formElement;

    $('table.tblsearch #ClientSysID', _formElement).combobox({
        width: 180,
        isRemoveButton: false
    });

    return true;
}

function LinkFunKey_onClick(srcElement, keys) {
    $.blockUI({ message: '' });

    _clientSysID = keys[1];

    _alert('dialog_Confirm');
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

function APIParaButton_onClick(srcElement) {
    var para = "SysID=" + $('#SysID').val() + "&"
        + "APIGroupID=" + $('#APIGroupID').val() + "&"
        + "APIFunID=" + $('#APIFunID').val();

    var objfeatures = { width: 450, height: 500 };

    _openWin("newwindow", "/Sys/SystemAPIPara", para, objfeatures);
    return false;
}

function CancelButton_onClick(srcElement) {
    $.blockUI({ message: '' });

    location.href = '/Sys/SystemAPI';
}

function ConfirmOKButton_onClick(srcElement) {
    $('#ClientSysID').val(_clientSysID);
    _clientSysID = '';

    $('#ExecAction').val(_ActionTypeDelete);
    _formElement.submit();
}

function ConfirmNOButton_onClick(srcElement) {
    _btnUnblockUI(this, false);
}