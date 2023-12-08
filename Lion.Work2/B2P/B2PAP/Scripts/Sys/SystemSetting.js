var _formElement;

function SystemSettingForm_onLoad(formElement) {
    _formElement = formElement;

    return true;
}

function DetailLinkFunKey_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#SysID').val(keys[1]);
    $('#SysNM').val(keys[2]);

    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}

function IPLinkFunKey_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#SysID').val(keys[1]);
    $('#SysNM').val(keys[2]);

    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}

function ServiceLinkFunKey_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#SysID').val(keys[1]);
    $('#SysNM').val(keys[2]);

    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}

function SubsystemLinkFunKey_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#SysID').val(keys[1]);
    $('#SysNM').val(keys[2]);

    $('#ExecAction').val(_ActionTypeSelect);
    return true;
}

function ListLinkFunKey_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#SysID').val(keys[1]);
    $('#SysNM').val(keys[2]);

    $('#ExecAction').val(_ActionTypeSelect);
    return true;
}

function LinkFunKey_onClick(srcElement, keys) {
    var objTR = $('tr[id="SubsystemList[' + keys[1] + ']"]');
    objTR.toggle();

    return false;
}

//----Button----//
function AddButton_onClick(srcElement) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#SysID').val($('#QuerySysID').val());

    $('#ExecAction').val(_ActionTypeAdd);
    return true;
}

//----Private Function----//
function Clean_HiddenValue() {
    $('#SysID').val('');
}