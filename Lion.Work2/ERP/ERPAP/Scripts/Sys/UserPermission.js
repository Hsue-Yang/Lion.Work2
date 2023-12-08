var _formElement;

function UserPermissionForm_onLoad(formElement) {
    _formElement = formElement;
    var table = $('#UserPermissionTable', _formElement);

    if (table.length > 0) {
        table.hide();
        var width = $(window).width();
        table.show();
        var height = $(window).height() - table.offset().top;
        $('.SelectTable').each(function () { height -= $(this).height(); });
        table.freezePanes({ width: width + 'px', height: height + 'px', fixedCols: 0 });
    }

    $('.BaseContainer').css('z-index', 0);

    $('table.tblsearch #QueryRestrictType', _formElement).combobox({
        width: 180,
        isRemoveButton: false
    });
    
    return true;
}

function LinkFunKey_onClick(srcElement, keys) {
    $.blockUI({ message: '' });

    $('#UserID').val(keys[1]);

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
        _formElement.submit();
    }
}

function Help03Button_onClick(srcElement) {
    var vMapFields = new Array(3);
    vMapFields[1] = 'QueryUserID';
    _hISearch(vMapFields, 'newwindow', _enumPUBAP + '/Help/Ishlp03', 'Name=' + encodeURI($.trim($('#QueryUserID').val())));
}