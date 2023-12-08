var _formElement;

function SystemAPIGroupForm_onLoad(formElement) {
    _formElement = formElement;

    var table = $('#SystemAPIGroupTable', _formElement);

    if (table.length > 0) {
        table.hide();
        var width = $(window).width();
        table.show();
        var height = $(window).height() - table.offset().top;
        $('.tblsearch').each(function () { height -= $(this).height(); });
        table.freezePanes({ width: width + 'px', height: height + 'px', fixedCols: 0 });
    }

    $('.BaseContainer').css('z-index', 0);

    var sysWidth = $('#QuerySysID', _formElement).width();
    $('table.tblsearch #QuerySysID', _formElement).combobox({
        width: sysWidth,
        isRemoveButton: false
    });

    return true;
}

function LinkFunKey_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#SysID').val(keys[1]);
    $('#APIGroupID').val(keys[2]);

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
    return false;
}

function AddButton_onClick(srcElement) {
    var result = _FormValidation();
    if (result) {
        $.blockUI({ message: '' });
        Clean_HiddenValue();

        $('#SysID').val($('#QuerySysID').val());

        $('#ExecAction').val(_ActionTypeAdd);
        return true;
    }
    return false;
}

//----Private Function----//
function Clean_HiddenValue() {
    $('#SysID').val('');
    $('#APIGroupID').val('');
}