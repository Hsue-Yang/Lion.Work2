var _formElement;

function SystemIPListForm_onLoad(formElement) {
    _formElement = formElement;

    return true;
}

function DeleteLinkFunKey_onClick(srcElement, keys) {
    $.blockUI({ message: '' });

    $('#IPAddress').attr('value', keys[1]);
    $('#IPAddress').attr('ReadOnly', 'true');
    $('#IPAddress').attr('style', 'background-color : #E7E8ED');

    _alert('dialog_Confirm');
}

function UpdateLinkFunKey_onClick(srcElement, keys) {
    $('#IPAddress').attr('value', keys[1]);
    $('#IPAddress').attr('ReadOnly', 'true');
    $('#IPAddress').attr('style', 'background-color : #E7E8ED');

    if (keys[2] == 'Y') {
        $('#IsAPServer').attr('checked', true);
    } else {
        $('#IsAPServer').attr('checked', false);
    }
                      
    if (keys[3] == 'Y') {
        $('#IsAPIServer').attr('checked', true);
    } else {
        $('#IsAPIServer').attr('checked', false);
    }

    if (keys[4] == 'Y') {
        $('#IsDBServer').attr('checked', true);
    } else {
        $('#IsDBServer').attr('checked', false);
    }

    if (keys[5] == 'Y') {
        $('#IsFileServer').attr('checked', true);
    } else {
        $('#IsFileServer').attr('checked', false);
    }

    $('#FolderPath').attr('value', keys[6]);
    
    $('#Remark').attr('value', keys[7]);

    $('#AddButton').attr('value', keys[8]);
    $('#AddButton').attr('name', 'SaveButton');
    $('#AddButton').attr('id', 'SaveButton');
}

function PageSize_onEnter(srcElement) {
    SelectButton_onClick();
    return true;
}

//----Button----//
function SelectButton_onClick(srcElement) {
    var Result = _FormValidation();
    _formElement.submit();
}

function AddButton_onClick(srcElement) {
    var Result = _FormValidation();
    $('#ExecAction').val(_ActionTypeAdd);
    if (Result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function SaveButton_onClick(srcElement) {
    var Result = _FormValidation();
    $('#ExecAction').val(_ActionTypeUpdate);
    if (Result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function CancelButton_onClick(srcElement) {
    $.blockUI({ message: '' });

    location.href = '/Sys/SystemSetting';
}

function ConfirmOKButton_onClick(srcElement) {
    $('#ExecAction').val(_ActionTypeDelete);
    _formElement.submit();
}

function ConfirmNOButton_onClick(srcElement) {
    _btnUnblockUI(this, false);
}