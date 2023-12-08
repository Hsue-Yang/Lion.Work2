var _formElement;

function DomainGroupUserForm_onLoad(formElement) {
    _formElement = formElement;
    return true;
}

function CancelButton_onClick(srcElement) {
    $.blockUI({ message: '' });

    location.href = '/Sys/DomainGroup';
}