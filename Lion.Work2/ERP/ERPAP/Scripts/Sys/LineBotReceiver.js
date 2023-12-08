var _formElement;

function LineBotReceiverForm_onLoad(formElement) {
    _formElement = formElement;
}

function DetailLinkFunKey_onClick(srcElement, key) {
    $.blockUI({ message: '' });
    $('#SysID').val(key[1]);
    $('#LineID').val(key[2]);
    $('#ReceiverID').val(key[3]);
    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}