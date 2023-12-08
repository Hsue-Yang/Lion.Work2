var _formElement;

function ExternalSystemForm_onLoad(formElement) {
    _formElement = formElement;
    return true;
}

function SSOLink_onClick(srcElement, keys) {
    var systemID = keys[1];
    _openWin('externalnewwindow' + systemID, _enumERPAP + '/Pub/ExternalSystem', 'SystemID=' + systemID, null);
    return false;
}