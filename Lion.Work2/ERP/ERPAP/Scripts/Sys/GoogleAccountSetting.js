var _formElement;

function GoogleAccountSettingForm_onLoad(formElement) {
    _formElement = formElement;
    var table = $('#GoogleAccountSettingTable', _formElement);
    
    if (table.length > 0) {
        table.hide();
        var width = $(window).width();
        table.show();
        var height = $(window).height() - table.offset().top;
        $('.tblsearch,table[id=Pager]').each(function () { height -= $(this).height(); });
        table.freezePanes({ width: width + 'px', height: height + 'px', fixedCols: 0 });
    }
    
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

function UpdateButton_onClick(srcElement) {
    var result = _FormValidation();
    $('#ExecAction').val(_ActionTypeUpdate);
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