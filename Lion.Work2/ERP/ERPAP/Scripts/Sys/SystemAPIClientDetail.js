var _formElement;

function SystemAPIClientDetailForm_onLoad(formElement) {
    _formElement = formElement;


    return true;
}

function CloseButton_onClick(srcElement) {
    _windowClose();
}

//json format event
$(document).ready(function () {
    $('p').beautifyJSON();
});