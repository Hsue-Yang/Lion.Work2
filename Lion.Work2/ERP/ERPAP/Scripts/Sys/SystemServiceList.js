var _formElement;

function SystemServiceListForm_onLoad(formElement) {
    _formElement = formElement;

    if ($(formElement).find('#SystemServiceListTable')[0] != null) {
        _TableHover('SystemServiceListTable', formElement);
    }
    return true;
}

function ServiceID_onChange(srcElement) {
    var result = false;
    var ServiceID = srcElement.value;
    var UpdateButton = $('#UpdateButton').val();
    var AddButton = $('#AddButtonHidden').val();

    if (ServiceID != null && ServiceID != "") {
        $('input[name=ServiceID]').each(function () {
            if ($(this).val() == ServiceID) {
                result = true;
            }
        });
    }

    if (result == true) {
        $('#AddButton.btn').attr('value', UpdateButton);
        $('#AddButton.btn').attr('name', 'UpdateButton');
        $('#AddButton.btn').attr('id', 'UpdateButton');

        $('#SaveButton.btn').attr('value', UpdateButton);
        $('#SaveButton.btn').attr('name', 'UpdateButton');
        $('#SaveButton.btn').attr('id', 'UpdateButton');
    } else {
        $('#UpdateButton.btn').attr('value', AddButton);
        $('#UpdateButton.btn').attr('name', 'AddButton');
        $('#UpdateButton.btn').attr('id', 'AddButton');

        $('#SaveButton.btn').attr('value', AddButton);
        $('#SaveButton.btn').attr('name', 'AddButton');
        $('#SaveButton.btn').attr('id', 'AddButton');
    }
}

function UpdataLinkFunKey_onClick(srcElement, keys) {
    $('#ServiceID').attr('value', keys[1]);
    $('#Remark').attr('value', keys[2]);
    $('#SubSysID').attr('value', keys[4]);

    $('#UpdateButton.btn').attr('value', keys[3]);
    $('#UpdateButton.btn').attr('name', 'SaveButton');
    $('#UpdateButton.btn').attr('id', 'SaveButton');

    $('#AddButton.btn').attr('value', keys[3]);
    $('#AddButton.btn').attr('name', 'SaveButton');
    $('#AddButton.btn').attr('id', 'SaveButton');
}

function DeleteLinkFunKey_onClick(srcElement, keys) {
    $.blockUI({ message: '' });

    $('#ServiceID').attr('value', keys[1]);
    $('#SubSysID').attr('value', keys[2]);

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

function SaveButton_onClick(srcElement) {
    var Result = _FormValidation();
    $('#ExecAction').val(_ActionTypeUpdate);
    if (Result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function UpdateButton_onClick(srcElement) {
    _alert('dialog_CofirmUpdate');
}

function CancelButton_onClick(srcElement) {
    _windowClose();
}

function ConfirmOKButton_onClick(srcElement) {
    $('#ExecAction').val(_ActionTypeDelete);
    _formElement.submit();
}

function ConfirmOKUpdateButton_onClick(srcElement) {
    $('#ExecAction').val(_ActionTypeUpdate);
    _formElement.submit();
}

function ConfirmNOButton_onClick(srcElement) {
    _btnUnblockUI(this, false);
}