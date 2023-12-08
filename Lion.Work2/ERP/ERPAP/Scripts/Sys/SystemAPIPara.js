var _formElement;

function SystemAPIParaForm_onLoad(formElement) {
    _formElement = formElement;
    $('#APIReturnReadOnlyText').val($('#APIReturn').find("option:selected").text());

    return true;
}

//----Button----//
function CloseButton_onClick(srcElement) {
    _windowClose();
}