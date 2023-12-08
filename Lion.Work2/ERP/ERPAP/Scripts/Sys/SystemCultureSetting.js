var _formElement;

function SystemCultureSettingForm_onLoad(formElement) {
    _formElement = formElement;

    $('.BaseContainer').css('z-index', 0);

    var cultureWidth = $('table.tblsearch #QueryCultureID', _formElement).width();
    $('table.tblsearch #QueryCultureID', _formElement).combobox({
        width: cultureWidth,
        isRemoveButton: true
    });

    return true;
}

function LinkFunKeyDetail_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();
    $('#CultureID').val(keys[1]);
    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}

function PageSize_onEnter(srcElement) {
    SelectButton_onClick();
    return true;
}

//----Button----//
function SelectButton_onClick(srcElement) {
    var result = _FormValidation();
    $('#ExecAction').val(_ActionTypeSelect);
    if (result) {
        $.blockUI({ message: '' });
        $('#PageIndex').val(1);
        _formElement.submit();
    }
}

function AddButton_onClick(srcElement) {
    var result = _FormValidation();
    if (result) {
        $.blockUI({ message: '' });
        Clean_HiddenValue();

        $('#ExecAction').val(_ActionTypeAdd);
        return true;
    }
}

function ReleaseButton_onClick(srcElement) {
    var result = _FormValidation();
    if (result) {
        $.blockUI({ message: '' });

        $('#ExecAction').val(_ActionTypeUpdate);
        return true;
    }
    return false;
}

//----Private Function----//
function Clean_HiddenValue() {
    $('#CultureID').val('');
}