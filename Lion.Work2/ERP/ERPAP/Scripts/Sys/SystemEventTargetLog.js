var _formElement;

function SystemEventTargetLogForm_onLoad(formElement) {
    _formElement = formElement;

    return true;
}

function CloseButton_onClick(srcElement) {
    _windowClose();
}