//create by Hello 2013.12.05
var _formElement;

function SystemEDIFlowErrorForm_onLoad(formElement) {
    _formElement = formElement;

    return true;
}

function CloseButton_onClick(srcElement) {
    _windowClose();
}