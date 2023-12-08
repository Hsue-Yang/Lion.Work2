var _formElement;

function LineBotAccountSettingDetailForm_onLoad(formElement) {
    _formElement = formElement;
}

function CancelButton_onClick(parameters) {
    $('#ExecAction').val(_ActionTypeSelect);
    _formElement.submit();
}

function AddButton_onClick(parameters) {
    $('#ExecAction').val(_ActionTypeAdd);
    return true;
}

function UpdateButton_onClick(parameters) {
    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}

function DeleteButton_onClick(parameters) {
    $('#ExecAction').val(_ActionTypeDelete);
    return true;
}