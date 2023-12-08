var _formElement;

function SystemIPListForm_onLoad(formElement) {
    _formElement = formElement;

    $('#IPAddress').removeAttr('isrequired');
    $('#ServerNM').removeAttr('isrequired');

    return true;
}

function IPAddress_onBlur(srcElement) {
    var result = false;
    var IPAddress = $('#IPAddress').val();
    var UpdateButton = $('#UpdateButton').val();
    var AddButton = $('#AddButtonHidden').val();

    if (IPAddress != null && IPAddress != "") {
        $('input[name=IPAddressTable]').each(function () {
            if (this.value == IPAddress) {
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

function DeleteLinkFunKey_onClick(srcElement, keys) {
    $.blockUI({ message: '' });

    $('#IPAddress').attr('value', keys[1]);
    $('#SubSysID').attr('value', keys[2]);

    _alert('dialog_Confirm');
}

function UpdateLinkFunKey_onClick(srcElement, keys) {
    $('#IPAddress').attr('value', keys[1]);

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

    if (keys[10] == 'Y') {
        $('#IsOutsourcing').attr('checked', true);
    } else {
        $('#IsOutsourcing').attr('checked', false);
    }

    $('#FolderPath').attr('value', keys[6]);

    $('#Remark').attr('value', keys[7]);

    $('#AddButton.btn').attr('value', keys[8]);
    $('#AddButton.btn').attr('name', 'SaveButton');
    $('#AddButton.btn').attr('id', 'SaveButton');

    $('#ServerNM').attr('value', keys[9]);

    $('#SubSysID').attr('value', keys[11]);
}

function PageSize_onEnter(srcElement) {
    $('#PageIndex').val(1);
    _formElement.submit();
}

//----Button----//

function AddButton_onClick() {
    AddAttrRequired();
    var Result = _FormValidation();

    if (Result) {
        if (ValidButton()) {
            $.blockUI({ message: '' });
            $('#ExecAction').val(_ActionTypeAdd);
            return true;
        } else {
            _ShowJsErrMessageBox();
            return false;
        }
    } else {
        return false;
    }
}

function SaveButton_onClick() {
    AddAttrRequired();
    var Result = _FormValidation();

    if (Result) {
        if (ValidButton()) {
            $.blockUI({ message: '' });
            $('#ExecAction').val(_ActionTypeUpdate);
            _formElement.submit();
            return true;
        } else {
            _ShowJsErrMessageBox();
            return false;
        }
    } else {
        return false;
    }
}

function UpdateButton_onClick(srcElement) {
    _alert('dialog_ConfirmUpdate');
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

//-----Privte-----//
function AddAttrRequired() {
    $('#IPAddress').attr('isrequired', 'isrequired');
    $('#ServerNM').attr('isrequired', 'isrequired');
}

//-----Valid----//
function ValidButton(srcElement) {
    var result = false;
    var resultFile = false;
    var ServerNM = $('#ServerNM').val();
    var regExp = /^[\d|a-zA-Z|\-.]+$/;

    if (regExp.test(ServerNM)) {
        result = true;
    } else {
        result = false;
    }

    if ($('#IsFileServer').attr('checked') == 'checked') {
        $('table[id*=SystemIPListTable] tr').each(function () {
            if ($('#SubSysID').val() == "" || $('#SubSysID').val() == null) {
                if ($(this).find("input[id*='SysComp']").val() == $('#SysID').val() && $(this).find("input[id*='SubComp']").val() == $('#SubComp').val() && $(this).find("input[id*='FileComp']").val() == $('#IsFileServer').val()) {
                    resultFile = true;
                }
            } else {
                if ($(this).find("input[id*='SysComp']").val() == $('#SysID').val() && $(this).find("input[id*='SubComp']").val() == $('#SubSysID').val() && $(this).find("input[id*='FileComp']").val() == $('#IsFileServer').val()) {
                    resultFile = true;
                }
            }
        });
    }

    if (result == false) {
        _AddJsErrMessage(JsMsg_OnlyAllowEnglish);
        return false;
    } else {
        if (resultFile == true) {
            _AddJsErrMessage(JsMsg_SystemSubSystemFileServerSame);
            return false;
        } else {
            return true;
        }
    }
}