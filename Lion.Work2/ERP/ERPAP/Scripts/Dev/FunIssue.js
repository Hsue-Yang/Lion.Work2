var _formElement;

function FunIssueForm_onLoad(formElement) {
    _formElement = formElement;

    if ($(formElement).find('#FunIssueTable')[0] != null) {
        _TableHover('FunIssueTable', formElement);
    }

    return true;
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

function CloseButton_onClick(srcElement) {
    _windowClose();
}